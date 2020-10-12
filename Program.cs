using System;
using System.Text;
using Newtonsoft.Json;

namespace BlockchainImplementation
{
  class Program
  {
    public static int Port = 0;
    public static Server Server = null;
    public static Client Client = new Client();
    public static Blockchain KCoin = new Blockchain();
    public static string name = null;
    public static string privateKey = null;
    public static string internalPrivateKey = null;
    public static string publicKey = null;
    public static string password = null;
    static void Main(string[] args)
    {
      KCoin.InitializeChain();
      if (args.Length == 0)
      {
        Console.WriteLine("Please Enter Port and ID");
        System.Environment.Exit(0);
      }
      else if (args.Length == 1)
      {
        Console.WriteLine("Please Enter an ID");
        System.Environment.Exit(0);
      }
      else
      {
        Port = int.Parse(args[0]);
        name = args[1];
      }

      if (Port > 0)
      {
        Server = new Server();
        Server.Start();
      }

      bool isRegister = false;
      while (isRegister == false)
      {
        Console.WriteLine($"Enter your private password: ");
        password = Console.ReadLine();
        privateKey = PrivateKey.GeneratePrivateKeyHash(name, name + password).Key;
        internalPrivateKey = string.Join("", Encoding.UTF8.GetBytes(privateKey)).Substring(0, 77);
        publicKey = PublicKey.GetPublicKeyFromPrivateKey(internalPrivateKey);
        Console.WriteLine("");
        Console.WriteLine($"Your private key: \"{privateKey}\". Please remember it for future use. We won't be able to recover it for you if you lose it.");
        Console.WriteLine($"Your public key: \"{publicKey}\".");
        isRegister = true;
      }

      Console.WriteLine("");
      Console.WriteLine($"Current user: {name} ({publicKey})");
      Console.WriteLine("======================");
      Console.WriteLine("1. Connect to server");
      Console.WriteLine("2. Add transaction");
      Console.WriteLine("3. Show blockchain");
      Console.WriteLine("4. Get balance");
      Console.WriteLine("5. End program");
      Console.WriteLine("======================");

      int option = 0;
      while (option != 5)
      {
        switch (option)
        {
          case 1:
            Console.WriteLine("Enter server URL:");
            string URL = Console.ReadLine();
            Client.Connect($"{URL}/Blockchain");
            break;

          case 2:
            Console.WriteLine("Enter recipient (Public Key):");
            string recipient = Console.ReadLine();
            Console.WriteLine("Enter amount:");
            string amount = Console.ReadLine();

            // create new transaction
            string message = $"{amount}:{publicKey}:{recipient}";
            string signature = Signature.GetSignature(internalPrivateKey, message);
            KCoin.CreateTransaction(new Transaction(publicKey, recipient, int.Parse(amount), Signature.GetSignature(internalPrivateKey, message)));

            // process transactions
            KCoin.ProcessPendingTransactions(publicKey);

            // send data to connected
            Client.Broadcast(JsonConvert.SerializeObject(KCoin));
            break;

          case 3:
            Console.WriteLine(JsonConvert.SerializeObject(KCoin, Formatting.Indented));
            break;
            
          case 4:
            Console.WriteLine("Enter account name: ");
            string address = Console.ReadLine();
            int balance = KCoin.GetBalance(address);
            if (balance == -1)
            {
              Console.WriteLine("Account does not exist.");
            }
            else
            {
              Console.WriteLine($"Account({address}) Balance: {balance}");
            }
            break;
        }
        Console.WriteLine("Select an option: ");
        option = int.Parse(Console.ReadLine());
      }
      Client.Close();
    }
  }
}

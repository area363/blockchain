using System;
using System.Collections.Generic;
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
    static void Main(string[] args)
    {
      KCoin.InitializeChain();
      if (args.Length == 0)
      {
        Console.WriteLine("Please Enter Port and Name");
        System.Environment.Exit(0);
      }
      else if (args.Length == 1)
      {
        Console.WriteLine("Please Enter a Name");
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

      Console.WriteLine($"Current user: {name}");
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
            Console.WriteLine("Enter recipient:");
            string recipient = Console.ReadLine();
            Console.WriteLine("Enter amount:");
            string amount = Console.ReadLine();
            // create new transaction
            KCoin.CreateTransaction(new Transaction(name, recipient, int.Parse(amount)));
            // process transactions
            KCoin.ProcessPendingTransactions(name);
            // send data to connected nodes
            Client.Broadcast(JsonConvert.SerializeObject(KCoin));
            break;
          case 3:
            Console.WriteLine(JsonConvert.SerializeObject(KCoin, Formatting.Indented));
            break;
          case 4:
            Console.WriteLine("Enter account name: ");
            string address = Console.ReadLine();
            Console.WriteLine(KCoin.GetBalance(address));
            break;
        }
        Console.WriteLine("Select an option: ");
        option = int.Parse(Console.ReadLine());
      }
      Client.Close();
    }
  }
}

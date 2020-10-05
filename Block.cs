using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace BlockchainImplementation
{
  public class Block
  {
    public int Index { get; set; }
    public DateTimeOffset TimeStamp { get; set; }
    public byte [] PrevHash { get; set; }
    public byte [] Hash { get; set; }
    public IList<Transaction> Transactions { get; set; }
    public int Nonce { get; set; }

    // block structure
    public Block (DateTimeOffset timeStamp, byte [] prevHash, IList<Transaction> transactions)
    {
      Index = 0;
      TimeStamp = timeStamp;
      PrevHash = prevHash;
      Transactions = transactions;
      Hash = CalculateHash();
    }

    // method for calculating hash
    public byte[] CalculateHash()
    {
      SHA256 sha256 = SHA256.Create();
      byte[] input = Encoding.ASCII.GetBytes($"{Index}-{TimeStamp}-{PrevHash}-{Transactions}-{Nonce}");
      
      return sha256.ComputeHash(input);
    }

    public void Mine(int difficulty)
    {
      string leadingZeroes = new string('0', difficulty);
      while (this.Hash == null || Convert.ToBase64String(this.Hash).Substring(0, difficulty) != leadingZeroes)
      {
      // Console.WriteLine($"1. {leadingZeroes}");
      // Console.WriteLine($"1. {Convert.ToBase64String(this.Hash)}");
      // Console.WriteLine($"1. {Convert.ToBase64String(this.Hash).Substring(0, difficulty)}");
        this.Nonce++;
        this.Hash = this.CalculateHash();
      }
      // Console.WriteLine(this.Nonce);
      // Console.WriteLine($"2. {leadingZeroes}");
      // Console.WriteLine($"2. {Convert.ToBase64String(this.Hash)}");
      // Console.WriteLine($"2. {Convert.ToBase64String(this.Hash).Substring(0, difficulty)}");
    }
  }
}
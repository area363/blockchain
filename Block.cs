using System;

namespace BlockchainImplementation
{
  public class Block
  {
    public int Index { get; set; }
    public DateTimeOffset TimeStamp { get; set; }
    public byte [] PrevHash { get; set; }
    public byte [] Hash { get; set; }
    public string Data { get; set; }

    // block structure
    public Block (DateTimeOffset timeStamp, byte [] prevHash, string data)
    {
      Index = 0;
      TimeStamp = timeStamp;
      PrevHash = prevHash;
      Data = data;
      Hash = CalculateHash();
    }

    // method for calculating hash
    public byte[] CalculateHash()
    {
      Random rnd = new Random();
      Byte[] hash = new Byte[1024];
      rnd.NextBytes(hash);
      return hash;
    }
  }
}
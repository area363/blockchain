using System;
using System.Security.Cryptography;
using System.Text;

namespace BlockchainImplementation
{
  class PrivateKey
  {
    public string ID { get; set; }
    public string Key { get; set; }

    public PrivateKey(string id, string privatekey)
    {
      ID = id;
      Key = privatekey;
    }

    // method to generate private key hash
    public static PrivateKey GeneratePrivateKeyHash(string id, string privatekey)
    {
      SHA256 sha256 = SHA256.Create();
      byte[] input = Encoding.ASCII.GetBytes($"{privatekey}");
      var key = sha256.ComputeHash(input);
      var pvkey = new PrivateKey(id, Convert.ToBase64String(key));
      return pvkey;
    }
  }


}
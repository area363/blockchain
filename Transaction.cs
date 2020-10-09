namespace BlockchainImplementation
{
  public class Transaction
  {
    public string Sender { get; set; }
    public string Recipient { get; set; }
    public int Amount { get; set; }
    public string Signature { get; set; }

    public Transaction(string sender, string recipient, int amount, string signature)
    {
      Sender = sender;
      Recipient = recipient;
      Amount = amount;
      Signature = signature;
    }
  }
}
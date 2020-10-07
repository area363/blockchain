namespace BlockchainImplementation
{
  public class Account
  {
    public string Address { get; set; }
    public int Balance { get; set; } = 0;

    public Account(string address, int amount)
    {
      Address = address;
      Balance = CalculateBalance(amount);
    }

    public int CalculateBalance(int amount)
    {
      return this.Balance + amount;
    }
  }
}
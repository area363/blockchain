using System;
using System.Collections.Generic;

namespace BlockchainImplementation
{
  public class Blockchain
  {
    public IList<Transaction> PendingTransactions = new List<Transaction>();
    public IList<Block> Chain { set; get; }
    public int Difficulty { set; get; } = 1;
    public int Reward = 1;
    
    // initial methods to run
    public Blockchain() 
    {
      InitializeChain();
      AddGenesisBlock();
    }

    // initialize chain with a new list
    public void InitializeChain()
    {
      Chain = new List<Block>();
    }

    // add genesis block on chain
    public void AddGenesisBlock()
    { 
      Block block = new Block(DateTimeOffset.Now, null, PendingTransactions);
      block.Mine(Difficulty);
      PendingTransactions = new List<Transaction>();
      Chain.Add(block);
    }

    // add new block
    public void AddBlock(Block block)
    {
      Block lastBlock = GetLastBlock();
      block.Index = lastBlock.Index + 1;
      block.PrevHash = lastBlock.Hash;
      block.Hash = block.CalculateHash();
      block.Mine(this.Difficulty);
      Chain.Add(block);
    }

    // get the last block
    public Block GetLastBlock()
    {
      return Chain[Chain.Count -1];
    }

    // add transaction to pending transaction list
    public void CreateTransaction(Transaction transaction)
    {
      PendingTransactions.Add(transaction);
    }

    // process transaction
    public void ProcessPendingTransactions(string address)
    {
      CreateTransaction(new Transaction(null, address, Reward));
      Block block = new Block(DateTime.Now, GetLastBlock().Hash, PendingTransactions);
      AddBlock(block);

    }

    // get balance of address
    public int GetBalance(string address)
    {
      int balance = 0;
      int spending = 0;
      int income = 0;
      
      foreach (Block block in Chain)
      {
        IList<Transaction> transactions = block.Transactions;

        foreach(Transaction transaction in transactions)
        {
          string sender = transaction.Sender;
          string recipient = transaction.Recipient;

          if (address == sender) 
          {
            spending += transaction.Amount;
          }
          if (address == recipient) 
          {
            income += transaction.Amount;
          }
          balance = income - spending;
        }
      }
      return balance;
    }

    // validate chain by comparing hash values;
    public bool ValidateChain()
    {
      for (int i = 1; i < Chain.Count; i++)
      {
        Block currentBlock = Chain[i];
        Block previousBlock = Chain[i - 1];
        for (int j = 0; j < currentBlock.Hash.Length; j++) 
        {
          if (currentBlock.Hash[j] != currentBlock.CalculateHash()[j])
          {
            return false;
          }
        }
        for (int j = 0; j < currentBlock.PrevHash.Length; j++) 
        {
          if (currentBlock.PrevHash[j] != previousBlock.Hash[j])
          {
            return false;
          }
        }
      }
      return true;
    }
  }
}
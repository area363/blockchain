using System;
using System.Collections.Generic;

namespace BlockchainImplementation
{
  public class Blockchain
  {
    public IList<Transaction> PendingTransactions = new List<Transaction>();
    public IList<Block> Chain { set; get; }
    public int Difficulty { set; get; } = 2;
    public int Reward = 1;

    // initialize chain and add genesis block
    public void InitializeChain()
    {
      Chain = new List<Block>();
      AddGenesisBlock();
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
      block.Mine(Difficulty);
      Chain.Add(block);
    }

    // get the last block
    public Block GetLastBlock()
    {
      return Chain[Chain.Count - 1];
    }

    // add transaction to pending transaction list
    public void CreateTransaction(Transaction transaction)
    {
      PendingTransactions.Add(transaction);
    }

    // process transaction
    public void ProcessPendingTransactions(string address)
    {
      // create reward transaction for processing node and add block
      CreateTransaction(new Transaction("KCoin", address, Reward));
      Block block = new Block(DateTime.Now, GetLastBlock().Hash, PendingTransactions);
      AddBlock(block);
      // reset PendingTransactions list
      PendingTransactions = new List<Transaction>();
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

        // iterate over each transaction
        foreach (Transaction transaction in transactions)
        {
          string sender = transaction.Sender;
          string recipient = transaction.Recipient;

          // increase spending for sender
          if (address == sender)
          {
            spending += transaction.Amount;
          }
          //increase income for recipient
          if (address == recipient)
          {
            income += transaction.Amount;
          }
          // calculate balance
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

        // return false if the hash of current block doesn't equal the result of calculatehash()
        if (Convert.ToBase64String(currentBlock.Hash) != Convert.ToBase64String(currentBlock.CalculateHash()))
        {
          return false;
        }
        // return false if the prevhash of current block doesn't equal the hash of previous block
        if (Convert.ToBase64String(currentBlock.PrevHash) != Convert.ToBase64String(previousBlock.Hash))
        {
          return false;
        }
      }
      return true;
    }
  }
}
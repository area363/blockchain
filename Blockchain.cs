using System;
using System.Collections.Generic;
using System.Text;

namespace BlockchainImplementation
{
  public class Blockchain
  {
    public IList<Transaction> PendingTransactions = new List<Transaction>();
    public IList<Block> Chain { set; get; }
    public int Difficulty { set; get; } = 2;
    public int Reward = 1;
    public IList<Account> Accounts = new List<Account>();

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
      string KCoinPVKey = "Gs5cyGHUggSKg76veFUrw7v7/yUtzdjYV3fIhRqfSLY=";
      string internalPrivateKey = string.Join("", Encoding.UTF8.GetBytes(KCoinPVKey)).Substring(0, 77);
      string kcoinPublicKey = PublicKey.GetPublicKeyFromPrivateKeyEx(internalPrivateKey);
      string message = $"1:{kcoinPublicKey}:{address}";
      CreateTransaction(new Transaction(kcoinPublicKey, address, Reward, Signature.GetSignature(internalPrivateKey, message)));
      Block block = new Block(DateTime.Now, GetLastBlock().Hash, PendingTransactions);
      AddBlock(block);
      CalculateBalance(block);
      // reset PendingTransactions list
      PendingTransactions = new List<Transaction>();
    }

    public void CalculateBalance(Block block)
    {
      IList<Transaction> transactions = block.Transactions;

      foreach (Transaction transaction in transactions)
      {
        string sender = transaction.Sender;
        string recipient = transaction.Recipient;
        bool senderExists = false;
        bool recipientExists = false;

        foreach (Account account in Accounts)
        {
          if (account.Address == sender)
          {
            account.Balance = account.CalculateBalance(-transaction.Amount);
            senderExists = true;
          }
          if (account.Address == recipient)
          {
            account.Balance = account.CalculateBalance(transaction.Amount);
            recipientExists = true;
          }
        }

        if (!senderExists)
        {
          Accounts.Add(new Account(sender, -transaction.Amount));
        }

        if (!recipientExists)
        {
          Accounts.Add(new Account(recipient, transaction.Amount));
        }

      }
    }

    // get balance of address
    public int GetBalance(string address)
    {
      foreach (Account account in Accounts)
      {
        if (account.Address == address)
        {
          return account.Balance;
        }
      }
      return -1;
    }

    // validate chain by comparing hash values & verify signature for each transaction;
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

        foreach (var key in currentBlock.Transactions)
        {
          string message = $"{key.Amount}:{key.Sender}:{key.Recipient}";
          if (!Signature.VerifySignature(message, key.Sender, key.Signature))
          {
            return false;
          }
        }
      }
      return true;
    }
  }
}
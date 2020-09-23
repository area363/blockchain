using System;
using System.Collections.Generic;
using System.Text;

namespace BlockchainImplementation
{
  public class Blockchain
  {
    public IList<Block> Chain { set; get; }

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
      Chain.Add(new Block(DateTimeOffset.Now, null, ""));
    }

    // get the last block
    public Block GetlastBlock()
    {
      return Chain[Chain.Count -1];
    }

    // add new block
    public void AddBlock(Block block)
    {
      Block lastBlock = GetlastBlock();
      block.Index = lastBlock.Index + 1;
      block.PrevHash = lastBlock.Hash;
      block.Hash = block.CalculateHash();
      Chain.Add(block);
    }

    // validate chain by comparing hash values;
    public bool ValidateChain()
    {
      for (int i = 1; i < Chain.Count; i++)
      {
        Block currentBlock = Chain[i];
        Block previousBlock = Chain[i - 1];

        if (currentBlock.PrevHash != previousBlock.Hash)
        {
          Console.WriteLine("hi");
          return false;
        }
      }
      return true;
    }
  }
}
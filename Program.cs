using System;
using System.Collections.Generic;

namespace BlockchainImplementation
{
    class Program
    {
        static void Main(string[] args)
        {
            Blockchain bc = new Blockchain(); // initiate chain and add genesis block

            // add 4 additional blocks
            bc.AddBlock(new Block(DateTimeOffset.Now, null, "data1"));
            bc.AddBlock(new Block(DateTimeOffset.Now, null, "data2"));
            bc.AddBlock(new Block(DateTimeOffset.Now, null, "data3"));
            bc.AddBlock(new Block(DateTimeOffset.Now, null, "data4"));
            
            Console.WriteLine(bc.Chain.Count); // 5
            Console.WriteLine(bc.ValidateChain()); // true

            // change data
            Console.WriteLine(bc.Chain[4].Data);
            bc.Chain[4].Data = "falseData";
            Console.WriteLine(bc.Chain[4].Data); 

            Console.WriteLine(bc.ValidateChain()); // false, but comes out true right now
        }
    }
}

using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace BlockchainImplementation
{
    class Program
    {   
        public static int Port = 0;
        public static Server Server = null;
        public static Blockchain KidonCoin = new Blockchain();
        static void Main(string[] args)
        {
            var startTime = DateTime.Now;  
  
            Blockchain bc = new Blockchain();  
            bc.CreateTransaction(new Transaction("Don", "Jon", 100));  
            bc.CreateTransaction(new Transaction("Jon", "Don", 5));  
            bc.CreateTransaction(new Transaction("Jon", "Don", 5));  
            bc.ProcessPendingTransactions("Jake");  
            
            var endTime = DateTime.Now;  
        
            Console.WriteLine($"Duration: {endTime - startTime}");  
            Console.WriteLine("=========================");  
            Console.WriteLine($"Don' balance: {bc.GetBalance("Don")}");  
            Console.WriteLine($"Jon' balance: {bc.GetBalance("Jon")}");  
            Console.WriteLine($"Jake' balance: {bc.GetBalance("Jake")}");  
            Console.WriteLine("=========================");  
            Console.WriteLine($"bc");  
            Console.WriteLine(JsonConvert.SerializeObject(bc, Formatting.Indented));  
        
            //  Console.ReadKey();  
         
            // // add 4 additional blocks
            // bc.AddBlock(new Block(DateTimeOffset.Now, null, "data1"));
            // bc.AddBlock(new Block(DateTimeOffset.Now, null, "data2"));
            // bc.AddBlock(new Block(DateTimeOffset.Now, null, "data3"));
            // bc.AddBlock(new Block(DateTimeOffset.Now, null, "data4"));
            
            // Console.WriteLine("Chain Count: " + bc.Chain.Count); // 5
            // Console.WriteLine("Original Validation: " + bc.ValidateChain()); // true

            // // manipulate Index
            // Console.WriteLine("Manipulate Index");
            // int OriginalIndex = bc.Chain[4].Index;
            // bc.Chain[4].Index = 5;
            // Console.WriteLine("After manipulation: " + bc.ValidateChain()); // false
            // bc.Chain[4].Index = OriginalIndex;
            // Console.WriteLine("Revert: " + bc.ValidateChain()); // true

            // // manipulate TimeStamp
            // Console.WriteLine("Manipulate TimeStamp");
            // DateTimeOffset OriginalTime = bc.Chain[4].TimeStamp;
            // bc.Chain[4].TimeStamp= DateTimeOffset.UtcNow;
            // Console.WriteLine("After manipulation: " + bc.ValidateChain()); // false
            // bc.Chain[4].TimeStamp= OriginalTime;
            // Console.WriteLine("Revert: " + bc.ValidateChain()); // true

            // // manipulate Data
            // Console.WriteLine("Manipulate Data");
            // IList<Transaction> OriginalData = bc.Chain[4].Transactions;
            // bc.Chain[4].Transactions = "falseData";
            // Console.WriteLine("After manipulation: " + bc.ValidateChain()); // false
            // bc.Chain[4].Transactions = OriginalData;
            // Console.WriteLine("Revert: " + bc.ValidateChain()); // true

            // // manipulate PrevHash
            // Console.WriteLine("Manipulate PrevHash");
            // byte [] OriginalPrevHash = bc.Chain[4].PrevHash;
            // byte [] dummyHash = null;
            // bc.Chain[4].PrevHash = dummyHash;
            // Console.WriteLine("After manipulation: " + bc.ValidateChain()); // false
            // bc.Chain[4].PrevHash = OriginalPrevHash;
            // Console.WriteLine("Revert: " + bc.ValidateChain()); // true
        }
    }
}

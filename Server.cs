using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace BlockchainImplementation
{
  public class Server: WebSocketBehavior
  {
    bool chainSync = false;
    WebSocketServer server = null;

    public void Start()
    {
      server = new WebSocketServer($"ws://127.0.0.1:{Program.Port}");
      server.AddWebSocketService<Server>("/Blockchain");
      server.Start();
      Console.WriteLine($"Server started at ws://127.0.0.1:{Program.Port}");
    }

    protected override void OnMessage(MessageEventArgs e)
    {
      if (e.Data == "Hi Server")
      {
        Console.WriteLine(e.Data);
        Send("Hi Client");
      }
      else 
      {
        Blockchain newChain = JsonConvert.DeserializeObject<Blockchain>(e.Data);

        if (newChain.ValidateChain() && newChain.Chain.Count > Program.KidonCoin.Chain.Count)
        {
          List<Transaction> newTransactions = new List<Transaction>();
          newTransactions.AddRange(newChain.PendingTransactions);
          newTransactions.AddRange(Program.KidonCoin.PendingTransactions);

          newChain.PendingTransactions = newTransactions;
          Program.KidonCoin = newChain;
        }

        if (!chainSync)
        {
          Send(JsonConvert.SerializeObject(Program.KidonCoin));
          chainSync = true;
        }
      }
    }
  }
}

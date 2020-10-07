using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using WebSocketSharp.NetCore;
using WebSocketSharp.NetCore.Server;

namespace BlockchainImplementation
{
  public class Server : WebSocketBehavior
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
      if (e.Data == "Connected to Client")
      {
        Console.WriteLine(e.Data);
        Send("Connected to Server");
      }
      else
      {
        Blockchain newChain = JsonConvert.DeserializeObject<Blockchain>(e.Data);

        if (newChain.ValidateChain() && newChain.Chain.Count > Program.KCoin.Chain.Count)
        {
          List<Transaction> newTransactions = new List<Transaction>();
          newTransactions.AddRange(newChain.PendingTransactions);
          newTransactions.AddRange(Program.KCoin.PendingTransactions);

          newChain.PendingTransactions = newTransactions;
          Program.KCoin = newChain;
        }

        if (!chainSync)
        {
          Send(JsonConvert.SerializeObject(Program.KCoin));
          chainSync = true;
        }
      }
    }
  }
}

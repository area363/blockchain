using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using WebSocketSharp.NetCore;

namespace BlockchainImplementation
{
  public class Client
  {
    IDictionary<string, WebSocket> websocketDict = new Dictionary<string, WebSocket>();

    public void Connect(string url)
    {
      if (!websocketDict.ContainsKey(url))
      {
        WebSocket websocket = new WebSocket(url);
        websocket.OnMessage += (sender, e) =>
        {
          // print message if message received from server is "Connected to Server"
          if (e.Data == "Connected to Server")
          {
            Console.WriteLine(e.Data);
          }
          else
          {
            // if message is a broadcasted blockchain, verify chain and update node's chain
            Blockchain newChain = JsonConvert.DeserializeObject<Blockchain>(e.Data);
            if (newChain.ValidateChain() && newChain.Chain.Count > Program.KCoin.Chain.Count)
            {
              List<Transaction> newTransactions = new List<Transaction>();
              newTransactions.AddRange(newChain.PendingTransactions);
              newTransactions.AddRange(Program.KCoin.PendingTransactions);

              newChain.PendingTransactions = newTransactions;
              Program.KCoin = newChain;
            }
          }
        };
        websocket.Connect();
        websocket.Send("Connected to Client");
        websocket.Send(JsonConvert.SerializeObject(Program.KCoin));
        websocketDict.Add(url, websocket);
      }
    }

    // method for sending data to connected nodes
    public void Broadcast(string data)
    {
      foreach (var item in websocketDict)
      {
        item.Value.Send(data);
      }
    }

    // method for closing client
    public void Close()
    {
      foreach (var item in websocketDict)
      {
        item.Value.Close();
      }
    }
  }
}
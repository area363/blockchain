using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using WebSocketSharp;

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
          if (e.Data == "Hi Client")
          {
            Console.WriteLine(e.Data);
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
          }
        };
        websocket.Connect();
        websocket.Send("Hello Server");
        websocket.Send(JsonConvert.SerializeObject(Program.KCoin));
        websocketDict.Add(url, websocket);
      }
    }

    public void Send(string url, string data)
    {
      foreach (var item in websocketDict)
      {
        if (item.Key == url)
        {
          item.Value.Send(data);
        }
      }
    }
  }
}
using System;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using WebSocketSharp;
public class WebsocketClient : MonoBehaviour
{
    private WebSocket ws;
    private string serverUrl;
    private string IP;
    private byte[] serverMessage;
    [SerializeField] private string port = "12348";
    [SerializeField] private PointLandmarkVisualizer pointLandmarkVisualizer;
    [SerializeField] private TMP_InputField inputField;

    // Update is called once per frame
    void Update()
    {
        if (pointLandmarkVisualizer is not null && ws is not null && ws.IsAlive)
        {
            var messageToSend = pointLandmarkVisualizer.GetLandmarkPointData();
            foreach (var point in messageToSend)
            {
                serverMessage = new byte[point.Length * sizeof(float)];
                Buffer.BlockCopy(point, 0, serverMessage, 0, serverMessage.Length);
                ws.Send(serverMessage);
            }
        }
    }

    public void InitWebsocketClient()
    {
        if(ws is not null && !ws.IsAlive) return;
        try
        {
            IP = inputField.text;
            Debug.Log(IP);
            Debug.Log(port);
            serverUrl = $"ws://{IP}:{port}";

            ws = new WebSocket(serverUrl);
            ws.OnOpen += OnServerOpen;
            ws.Connect();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        
    }

    private void OnServerOpen(object sender, EventArgs e)
    {
        print("Server open");
    }
}

using System;
using TMPro;
using UnityEngine;
using WebSocketSharp;
public class WebsocketClient : MonoBehaviour
{
    private WebSocket ws;
    private string serverUrl;
    private string IP;
    private byte[] serverMessage;
    private float startTime;
    private float frameCount = -1;
    [SerializeField] private string port = "12348";
    [SerializeField] private PointLandmarkVisualizer pointLandmarkVisualizer;
    [SerializeField] private TMP_InputField inputField;

    // Update is called once per frame
    void Update()
    {
        ConvertAndSendMessage();
    }

    private void ConvertAndSendMessage()
    {
        if (pointLandmarkVisualizer is null || ws is null || !ws.IsAlive) return;
        
        frameCount++;
        var messageToSend = new []{frameCount, Time.time - startTime};
        serverMessage = new byte[messageToSend.Length * sizeof(float)];
        Buffer.BlockCopy(messageToSend, 0, serverMessage, 0, serverMessage.Length);
        ws.Send(serverMessage);
            
        foreach (var point in pointLandmarkVisualizer.GetLandmarkPointData())
        {
            serverMessage = new byte[point.Length * sizeof(float)];
            Buffer.BlockCopy(point, 0, serverMessage, 0, serverMessage.Length);
            ws.Send(serverMessage);
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
        startTime = Time.time;
    }

    private void OnApplicationQuit()
    {
        ws.Close();
    }
}

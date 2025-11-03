using System;
using TMPro;
using UnityEngine;
using WebSocketSharp;
using static BodyLandmarks;
public class WebsocketClient : MonoBehaviour
{
    private WebSocket ws;
    private string serverUrl;
    private string IP;
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
        SendMessage(messageToSend);
            
        foreach (var point in pointLandmarkVisualizer.GetLandmarkPointData())
        {
            SendMessage(point);
        }
    }

    public void InitWebsocketClient()
    {
        if(ws is not null && !ws.IsAlive) return;
        try
        {
            IP = inputField.text;
            Debug.Log(IP);
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
        InitialLog();
    }

    private void InitialLog()
    {
        var pointCount = new []{Convert.ToSingle(Landmarks.Count)};
        SendMessage(pointCount);

        foreach (var pair in LandmarkPairs)
        {
            SendMessage(new [] {pair.X, pair.Y});
        }
    }

    private void SendMessage(float[] messageToSend)
    {
        var serverMessage = new byte[messageToSend.Length * sizeof(float)];
        Buffer.BlockCopy(messageToSend, 0, serverMessage, 0, serverMessage.Length);
        ws.Send(serverMessage);
    }
    
    private void OnApplicationQuit()
    {
        ws.Close();
    }
}

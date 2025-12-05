using System;
using TMPro;
using UnityEngine;
using WebSocketSharp;
using static BodyLandmarks;
using Enum = System.Enum;

public class WebsocketClient : MonoBehaviour
{
    [SerializeField] private string port = "80";
    [SerializeField] private TMP_InputField inputField;
    
    private PointLandmarkVisualizer pointLandmarkVisualizer;
    private WebSocket ws;
    private string serverUrl;
    private string IP;
    private float startTime;
    private float frameCount = -1;
    private bool startServerLog;
    
    // Update is called once per frame
    private void Start()
    {
        pointLandmarkVisualizer = FindFirstObjectByType<PointLandmarkVisualizer>();
    }
    
    void Update()
    {
        if(startServerLog) ConvertAndSendMessage();
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

    public bool InitWebsocketClient()
    {
        if(ws is not null && !ws.IsAlive) return false;
        try
        {
            IP = inputField.text;
            Debug.Log(IP);
            serverUrl = $"ws://{IP}:{port}";

            ws = new WebSocket(serverUrl);
            ws.OnOpen += OnServerOpen;
            ws.Connect();
            if (ws.IsAlive) return ws.IsAlive;
            
            ws = null;
            return false;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return false;
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
        var pointCount = new []{Convert.ToSingle(Enum.GetValues(typeof(PoseLandmark)).Length)};
        SendMessage(pointCount);

        foreach (var point in PoseLandmarkPairs.Keys)
        {
            if(PoseLandmarkPairs[point] == null) continue;
            foreach (var pair in PoseLandmarkPairs[point])
            {
                SendMessage(new [] {(float)point, (float)pair});
            }
            
        }
    }

    private void SendMessage(float[] messageToSend)
    {
        var serverMessage = new byte[messageToSend.Length * sizeof(float)];
        Buffer.BlockCopy(messageToSend, 0, serverMessage, 0, serverMessage.Length);
        ws.Send(serverMessage);
    }

    public void StartServerLog()
    {
        startServerLog = true;
    }
    
    private void OnApplicationQuit()
    {
        ws.Close();
    }
}

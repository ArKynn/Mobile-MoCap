using System;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using UnityEngine;
using WebSocketSharp;
public class WebsocketClient : MonoBehaviour
{
    private WebSocket ws;
    private string serverUrl;
    private string IP;
    [SerializeField] private string port = "12348";
    [SerializeField] private PointLandmarkVisualizer pointLandmarkVisualizer;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InitWebsocketClient();
    }

    // Update is called once per frame
    void Update()
    {
        if (pointLandmarkVisualizer && ws.IsAlive)
        {
            ws.Send(pointLandmarkVisualizer.GetLandmarkPointData());
        }
    }

    private void GetWifiIP()
    {
        foreach (NetworkInterface ni in NetworkInterface.GetAllNetworkInterfaces())
        {
            if (ni.OperationalStatus == OperationalStatus.Up)
            {
                if (ni.NetworkInterfaceType == NetworkInterfaceType.Wireless80211)
                {
                    foreach (UnicastIPAddressInformation ip in ni.GetIPProperties().UnicastAddresses)
                    {
                        if (ip != null && ip.Address.AddressFamily == AddressFamily.InterNetwork)
                        {
                            IP = ip.Address.ToString();
                        }
                    }
                }
            }
        }
    }

    private void InitWebsocketClient()
    {
        GetWifiIP();
        serverUrl = $"ws://{IP}:{port}";

        ws = new WebSocket(serverUrl);
        ws.OnOpen += OnServerOpen;
        ws.Connect();
    }

    private void OnServerOpen(object sender, EventArgs e)
    {
        print("Server open");
        ws.Send("Client Connected");
    }
}

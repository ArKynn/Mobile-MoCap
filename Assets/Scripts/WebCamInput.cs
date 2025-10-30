using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class WebCamInput : MonoBehaviour
{
    [SerializeField] string webCamName;
    [SerializeField] Vector2 webCamResolution = new Vector2(1080, 1920);
    [SerializeField] RawImage image;
    [SerializeField] CanvasScaler scaler;

    private WebCamTexture _webCamTexture;
    public WebCamTexture WebCamTexture => _webCamTexture;
    public RenderTexture inputRT;
    
    private WebCamDevice[] _devices;
    private int _currentDevice;
    private WebCamDevice CurrentDevice => _devices[_currentDevice];

    private void Awake()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        Screen.orientation = ScreenOrientation.Portrait;
    }

    void Start()
    {
        inputRT = new RenderTexture((int)webCamResolution.x, (int)webCamResolution.y, 0)
        {
            enableRandomWrite = true
        };
        InitializeWebcam();   
    }

    void InitializeWebcam()
    {
        _devices = WebCamTexture.devices;
        _webCamTexture = new WebCamTexture();
        UpdateWebcam();
        UpdateRawImage();
    }

    public void NextWebcamDevice()
    {
        if (_currentDevice + 1 <= _devices.Length - 1)
        {
            _currentDevice++;
        }
        else _currentDevice = 0;
        
        UpdateWebcam();
        UpdateRawImage();
    }
    
    void UpdateWebcam()
    {
        _webCamTexture.Stop();
        _webCamTexture.deviceName = CurrentDevice.name;
        _webCamTexture.requestedWidth = inputRT.width;
        _webCamTexture.requestedHeight = inputRT.height;
        _webCamTexture.Play();
    }

    void UpdateRawImage()
    {
        scaler.referenceResolution = new Vector2(_webCamTexture.requestedWidth, _webCamTexture.requestedHeight);
        image.rectTransform.sizeDelta = new Vector2(_webCamTexture.requestedWidth, _webCamTexture.requestedHeight);
    }

    void Update()
    {
        if(!_webCamTexture.didUpdateThisFrame) return;
        
        Graphics.Blit(_webCamTexture, inputRT); 
        
        image.texture = inputRT;
    }

    void OnDestroy(){
        if (_webCamTexture != null) Destroy(_webCamTexture);
        if (inputRT != null) Destroy(inputRT);
    }
}
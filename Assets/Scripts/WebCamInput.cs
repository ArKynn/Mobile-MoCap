using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class WebCamInput : MonoBehaviour
{
    [SerializeField] string webCamName;
    [SerializeField] Vector2 webCamResolution = new Vector2(1080, 1920);
    [SerializeField] PointLandmarkVisualizer pointLandmarkVisualizer;
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
#if UNITY_ANDROID
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        Screen.orientation = ScreenOrientation.Portrait;
#endif
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
    }

    public void NextWebcamDevice()
    {
        if (_currentDevice + 1 <= _devices.Length - 1)
        {
            _currentDevice++;
        }
        else _currentDevice = 0;
        
        UpdateWebcam();
    }
    
    void UpdateWebcam()
    {
        _webCamTexture.Stop();
        _webCamTexture.deviceName = CurrentDevice.name;
#if UNITY_ANDROID
        _webCamTexture.requestedWidth = 1080;
        _webCamTexture.requestedHeight = 1920;
#endif
        _webCamTexture.Play();
    }

    void Update()
    {
        if(!_webCamTexture.didUpdateThisFrame) return;
        
        Graphics.Blit(_webCamTexture, inputRT);

#if UNITY_ANDROID
        image.texture = pointLandmarkVisualizer.Detecter.OutputTexture;
#else
        image.texture = inputRT;
#endif

    }

    void OnDestroy(){
        if (_webCamTexture != null) Destroy(_webCamTexture);
        if (inputRT != null) Destroy(inputRT);
    }
}
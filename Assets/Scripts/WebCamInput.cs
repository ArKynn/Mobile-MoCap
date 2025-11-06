using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class WebCamInput : MonoBehaviour
{
    [SerializeField] Vector2 webCamResolution = new Vector2(1080, 1920);
    [SerializeField] RawImage image;
    
    private PointLandmarkVisualizer _pointLandmarkVisualizer;
    private WebCamTexture _webCamTexture;
    public WebCamTexture WebCamTexture => _webCamTexture;
    [NonSerialized] public RenderTexture inputRT;
    
    private WebCamDevice[] _devices;
    private int _currentDevice;
    private WebCamDevice CurrentDevice => _devices[_currentDevice];

    private void Awake()
    {
        _pointLandmarkVisualizer = FindFirstObjectByType<PointLandmarkVisualizer>();
        
#if UNITY_ANDROID
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        Screen.orientation = ScreenOrientation.Portrait;
#endif
    }

    void Start()
    {
        var desc = new RenderTextureDescriptor((int)webCamResolution.x, (int)webCamResolution.y)
        {
            colorFormat = RenderTextureFormat.Default,
            sRGB = false
        };
        
        inputRT = new RenderTexture(desc)
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
        _webCamTexture.requestedFPS = 60;
#if UNITY_ANDROID
        _webCamTexture.requestedWidth = 1080;
        _webCamTexture.requestedHeight = 1920;
#endif
        _webCamTexture.Play();
        _pointLandmarkVisualizer.transform.rotation = Quaternion.Euler(0, 0, -_webCamTexture.videoRotationAngle);
    }

    void Update()
    {
        if(!_webCamTexture.didUpdateThisFrame) return;
        
        Graphics.Blit(_webCamTexture, inputRT);
        
        //image.texture = inputRT;
    }

    void OnDestroy(){
        if (_webCamTexture != null) Destroy(_webCamTexture);
        if (inputRT != null) Destroy(inputRT);
    }
}
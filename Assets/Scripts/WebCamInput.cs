using System;
using UnityEngine;
using UnityEngine.UI;

public class WebCamInput : MonoBehaviour
{
    [SerializeField] string webCamName;
    [SerializeField] Vector2 webCamResolution = new Vector2(1920, 1080);
    [SerializeField] RawImage image;

    // Provide input image Texture.
    public Texture InputImageTexture => inputRT;

    private WebCamTexture _webCamTexture;
    private Texture2D _rotatedTexture;
    public RenderTexture inputRT;
    
    private WebCamDevice[] _devices;
    private int _currentDevice;
    private WebCamDevice CurrentDevice => _devices[_currentDevice];

    private void Awake()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }

    void Start()
    {
        inputRT = new RenderTexture((int)webCamResolution.x, (int)webCamResolution.y, 0);
        
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
        _webCamTexture.requestedWidth = inputRT.width;
        _webCamTexture.requestedHeight = inputRT.height;
        _webCamTexture.Play();
    }

    void Update()
    {
        if(!_webCamTexture.didUpdateThisFrame) return;

        //_rotatedTexture = RotateTexture(_webCamTexture, false);
        
        Graphics.Blit(_webCamTexture, inputRT); 
    }

    void LateUpdate()
    {
        image.texture = inputRT;
    }

    void OnDestroy(){
        if (_webCamTexture != null) Destroy(_webCamTexture);
        if (inputRT != null) Destroy(inputRT);
    }

    
    //Code by KwahuNashoba (https://discussions.unity.com/t/rotate-the-contents-of-a-texture/136686)
    Texture2D RotateTexture(WebCamTexture originalTexture, bool clockwise)
    {
        Color32[] original = originalTexture.GetPixels32();
        Color32[] rotated = new Color32[original.Length];
        int w = originalTexture.width;
        int h = originalTexture.height;

        int iRotated, iOriginal;

        for (int j = 0; j < h; ++j)
        {
            for (int i = 0; i < w; ++i)
            {
                iRotated = (i + 1) * h - j - 1;
                iOriginal = clockwise ? original.Length - 1 - (j * w + i) : j * w + i;
                rotated[iRotated] = original[iOriginal];
            }
        }

        Texture2D rotatedTexture = new Texture2D(h, w);
        rotatedTexture.SetPixels32(rotated);
        rotatedTexture.Apply();
        return rotatedTexture;
    }
}
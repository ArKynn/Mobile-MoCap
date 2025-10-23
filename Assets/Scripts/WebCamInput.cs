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

    // Provide input image Texture.
    public Texture InputImageTexture => inputRT;

    private WebCamTexture _webCamTexture;
    private Texture2D _tempTexture;
    public RenderTexture inputRT;
    [FormerlySerializedAs("visualizerInputRT")] public RenderTexture imageInputRT;
    
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
        inputRT = new RenderTexture((int)webCamResolution.x, (int)webCamResolution.y, 0);
        imageInputRT = new RenderTexture((int)webCamResolution.y, (int)webCamResolution.x, 0);
        InitializeWebcam();   
        _tempTexture = new Texture2D(_webCamTexture.width, _webCamTexture.height, TextureFormat.RGBA32, false);
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

        _tempTexture.SetPixels(_webCamTexture.GetPixels());
        _tempTexture.Apply();
        
        //_tempTexture = RotateTexture(_tempTexture, false);
        RotateImage(_tempTexture, 90f);
        
        Graphics.Blit(_webCamTexture, imageInputRT);
        Graphics.Blit(_tempTexture, inputRT); 
        
        image.texture = imageInputRT;
    }

    void OnDestroy(){
        if (_webCamTexture != null) Destroy(_webCamTexture);
        if (inputRT != null) Destroy(inputRT);
    }
    
    public static void RotateImage(Texture2D tex, float angleDegrees)
    {
        int width = tex.width;
        int height = tex.height;
        float halfHeight = height * 0.5f;
        float halfWidth = width * 0.5f;

        var texels = tex.GetRawTextureData<Color32>();
        var copy = System.Buffers.ArrayPool<Color32>.Shared.Rent(texels.Length);
        Unity.Collections.NativeArray<Color32>.Copy(texels, copy, texels.Length);

        float phi = Mathf.Deg2Rad * angleDegrees;
        float cosPhi = Mathf.Cos(phi);
        float sinPhi = Mathf.Sin(phi);

        int address = 0;
        for (int newY = 0; newY < height; newY++)
        {
            for (int newX = 0; newX < width; newX++)
            {
                float cX = newX - halfWidth;
                float cY = newY - halfHeight;
                int oldX = Mathf.RoundToInt(cosPhi * cX + sinPhi * cY + halfWidth);
                int oldY = Mathf.RoundToInt(-sinPhi * cX + cosPhi * cY + halfHeight);
                bool InsideImageBounds = (oldX > -1) & (oldX < width)
                                                     & (oldY > -1) & (oldY < height);

                texels[address++] = InsideImageBounds ? copy[oldY * width + oldX] : default;
            }
        }

        // No need to reinitialize or SetPixels - data is already in-place.
        tex.Apply(true);

        System.Buffers.ArrayPool<Color32>.Shared.Return(copy);
    }
}
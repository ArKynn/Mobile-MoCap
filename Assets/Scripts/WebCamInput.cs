using UnityEngine;
using UnityEngine.UI;

public class WebCamInput : MonoBehaviour
{
    [SerializeField] string webCamName;
    [SerializeField] Vector2 webCamResolution = new Vector2(1920, 1080);
    [SerializeField] RawImage image;

    // Provide input image Texture.
    public Texture inputImageTexture => inputRT;

    WebCamTexture webCamTexture;
    public RenderTexture inputRT;
    
    private WebCamDevice[] _devices;
    private int _currentDevice;
    private WebCamDevice CurrentDevice => _devices[_currentDevice];
    
    private Resolution _webcamResolution;

    void Start()
    {
        inputRT = new RenderTexture((int)webCamResolution.x, (int)webCamResolution.y, 0);
        
        InitializeWebcam();        
    }

    void InitializeWebcam()
    {
        _devices = WebCamTexture.devices;
        webCamTexture = new WebCamTexture();
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
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
        UpdateWebcamWindows(CurrentDevice.name);
#elif UNITY_ANDROID
        UpdateWebcamAndroid(CurrentDevice.name, CurrentDevice.availableResolutions[0]);
#endif
    }

    void UpdateWebcamAndroid(string name, Resolution resolution)
    {
        webCamTexture.Stop();
        webCamTexture.deviceName = name;
        webCamTexture.width = resolution.width;
        webCamTexture.height = resolution.height;
        webCamTexture.Play();
    }

    void UpdateWebcamWindows(string name)
    {
        webCamTexture.Stop();
        webCamTexture.deviceName = name;
        webCamTexture.Play();
    }

    void Update()
    {
        if(!webCamTexture.didUpdateThisFrame) return;

        var aspect1 = (float)webCamTexture.width / webCamTexture.height;
        var aspect2 = (float)inputRT.width / inputRT.height;
        var aspectGap = aspect2 / aspect1;

        var vMirrored = webCamTexture.videoVerticallyMirrored;
        var scale = new Vector2(aspectGap, vMirrored ? -1 : 1);
        var offset = new Vector2((1 - aspectGap) / 2, vMirrored ? 1 : 0);

        Graphics.Blit(webCamTexture, inputRT, scale, offset);
    }

    void LateUpdate()
    {
        image.texture = inputRT;
    }

    void OnDestroy(){
        if (webCamTexture != null) Destroy(webCamTexture);
        if (inputRT != null) Destroy(inputRT);
    }
}
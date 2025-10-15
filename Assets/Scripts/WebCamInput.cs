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

    void Start()
    {
        InitializeWebcam();        

        inputRT = new RenderTexture((int)webCamResolution.x, (int)webCamResolution.y, 0);
    }

    void InitializeWebcam()
    {
        _devices = WebCamTexture.devices;
        
        webCamTexture = new WebCamTexture(_devices[_currentDevice].name);
        webCamTexture.Play();
    }

    public void NextWebcamDevice()
    {
        webCamTexture.Stop();
        if (_currentDevice + 1 <= _devices.Length - 1)
        {
            _currentDevice++;
        }
        else _currentDevice = 0;
        
        webCamTexture.deviceName = _devices[_currentDevice].name;
        webCamTexture.Play();
        
        print ("Webcam Device: " + webCamTexture.deviceName);
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
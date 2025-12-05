using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class UIController : MonoBehaviour
{

    [SerializeField] private GameObject settingsMenu;
    [FormerlySerializedAs("poseSaveTimerGraphic")] [SerializeField] private TMP_Text Timer;
    [SerializeField] private float poseSaveTimerStart;
    [SerializeField] private GameObject poseSimilarityObject;
    [SerializeField] private TMP_Text poseSimilarityText;
    [SerializeField] private GameObject serverLogStartButton;
    [SerializeField] private float serverLogTimerStart = 10;
    
    private WebsocketClient wsClient;
    private PoseSaver poseSaver;
    private float poseSaveTimer;
    private bool isPoseSaving;
    private float serverLogTimer;
    private bool startServerLogCountdown;

    private void Start()
    {
        wsClient = FindFirstObjectByType<WebsocketClient>();
        poseSaver = FindFirstObjectByType<PoseSaver>();
    }

    private void Update()
    {
        if(startServerLogCountdown) UpdateServerCountdown();
        
        if(isPoseSaving) UpdatePoseCountdown();
    }
    
    public void ToggleSettings()
    {
        settingsMenu?.SetActive(!settingsMenu.activeSelf);
    }

    public void StartPoseSave()
    {
        if(startServerLogCountdown) return;
        
        isPoseSaving = true;
        poseSaveTimer = poseSaveTimerStart;
        ToggleSettings();
        Timer.GameObject().transform.parent.GameObject().SetActive(true);
        poseSimilarityObject.SetActive(false);
    }

    private void UpdatePoseCountdown()
    {
        poseSaveTimer -= Time.deltaTime;
        Timer.text = poseSaveTimer.ToString("0.00");
        if (poseSaveTimer > 0) return;
        
        isPoseSaving = false;
        Timer.GameObject().transform.parent.GameObject().SetActive(false);
        poseSimilarityObject.SetActive(true);
        poseSaver.SaveCurrentPose();
    }

    public void TryServerConnection()
    {
        if (wsClient?.InitWebsocketClient() != true) return;
        
        ToggleServerLogButton(true);
        ToggleSettings();
    }

    public void ToggleServerLogButton(bool state)
    {
        serverLogStartButton.SetActive(state);
    }
    
    public void StartServerLogCountdown()
    {
        if(isPoseSaving) return;
        
        ToggleServerLogButton(false);
        serverLogTimer = serverLogTimerStart;
        startServerLogCountdown = true;
        Timer.GameObject().transform.parent.GameObject().SetActive(true);
    }
    
    private void UpdateServerCountdown()
    {
        serverLogTimer -= Time.deltaTime;
        Timer.text = serverLogTimer.ToString("0.00");
        if (serverLogTimer > 0) return;
        
        startServerLogCountdown = false;
        wsClient.StartServerLog();
    }

    public void UpdatePoseSimilarityScore(float score)
    {
        poseSimilarityText.text = score.ToString("0.00");
    }
}

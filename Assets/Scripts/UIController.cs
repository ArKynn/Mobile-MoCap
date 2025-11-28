using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class UIController : MonoBehaviour
{

    [SerializeField] private GameObject settingsMenu;
    [SerializeField] private TMP_Text poseSaveTimerGraphic;
    [SerializeField] private float poseSaveTimerStart;
    [SerializeField] private GameObject poseSimilarityObject;
    [SerializeField] private TMP_Text poseSimilarityText;
    
    private PoseSaver poseSaver;
    private float poseSaveTimer;
    private bool isPoseSaving;

    private void Start()
    {
        poseSaver = FindFirstObjectByType<PoseSaver>();
    }

    private void Update()
    {
        if(!isPoseSaving) return;
        UpdatePoseCountdown();
    }
    
    public void ToggleSettings()
    {
        settingsMenu?.SetActive(!settingsMenu.activeSelf);
    }

    public void StartPoseSave()
    {
        isPoseSaving = true;
        poseSaveTimer = poseSaveTimerStart;
        ToggleSettings();
        poseSaveTimerGraphic.GameObject().transform.parent.GameObject().SetActive(true);
        poseSimilarityObject.SetActive(false);
    }

    private void UpdatePoseCountdown()
    {
        poseSaveTimer -= Time.deltaTime;
        poseSaveTimerGraphic.text = poseSaveTimer.ToString("0.00");
        if (poseSaveTimer > 0) return;
        
        isPoseSaving = false;
        poseSaveTimerGraphic.GameObject().transform.parent.GameObject().SetActive(false);
        poseSimilarityObject.SetActive(true);
        poseSaver.SaveCurrentPose();
    }

    public void UpdatePoseSimilarityScore(float score)
    {
        poseSimilarityText.text = score.ToString("0.00");
    }
}

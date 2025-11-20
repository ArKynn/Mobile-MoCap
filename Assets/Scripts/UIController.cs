using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class UIController : MonoBehaviour
{

    [SerializeField] private GameObject settingsMenu;
    [SerializeField] private TMP_Text smoothingCounter;
    [SerializeField] private TMP_Text poseSaveTimerGraphic;
    [SerializeField] private float poseSaveTimerStart;
    
    private PointLandmarkVisualizer visualizer;
    private float poseSaveTimer;
    private bool isPoseSaving;

    private void Start()
    {
        visualizer = FindFirstObjectByType<PointLandmarkVisualizer>();
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

    public void ModifySmoothingButton(int modifier)
    {
        visualizer?.ModifySmoothingPoints(modifier);
        smoothingCounter.text = visualizer?.VisualizerSmoothingPoints.ToString();
    }

    public void StartPoseSave()
    {
        isPoseSaving = true;
        poseSaveTimer = poseSaveTimerStart;
        ToggleSettings();
        poseSaveTimerGraphic.GameObject().transform.parent.GameObject().SetActive(true);
    }

    private void UpdatePoseCountdown()
    {
        poseSaveTimer -= Time.deltaTime;
        poseSaveTimerGraphic.text = poseSaveTimer.ToString("0.00");
        if (poseSaveTimer > 0) return;
        
        visualizer.SaveCurrentPose();
        poseSaveTimerGraphic.GameObject().transform.parent.GameObject().SetActive(false);
        isPoseSaving = false;
    }
}

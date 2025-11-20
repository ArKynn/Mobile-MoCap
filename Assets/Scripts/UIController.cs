using TMPro;
using UnityEngine;

public class UIController : MonoBehaviour
{

    [SerializeField] private GameObject settingsMenu;
    [SerializeField] private TMP_Text smoothingCounter;
    
    private PointLandmarkVisualizer visualizer;

    private void Start()
    {
        visualizer = FindFirstObjectByType<PointLandmarkVisualizer>();
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
}

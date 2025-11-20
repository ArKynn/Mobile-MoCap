using System.Collections.Generic;
using UnityEngine;
using Mediapipe.BlazePose;
using UnityEngine.UI;
using static BodyLandmarks;

public class PointLandmarkVisualizer : MonoBehaviour
{
    [SerializeField] private int visualizerSmoothingPoints = 1;
    [SerializeField] private int maxVisualizerSmoothingPoints = 10;
    [SerializeField] private GameObject landmarkPrefab;
    [SerializeField] private GameObject lineRendererPrefab;
    [SerializeField] private RawImage _image;

    WebCamInput webCamInput;
    BlazePoseDetecter detecter;
    GameObject[] landmarkObjects;
    Landmark[] landmarks;

    public int VisualizerSmoothingPoints
    {
        get => visualizerSmoothingPoints;
        private set
        {
            visualizerSmoothingPoints = value;
            UpdateSmoothing();
        }
    }
    
    private readonly bool useWorldCoords = true;

    public BlazePoseDetecter Detecter => detecter;
    void Start()
    {
        webCamInput = FindFirstObjectByType<WebCamInput>();
        detecter = new BlazePoseDetecter();
        InitializePoints();
    }

    void LateUpdate(){
        // Predict pose by neural network model.
        detecter.ProcessImage(_image, webCamInput.inputRT, -webCamInput.WebCamTexture.videoRotationAngle);

        UpdatePoints(useWorldCoords);
    }

    void InitializePoints()
    {
        landmarkObjects = new GameObject[Landmarks.Count];
        landmarks = new Landmark[Landmarks.Count];
        for (int i = 0; i < Landmarks.Count; i++)
        {
            landmarkObjects[i] = Instantiate(landmarkPrefab,transform);
            landmarkObjects[i].name = Landmarks[i];
            landmarks[i] = landmarkObjects[i].GetComponent<Landmark>();
            landmarks[i].SetLineRendererPrefab(lineRendererPrefab);
            landmarks[i].InitializeSmoothing(visualizerSmoothingPoints);
        }

        for (int i = 0; i < LandmarkPairs.Count; i++)
        {
            landmarks[(int) LandmarkPairs[i].X].SetNext(landmarks[(int) LandmarkPairs[i].Y]);
        }
    }

    void UpdatePoints(bool useWorldPosition)
    {
        for (int i = 0; i < landmarkObjects.Length; i++)
        {
            landmarks[i].UpdateValues(useWorldPosition ? detecter.GetPoseWorldLandmark(i) : detecter.GetPoseLandmark(i));
        }
    }

    public IEnumerable<float[]> GetLandmarkPointData()
    {
        for (int i = 0; i < landmarkObjects.Length; i++)
        {
            var temp = useWorldCoords ? detecter.GetPoseWorldLandmark(i) : detecter.GetPoseLandmark(i);
            yield return new []{temp[0], temp[1], temp[2], temp[3]};
        }
    }

    public void ModifySmoothingPoints(int modifier)
    {
        if(VisualizerSmoothingPoints + modifier < 0 || VisualizerSmoothingPoints + modifier > maxVisualizerSmoothingPoints) return;
        VisualizerSmoothingPoints += modifier;
    }

    private void UpdateSmoothing()
    {
        for (int i = 0; i < Landmarks.Count; i++)
        {
            landmarks[i].InitializeSmoothing(visualizerSmoothingPoints);
        }
    }

    void OnApplicationQuit()
    {
        // Must call Dispose method when no longer in use.
        detecter?.Dispose();
    }
}

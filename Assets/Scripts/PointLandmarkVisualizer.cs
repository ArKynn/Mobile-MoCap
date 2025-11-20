using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
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

    private PoseSimilarityComparer poseSimilarityComparer;
    WebCamInput webCamInput;
    BlazePoseDetecter detecter;
    GameObject[] landmarkObjects;
    Landmark[] landmarks;
    private GameObject trackedPose;
    private GameObject savedPose;

    public GameObject TrackedPose => trackedPose;
    
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
        poseSimilarityComparer = GetComponent<PoseSimilarityComparer>();
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
        trackedPose = new GameObject
        {
            transform =
            {
                parent = transform,
                localPosition = Vector3.zero
            },
            name = "TrackedPose"
        };
        
        landmarkObjects = new GameObject[Landmarks.Count];
        landmarks = new Landmark[Landmarks.Count];
        
        for (int i = 0; i < Landmarks.Count; i++)
        {
            landmarkObjects[i] = Instantiate(landmarkPrefab, trackedPose.transform);
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

    public void SaveCurrentPose()
    {
        if(savedPose != null) Destroy(savedPose);
        savedPose = Instantiate(trackedPose, transform);
        {
            name = "SavedPose";
        }
        poseSimilarityComparer.StartComparer(savedPose);
        StartCoroutine(UpdateSavedPointsAlpha());
    }

    private IEnumerator UpdateSavedPointsAlpha()
    {
        yield return null;
        foreach (var point in savedPose.GetComponentsInChildren<Landmark>())
            point.IsCopy();
    }

    void OnApplicationQuit()
    {
        // Must call Dispose method when no longer in use.
        detecter?.Dispose();
    }
}

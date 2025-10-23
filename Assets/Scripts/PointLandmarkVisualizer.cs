using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Mediapipe.BlazePose;
using static BodyLandmarks;

public class PointLandmarkVisualizer : MonoBehaviour
{
    [SerializeField] Camera mainCamera;
    [SerializeField] WebCamInput webCamInput;
    [SerializeField, Range(0, 1)] float humanExistThreshold = 0.5f;
    [SerializeField] private bool useWorldCoords;
    [SerializeField] private GameObject landmarkPrefab;
    [SerializeField] private GameObject lineRendererPrefab;

    BlazePoseDetecter detecter;
    GameObject[] landmarkObjects;
    Landmark[] landmarks;
    
    void Start(){
        detecter = new BlazePoseDetecter();
        InitializePoints();
    }

    void Update(){
        // Predict pose by neural network model.
        detecter.ProcessImage(webCamInput.inputRT);

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

    void OnApplicationQuit()
    {
        // Must call Dispose method when no longer in use.
        detecter?.Dispose();
    }
}

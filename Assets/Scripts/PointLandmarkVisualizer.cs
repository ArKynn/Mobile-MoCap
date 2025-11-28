using System.Collections.Generic;
using UnityEngine;
using Mediapipe.BlazePose;
using UnityEngine.UI;
using static BodyLandmarks;

public class PointLandmarkVisualizer : MonoBehaviour
{
    [SerializeField] private int visualizerSmoothingPoints = 1;
    [SerializeField] private GameObject landmarkPrefab;
    [SerializeField] private GameObject lineRendererPrefab;
    [SerializeField] private RawImage _image;

    WebCamInput webCamInput;
    BlazePoseDetecter detecter;
    private Pose trackedPose;

    public Pose TrackedPose => trackedPose;
    
    private readonly bool useWorldCoords = true;

    public BlazePoseDetecter Detecter => detecter;
    public int VisualizerSmoothingPoints => visualizerSmoothingPoints;
    void Start()
    {
        webCamInput = FindFirstObjectByType<WebCamInput>();
        detecter = new BlazePoseDetecter();
        InitializePose(out trackedPose, "Tracked Pose");
    }

    void LateUpdate(){
        // Predict pose by neural network model.
        detecter.ProcessImage(_image, webCamInput.inputRT, -webCamInput.WebCamTexture.videoRotationAngle);
        trackedPose.UpdatePoints();
    }

    public void InitializePose(out Pose poseToInitialize, string gameObjectName, PoseLandmark[] toSave = null)
    {
        var trackedPoseObject = new GameObject
        {
            transform =
            {
                localPosition = Vector3.zero
            },
            name = gameObjectName
        };
        poseToInitialize = trackedPoseObject.AddComponent<Pose>();
        poseToInitialize.transform.SetParent(transform, false);
        poseToInitialize.Init(landmarkPrefab, lineRendererPrefab, visualizerSmoothingPoints, detecter: detecter, poseParts: toSave);
    }
    

    public IEnumerable<float[]> GetLandmarkPointData(Pose pose = null)
    {
        pose ??= trackedPose;
        for (int i = 0; i < pose.Landmarks.Length; i++)
        {
            var temp = useWorldCoords ? detecter.GetPoseWorldLandmark(i) : detecter.GetPoseLandmark(i);
            yield return new []{temp[0], temp[1], temp[2], temp[3]};
        }
    }

    void OnApplicationQuit()
    {
        // Must call Dispose method when no longer in use.
        detecter?.Dispose();
    }
}

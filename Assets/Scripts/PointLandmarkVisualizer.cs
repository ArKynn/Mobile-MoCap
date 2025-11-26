using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mediapipe.BlazePose;
using UnityEngine.UI;
using static BodyLandmarks;
using Enum = System.Enum;

public class PointLandmarkVisualizer : MonoBehaviour
{
    [SerializeField] private int visualizerSmoothingPoints = 1;
    [SerializeField] private GameObject landmarkPrefab;
    [SerializeField] private GameObject lineRendererPrefab;
    [SerializeField] private RawImage _image;

    private PoseSimilarityComparer poseSimilarityComparer;
    WebCamInput webCamInput;
    BlazePoseDetecter detecter;
    private Pose trackedPose;
    private Pose savedPose;

    public Pose TrackedPose => trackedPose;
    
    private readonly bool useWorldCoords = true;

    public BlazePoseDetecter Detecter => detecter;
    void Start()
    {
        poseSimilarityComparer = GetComponent<PoseSimilarityComparer>();
        webCamInput = FindFirstObjectByType<WebCamInput>();
        detecter = new BlazePoseDetecter();
        InitializePose(out trackedPose, "Tracked Pose");
    }

    void LateUpdate(){
        // Predict pose by neural network model.
        detecter.ProcessImage(_image, webCamInput.inputRT, -webCamInput.WebCamTexture.videoRotationAngle);
        trackedPose.UpdatePoints();
    }

    void InitializePose(out Pose poseToInitialize, string gameObjectName)
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
        poseToInitialize.Init(landmarkPrefab, lineRendererPrefab, visualizerSmoothingPoints, detecter: detecter);
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

    public void SaveCurrentPose()
    {
        if(savedPose != null) Destroy(savedPose);
        InitializePose(out savedPose, "Saved Pose");
        savedPose.UpdatePoints();
        //poseSimilarityComparer.StartComparer(savedPose);
    }

    void OnApplicationQuit()
    {
        // Must call Dispose method when no longer in use.
        detecter?.Dispose();
    }
}

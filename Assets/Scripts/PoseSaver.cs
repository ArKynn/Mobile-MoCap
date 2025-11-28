using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static BodyLandmarks;

public class PoseSaver : MonoBehaviour
{
    [SerializeField] private bool editorWritePoseToFile;
    private PointLandmarkVisualizer pointVisualizer;
    private PoseSimilarityComparer poseSimilarityComparer;
    
    public List<PoseLandmark> landmarksToSave = new ();
    public Pose savedPose {get; private set;}


    private void Start()
    {
        pointVisualizer = GetComponent<PointLandmarkVisualizer>();
        poseSimilarityComparer = GetComponent<PoseSimilarityComparer>();
    }

    public void SaveCurrentPose()
    {
        if(savedPose != null) Destroy(savedPose);
#if UNITY_EDITOR
        var landmarks = SortPoseLandmarks(landmarksToSave.ToArray());
        pointVisualizer.InitializePose(out var temp, "Saved Pose", landmarksToSave.ToArray());
        if(editorWritePoseToFile) SavePoseToFile(temp);
#else
        pointVisualizer.InitializePose(out var temp, "Saved Pose");
#endif
        savedPose = temp;
        savedPose.UpdatePoints();
        poseSimilarityComparer.StartComparer(savedPose);
    }

    private void SavePoseToFile(Pose poseToSave)
    {
        var saveObj = ScriptableObject.CreateInstance<SavedPose>();
        AssetDatabase.CreateAsset(saveObj, $"Assets/SavedPoses/{DateTime.Now:dd_MM_yyyy___HH_mm_ss}.asset");
        saveObj.SavePose(poseToSave);
        EditorUtility.SetDirty(saveObj);
        AssetDatabase.SaveAssets();
    }
}

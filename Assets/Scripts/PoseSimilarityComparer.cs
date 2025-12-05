using System.Collections.Generic;
using UnityEngine;
using static BodyLandmarks;

public class PoseSimilarityComparer : MonoBehaviour
{
    private PointLandmarkVisualizer visualizer;
    private UIController uiController;
    private Pose savedPose;
    private Pose detectedPose;
    private float poseSimilarityScore;
    
    private Vector3[] detectedPoseLineDirections;
    private Vector3[] savedPoseLineDirections;
    private float[] individualPoseSilimiarityScores;
    
    public void StartComparer(Pose savedPose)
    {
        this.savedPose = savedPose;
        detectedPose = visualizer.TrackedPose;
        GetPoseLineDirections(this.savedPose, out savedPoseLineDirections, savedPose.PoseLandmarks);
    }

    private void Start()
    {
        visualizer = GetComponent<PointLandmarkVisualizer>();
        uiController = FindFirstObjectByType<UIController>();
    }

    private void Update()
    {
        if (savedPose != null && detectedPose != null)
        {
            GetPoseLineDirections(detectedPose, out detectedPoseLineDirections, savedPose.PoseLandmarks);
            CalculatePoseSimilarity();
            DisplayIndividualSimilarityScores();
            uiController.UpdatePoseSimilarityScore(poseSimilarityScore);
        }
    }

    private void GetPoseLineDirections(Pose pose, out Vector3[] lineDirections, PoseLandmark[] constrictingLandmarks)
    {
        var lineDirectionsList = new List<Vector3>();
        
        for (int i = 0; i < pose.Landmarks.Length; i++)
        {
            for (int j = 0; j < constrictingLandmarks.Length; j++)
            {
                if (pose.Landmarks[i].poseLandmark != constrictingLandmarks[j]) continue;
                
                var pairs = pose.Landmarks[i].GetNext();
                for (int k = 0; k < pairs.Length; k++)
                {
                    foreach (var landmark in constrictingLandmarks)
                    {
                        if (pairs[k].poseLandmark != landmark) continue;
                            
                        var point1 = pose.Landmarks[i].transform.position;;
                        var point2 = pairs[k].transform.position;;
                        lineDirectionsList.Add(new Vector3(point2.x - point1.x, point2.y - point1.y, point2.z - point1.z));
                    }
                }
            }
        }
        
        lineDirections = lineDirectionsList.ToArray();
    }

    private void DisplayIndividualSimilarityScores()
    {
        for (int i = 0; i < savedPose.Landmarks.Length; i++)
        {
            for (int j = 0; j < savedPose.Landmarks[i].GetNext().Length; j++)
            {
                savedPose.Landmarks[i].UpdateSingleLineRenderer(j, individualPoseSilimiarityScores[i]);
            }
        }
    }

    private void CalculatePoseSimilarity()
    {
        individualPoseSilimiarityScores = new float[savedPoseLineDirections.Length];
        poseSimilarityScore = 0;
        for (int i = 0; i < savedPoseLineDirections.Length; i++)
        {
            poseSimilarityScore += individualPoseSilimiarityScores[i] = CosineSimilarity(savedPoseLineDirections[i], detectedPoseLineDirections[i]);
        }
        
        poseSimilarityScore /= savedPoseLineDirections.Length;
    }

    private static float CosineSimilarity(Vector3 v1, Vector3 v2)
    {
        float dot = Vector3.Dot(v1, v2);
        float magA = v1.magnitude;
        float magB = v2.magnitude;

        if (magA == 0f || magB == 0f)
            return 0f;

        return dot / (magA * magB);
    }
}

using UnityEngine;

public class PoseSimilarityComparer : MonoBehaviour
{
    private PointLandmarkVisualizer visualizer;
    private UIController uiController;
    private GameObject savedPose;
    private GameObject detectedPose;
    private float poseSimilarityScore;

    private Vector3[] savedPoseLineDirections;
    private Vector3[] detectedPoseLineDirections;
    
    public void StartComparer(GameObject savedPose)
    {
        this.savedPose = savedPose;
        detectedPose = visualizer.TrackedPose;
        GetPoseLineDirections(savedPose, savedPoseLineDirections);
    }

    private void Start()
    {
        visualizer = GetComponent<PointLandmarkVisualizer>();
        uiController = FindFirstObjectByType<UIController>();
        savedPoseLineDirections = new Vector3[BodyLandmarks.LandmarkPairs.Count];
        detectedPoseLineDirections = new Vector3[BodyLandmarks.LandmarkPairs.Count];
    }

    private void Update()
    {
        if (savedPose != null && detectedPose != null)
        {
            GetPoseLineDirections(detectedPose, detectedPoseLineDirections);
            CalculatePoseSimilarity();
            uiController.UpdatePoseSimilarityScore(poseSimilarityScore);
        }
    }

    private void GetPoseLineDirections(GameObject pose, Vector3[] lineDirections)
    {
        var points = pose.transform.GetComponentsInChildren<Transform>();
        
        for (int i = 0; i < BodyLandmarks.LandmarkPairs.Count; i++)
        {
            var point1 = points[(int)BodyLandmarks.LandmarkPairs[i].X];
            var point2 = points[(int)BodyLandmarks.LandmarkPairs[i].Y];
            lineDirections[i] = new Vector3(point2.position.x - point1.position.x, point2.position.y - point1.position.y, point2.position.z - point1.position.z);
        }
    }

    private void CalculatePoseSimilarity()
    {
        for (int i = 0; i < savedPoseLineDirections.Length; i++)
        {
            poseSimilarityScore += CosineSimilarity(savedPoseLineDirections[i] ,detectedPoseLineDirections[i]);
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

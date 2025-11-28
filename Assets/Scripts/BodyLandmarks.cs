using System.Collections.Generic;
using System.Linq;
using static BodyLandmarks.PoseLandmark;

public static class BodyLandmarks
{
    // All body landmarks detected by Neural Network model
    // Defined by the figure in https://ai.google.dev/edge/mediapipe/solutions/vision/pose_landmarker.
    
    public enum PoseLandmark
    {
        Nose,
        LeftEyeInner,
        LeftEyeCenter,
        LeftEyeOuter,
        RightEyeInner,
        RightEyeCenter,
        RightEyeOuter,
        LeftEar,
        RightEar,
        MouthLeft,
        MouthRight,
        LeftShoulder,
        RightShoulder,
        LeftElbow,
        RightElbow,
        LeftWrist,
        RightWrist,
        LeftPinky,
        RightPinky,
        LeftIndex,
        RightIndex,
        LeftThumb,
        RightThumb,
        LeftHip,
        RightHip,
        LeftKnee,
        RightKnee,
        LeftAnkle,
        RightAnkle,
        LeftHeel,
        RightHeel,
        LeftFootIndex,
        RightFootIndex
    }
    
    // Landmark connections
    // Defined by the figure in https://ai.google.dev/edge/mediapipe/solutions/vision/pose_landmarker.
    public static readonly Dictionary<PoseLandmark, PoseLandmark[]> PoseLandmarkPairs =
        new()
        {
            { Nose, new[] { LeftEyeInner, RightEyeInner } },
            { LeftEyeInner, new[] { LeftEyeCenter } },
            { LeftEyeCenter, new[] { LeftEyeOuter } },
            { LeftEyeOuter, new[] { LeftEar } },
            { RightEyeInner, new[] { RightEyeCenter } },
            { RightEyeCenter, new[] { RightEyeOuter } },
            { RightEyeOuter, new[] { RightEar } },
            { LeftEar, null },
            { RightEar, null },
            { MouthLeft, new[] { MouthRight } },
            { MouthRight, null },
            { LeftShoulder, new[] { RightShoulder, LeftElbow, LeftHip } },
            { RightShoulder, new[] { RightElbow, RightHip } },
            { LeftElbow, new[] { LeftWrist } },
            { RightElbow, new[] { RightWrist } },
            { LeftWrist, new[] { LeftPinky, LeftThumb } },
            { RightWrist, new[] { RightPinky, RightThumb } },
            { LeftPinky, new[] { LeftIndex } },
            { RightPinky, new[] { RightIndex } },
            { LeftIndex, new[] { LeftWrist } },
            { RightIndex, new[] { RightWrist } },
            { LeftThumb, null },
            { RightThumb, null },
            { LeftHip, new[] { RightHip, LeftKnee } },
            { RightHip, new[] { RightKnee } },
            { LeftKnee, new[] { LeftAnkle } },
            { RightKnee, new[] { RightAnkle } },
            { LeftAnkle, new[] { LeftHeel } },
            { RightAnkle, new[] { RightHeel } },
            { LeftHeel, new[] { LeftFootIndex } },
            { RightHeel, new[] { RightFootIndex } },
            { LeftFootIndex, new[] { LeftAnkle } },
            { RightFootIndex, new[] { RightAnkle } }
        };
    
    public static PoseLandmark[] SortPoseLandmarks(PoseLandmark[] poseLandmarks)
    {
        var landmarksList = poseLandmarks.ToList();
        return landmarksList.OrderBy(o => (int)o).ToArray();
    }
}
using System.Collections.Generic;
using System.Numerics;

public static class BodyLandmarks
{
    // All body landmarks detected by Neural Network model
    // Defined by the figure in https://ai.google.dev/edge/mediapipe/solutions/vision/pose_landmarker.
    public static readonly List<string> Landmarks = new (){
        "0 - nose",
        "1 - left eye (inner)",
        "2 - left eye (center)",
        "3 - left eye (outer)",
        "4 - right eye (inner)",
        "5 - right eye(center)",
        "6 - right eye (outer)",
        "7 - left ear",
        "8 - right ear",
        "9 - mouth (left)",
        "10 - mouth (right)",
        "11 - left shoulder",
        "12 - right shoulder",
        "13 - left elbow",
        "14 - right elbow",
        "15 - left wrist",
        "16 - right wrist",
        "17 - left pinky",
        "18 - right pinky",
        "19 - left index",
        "20 - right index",
        "21 - left thumb",
        "22 - right thumb",
        "23 - left hip",
        "24 - right hip",
        "25 - left knee",
        "26 - right knee",
        "27 - left ankle",
        "28 - right ankle",
        "29 - left heel",
        "30 - right heel",
        "31 - left foot index",
        "32 - right foot index"
    };
    
    // Landmark connections
    // Defined by the figure in https://ai.google.dev/edge/mediapipe/solutions/vision/pose_landmarker.

    public static readonly List<Vector2> LandmarkPairs = new List<Vector2>{
        new Vector2(0, 1), new Vector2(1, 2), new Vector2(2, 3), new Vector2(3, 7), new Vector2(0, 4), 
        new Vector2(4, 5), new Vector2(5, 6), new Vector2(6, 8), new Vector2(9, 10), new Vector2(11, 12), 
        new Vector2(11, 13), new Vector2(13, 15), new Vector2(15, 17), new Vector2(17, 19), new Vector2(19, 15), 
        new Vector2(15, 21), new Vector2(12, 14), new Vector2(14, 16), new Vector2(16, 18), new Vector2(18, 20), 
        new Vector2(20, 16), new Vector2(16, 22), new Vector2(11, 23), new Vector2(12, 24), new Vector2(23, 24), 
        new Vector2(23, 25), new Vector2(25, 27), new Vector2(27, 29), new Vector2(29, 31), new Vector2(31, 27), 
        new Vector2(24, 26), new Vector2(26, 28), new Vector2(28, 30), new Vector2(30, 32), new Vector2(32, 28)
    };
}
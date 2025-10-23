﻿using System.Collections.Generic;

public static class BodyLandmarks
{
    // All body landmarks detected by Neural Network model
    // Defined by the figure in https://ai.google.dev/edge/mediapipe/solutions/vision/pose_landmarker.
    public static List<string> Landmarks = new (){
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
}
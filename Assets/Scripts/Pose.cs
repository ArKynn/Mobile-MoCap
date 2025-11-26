using Mediapipe.BlazePose;
using UnityEngine;
using static BodyLandmarks;
using Enum = System.Enum;

public class Pose : MonoBehaviour
    {
        public Landmark[] Landmarks {get; private set;}
        private BlazePoseDetecter _detecter;

        public void Init(GameObject landmarkPrefab, GameObject lineRendererPrefab, int visualizerSmoothingPointCount = 1, PoseLandmarks[] poseParts = null, BlazePoseDetecter detecter = null)
        {
            if(detecter != null) _detecter = detecter;
            poseParts ??= Enum.GetValues(typeof(PoseLandmarks)) as PoseLandmarks[];
            var pointCount = poseParts.Length;
            
            Landmarks = new Landmark[pointCount];
        
            //Instantiates a new LandmarkObject for each landmark in this pose
            for (int i = 0; i < Landmarks.Length; i++)
            {
                Landmarks[i] = Instantiate(landmarkPrefab, transform).GetComponent<Landmark>();
                Landmarks[i].name = $"{i} - {poseParts[i]}";
                Landmarks[i].SetLineRendererPrefab(lineRendererPrefab);
                Landmarks[i].poseLandmark = poseParts[i];
                if(visualizerSmoothingPointCount > 1) Landmarks[i].InitializeSmoothing(visualizerSmoothingPointCount);
            }

            //Connects Landmark objects with their defined pair, if any is available in this pose
            for (int i = 0; i < Landmarks.Length; i++)
            {
                if (!PoseLandmarkPairs.TryGetValue(Landmarks[i].poseLandmark, out var pairList) || pairList == null) continue; 
                
                foreach (var landmarkPair in PoseLandmarkPairs[Landmarks[i].poseLandmark])
                {
                    for (int j = 0; j < Landmarks.Length; j++)
                    {
                        if (Landmarks[j].poseLandmark == landmarkPair)
                        {
                            Landmarks[i].SetNext(Landmarks[j]); 
                            break;
                        }
                        
                    }
                }
            }
        }
        
        public void UpdatePoints()
        {
            for (int i = 0; i < Landmarks.Length; i++)
            {
                Landmarks[i].UpdateValues(_detecter.GetPoseWorldLandmark((int)Landmarks[i].poseLandmark));
            }
            
            for (int i = 0; i < Landmarks.Length; i++)
            {
                Landmarks[i].UpdateLineRenderers();
            }
        }
    }

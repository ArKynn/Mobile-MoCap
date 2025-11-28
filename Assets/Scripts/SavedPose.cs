using UnityEngine;

[CreateAssetMenu(fileName = "SavedPose", menuName = "ScriptableObjects/SavedPose", order = 1)]
public class SavedPose : ScriptableObject
{
    public Pose savedPose { get; private set; }

    public void SavePose(Pose newPose)
    {
        savedPose = newPose;
    }
}

using UnityEngine;

public class Landmark : MonoBehaviour
{
    [SerializeField] private Color32 inViewColor = Color.green;
    [SerializeField] private Color32 blockedViewColor = Color.red;

    private GameObject nextPoint;
    public Material material { get; private set; }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        material = GetComponent<MeshRenderer>().material;
    }

    public void UpdateValues(Vector4 landmarkInfo)
    {
        Vector3 bufferVector = new Vector3(landmarkInfo.x, landmarkInfo.y, landmarkInfo.z);
        if(!float.IsNaN(bufferVector.x) && !float.IsNaN(bufferVector.y) && !float.IsNaN(bufferVector.z)) transform.position = bufferVector;
        material.color = Color32.Lerp(blockedViewColor, inViewColor, landmarkInfo.w);
    }

    public void GetNext(GameObject next)
    {
        if(nextPoint == null) nextPoint = next;
    }
}

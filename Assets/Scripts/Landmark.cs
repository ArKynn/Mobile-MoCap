using System.Collections.Generic;
using UnityEngine;

public class Landmark : MonoBehaviour
{
    [SerializeField] private Color32 inViewColor = Color.green;
    [SerializeField] private Color32 blockedViewColor = Color.red;
    [SerializeField] private Color32 inViewLineColor = Color.green;
    [SerializeField] private Color32 blockedViewLineColor = Color.red;

    private List<Landmark> nextPoints;
    private List<LineRenderer> lineRenderers;
    private GameObject lineRendererPrefab;
    public Material material { get; private set; }
    public float visibilityScore {get; private set;}

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        material = GetComponent<MeshRenderer>().material;
        nextPoints = new List<Landmark>();
        lineRenderers = new List<LineRenderer>();
    }

    void LateUpdate()
    {
        for (int i = 0; i < nextPoints.Count; i++)
        {
            lineRenderers[i].SetPosition(0, transform.position);
            lineRenderers[i].SetPosition(1, nextPoints[i] ? nextPoints[i].transform.position : transform.position);
            lineRenderers[i].startColor = Color32.Lerp(blockedViewLineColor, inViewLineColor, visibilityScore);
            lineRenderers[i].endColor = Color32.Lerp(blockedViewLineColor, inViewLineColor, nextPoints[i].visibilityScore);
        }
    }

    public void UpdateValues(Vector4 landmarkInfo)
    {
        visibilityScore = landmarkInfo.w;
        Vector3 bufferVector = new Vector3(landmarkInfo.x, landmarkInfo.y, landmarkInfo.z);
        if(!float.IsNaN(bufferVector.x) && !float.IsNaN(bufferVector.y) && !float.IsNaN(bufferVector.z)) transform.localPosition = bufferVector;
        
        material.color = Color32.Lerp(blockedViewColor, inViewColor, visibilityScore);
    }

    public void SetNext(Landmark next)
    {
        nextPoints.Add(next);
        lineRenderers.Add(Instantiate(lineRendererPrefab, transform).GetComponent<LineRenderer>());
    }

    public void SetLineRendererPrefab(GameObject prefab)
    {
        lineRendererPrefab = prefab;
    }
}

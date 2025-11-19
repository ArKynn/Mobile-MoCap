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
    private List<Vector4> smoothingPoints;
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
        if (smoothingPoints != null)
        {
            AddSmoothingPoint(landmarkInfo);
            UpdatePointSmooth();
        }
        else UpdatePoint(landmarkInfo);
        
        material.color = Color32.Lerp(blockedViewColor, inViewColor, visibilityScore);
    }

    private void AddSmoothingPoint(Vector4 point)
    {
        if(smoothingPoints.Count >= smoothingPoints.Capacity) smoothingPoints.RemoveAt(0);
        
        smoothingPoints.Add(point);
    }

    private void UpdatePointSmooth()
    {
        var visibility = 0f;
        var tempVector = Vector3.zero;
        foreach (var point in smoothingPoints)
        {
            tempVector.x += point.x;
            tempVector.y += point.y;
            tempVector.z += point.z;
            visibility += point.w;
        }
        visibilityScore = visibility / smoothingPoints.Count;
        tempVector.x /= smoothingPoints.Count;
        tempVector.y /= smoothingPoints.Count;
        tempVector.z /= smoothingPoints.Count;
        
        UpdatePositionVectorNanCheck(tempVector);
    }

    private void UpdatePoint(Vector4 landmarkInfo)
    {
        visibilityScore = landmarkInfo.w;
        UpdatePositionVectorNanCheck(landmarkInfo);
    }

    private void UpdatePositionVectorNanCheck(Vector3 position)
    {
        if(!float.IsNaN(position.x) && !float.IsNaN(position.y) && !float.IsNaN(position.z)) transform.localPosition = position;
    }

    public void InitializeSmoothing(int smoothingPointCount)
    {
        smoothingPoints ??= new List<Vector4>(smoothingPointCount);
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

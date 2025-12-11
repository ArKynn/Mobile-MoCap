using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using static BodyLandmarks;

public class Landmark : MonoBehaviour
{
    [SerializeField] private Color32 inViewColor = Color.green;
    [SerializeField] private Color32 blockedViewColor = Color.red;
    [SerializeField] private Color32 inViewLineColor = Color.green;
    [SerializeField] private Color32 blockedViewLineColor = Color.red;

    private List<Landmark> pairPoints;
    private List<LineRenderer> lineRenderers;
    private List<Vector4> smoothingPoints;
    private GameObject lineRendererPrefab;
    public Material material { get; private set; }
    public float visibilityScore {get; private set;}
    public PoseLandmark poseLandmark;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        material = GetComponent<MeshRenderer>().material;
        pairPoints = new List<Landmark>();
        lineRenderers = new List<LineRenderer>();
    }

    public void UpdateValues(Vector4 landmarkInfo)
    {
        if (smoothingPoints?.Capacity > 1)
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

    public void UpdateLineRenderers()
    {
        for (int i = 0; i < pairPoints.Count; i++)
        {
            lineRenderers[i].SetPosition(0, transform.position);
            lineRenderers[i].SetPosition(1, pairPoints[i] ? pairPoints[i].transform.position : transform.position);
            lineRenderers[i].startColor = Color32.Lerp(blockedViewLineColor, inViewLineColor, visibilityScore);
            lineRenderers[i].endColor = Color32.Lerp(blockedViewLineColor, inViewLineColor, pairPoints[i].visibilityScore);
        }
    }

    public void UpdateSingleLineRenderer(int index, float score)
    {
        score = Mathf.Pow((score + 1) / 2, 3); 
        lineRenderers[index].startColor = Color32.Lerp(Color.red, Color.green, score);
        lineRenderers[index].endColor = Color32.Lerp(Color.red, Color.green, score);
    }

    private void UpdatePositionVectorNanCheck(Vector3 position)
    {
        if(!float.IsNaN(position.x) && !float.IsNaN(position.y) && !float.IsNaN(position.z)) transform.localPosition = position;
    }

    public void InitializeSmoothing(int smoothingPointCount)
    {
        smoothingPoints = new List<Vector4>(smoothingPointCount);
    }

    public void SetNext(Landmark next)
    {
        pairPoints.Add(next);
        lineRenderers.Add(Instantiate(lineRendererPrefab, transform).GetComponent<LineRenderer>());
    }

    public Landmark[] GetPairs()
    {
        return pairPoints.ToArray();
    }

    public void SetLineRendererPrefab(GameObject prefab)
    {
        lineRendererPrefab = prefab;
    }
}

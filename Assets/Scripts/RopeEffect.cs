using System.Collections.Generic;
using UnityEngine;

public class RopeEffect : MonoBehaviour
{
    [SerializeField] private LineRenderer lineRenderer;
    private List<Vector3> controlPoints = new List<Vector3>(); 
    private int resolution = 10; 
    private float wobbleAmplitude = 0.1f; 
    private float wobbleFrequency = 2f; 

    private void Update()
    {

        List<Vector3> ropePoints = CreateRope(controlPoints);
        ApplyWobbleEffect(ropePoints);
        UpdateLineRenderer(ropePoints);
    }

    public void SetControlPoints(List<Transform> positions)
    {
        controlPoints.Clear();
        foreach (var position in positions)
        {
            controlPoints.Add(position.position);
        }
    }

    private List<Vector3> CreateRope(List<Vector3> points)
    {
        List<Vector3> ropePoints = new List<Vector3>();

        for (int i = 0; i < points.Count - 1; i++)
        {
            Vector3 p0 = points[i];
            Vector3 p1 = points[i + 1];

            for (int j = 0; j <= resolution; j++)
            {
                float t = (float)j / resolution;
                Vector3 pointOnCurve = Vector3.Lerp(p0, p1, t);
                ropePoints.Add(pointOnCurve);
            }
        }

        return ropePoints;
    }

    private void ApplyWobbleEffect(List<Vector3> ropePoints)
    {
        for (int i = 0; i < ropePoints.Count; i++)
        {
            Vector3 offset = Vector3.up * Mathf.Sin(Time.time * wobbleFrequency + i * 0.2f) * wobbleAmplitude;
            ropePoints[i] += offset;
        }
    }

    private void UpdateLineRenderer(List<Vector3> ropePoints)
    {
        lineRenderer.positionCount = ropePoints.Count;
        lineRenderer.SetPositions(ropePoints.ToArray());
    }

    public void ClearLineRenderer()
    {
        lineRenderer.positionCount = 0;
        controlPoints.Clear();
    }
}

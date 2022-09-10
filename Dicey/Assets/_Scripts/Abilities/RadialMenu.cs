using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class RadialMenu : MonoBehaviour
{
    [Range(1, 30)]
    public int SegmentCount = 1;
    [Range(5, 1000)]
    public int PointCount = 200;
    [Range(0, 50)]
    public float Distance = 3.3f;
    [Range(0, 0.05f)]
    public float OffsetPercentage = 0f;
    private void Update()
    {
        List<Vector3> points = new List<Vector3>();
        var lines = GetComponentsInChildren<LineRenderer>();
        int countDiff = lines.Length - SegmentCount;
        if (countDiff > 0)
        {
            for (int i = 0; i < countDiff; i++)
            {
                Destroy(lines[i].gameObject);
            }
            lines = GetComponentsInChildren<LineRenderer>();
        }
        else if (countDiff < 0)
        {
            countDiff *= -1;
            for (int i = 0; i < countDiff; i++)
            {
                var newObj = new GameObject("Line");
                newObj.transform.SetParent(transform);
                newObj.AddComponent<LineRenderer>();
            }
            lines = GetComponentsInChildren<LineRenderer>();
        }
        Camera cam = Camera.main;
        Vector3 forwardDir = cam.transform.forward * Distance;
        Vector3 basePoint = cam.transform.position + forwardDir * 2;
        Vector3 dir = cam.transform.up * 3;
        if (SegmentCount == 1)
        {
            LineRenderer line = lines[0];
            line.positionCount = PointCount;
            for (int i = 0; i < PointCount; i++)
            {
                Vector3 point = basePoint + Quaternion.AngleAxis(i * (360f / (float)PointCount), forwardDir) * dir;
                // Debug.Log("Angle: " + i * (360 / PointCount));
                // Debug.Log("Point: " + point);
                // Debug.Log("Rot: " + Quaternion.AngleAxis(i * (360f / (float)PointCount), forwardDir) * dir);
                line.SetPosition(i, point);
            }
            line.loop = true;
            line.numCornerVertices = 5;
        }
        else
        {
            // rounds up PointCount for clean calculation
            int adjustedPointCount = PointCount + (SegmentCount - (PointCount % SegmentCount));
            int perSegmenPoints = adjustedPointCount / SegmentCount;
            float perSegmentAngleTotal = 360f / (float)SegmentCount;
            float angleOffset = perSegmentAngleTotal * OffsetPercentage;
            float perSegmentAngle = perSegmentAngleTotal - angleOffset * 2;
            // Debug.Log("Offset: " + angleOffset);
            for (int i = 0; i < SegmentCount; i++)
            {
                LineRenderer line = lines[i];
                line.positionCount = perSegmenPoints;
                for (int j = 0; j < perSegmenPoints; j++)
                {
                    Vector3 point;
                    if (j == 0)
                    {
                        point = basePoint + Quaternion.AngleAxis(i * perSegmentAngleTotal + j * perSegmentAngle / (float)perSegmenPoints + angleOffset, forwardDir) * dir;
                    }
                    else
                    {
                        point = basePoint + Quaternion.AngleAxis(i * perSegmentAngleTotal + j * perSegmentAngle / (float)perSegmenPoints, forwardDir) * dir;
                    }
                    line.SetPosition(j, point);
                }
                line.numCornerVertices = 5;
                line.loop = false;
            }
        }

    }


}
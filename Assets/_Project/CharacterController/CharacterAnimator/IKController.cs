using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IKController : MonoBehaviour
{
    [SerializeField] private LineRenderer line;
    [SerializeField] private Transform target;
    [SerializeField] private List<IKSegment> segments = new();
    

    private void LateUpdate()
    {
        CalculateSegmentPositions();
        ReturnToStart();
        Draw();
    }

    private void CalculateSegmentPositions()
    {
        IKSegment destination = new IKSegment(target.position);
        foreach (var segment in segments)
        {
            destination = segment.Follow(destination);
        }
    }
    private void ReturnToStart()
    {
        Vector2 offset = segments[segments.Count - 1].GetEndPosition() - (Vector2)transform.position;
        
        foreach (var segment in segments)
        {
            segment.ReturnToStart(offset);
        }
    }
    private void Draw()
    {
        List<Vector3> points = new();
        points.Add(transform.position);
        for(int i = segments.Count - 1; i >= 0; i--)
        {
            points.Add(segments[i].GetPosition());
        }

        line.positionCount = points.Count;
        line.SetPositions(points.ToArray());
    }
}

[System.Serializable]
public class IKSegment
{
    [SerializeField] private float length;
    private Vector2 end;
    private Vector2 begin;
    public IKSegment(Vector2 end) => this.end = end;

    public IKSegment Follow(IKSegment target)
    {
        begin = target.GetEndPosition();
        end = begin + (end - begin).normalized * length;
        return this;
    }

    public void ReturnToStart(Vector2 offset)
    {
        end -= offset;
        begin -= offset;
    }
    
    public Vector2 GetPosition() => begin;
    public Vector2 GetEndPosition() => end;
}
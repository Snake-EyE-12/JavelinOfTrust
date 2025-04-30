using System.Collections.Generic;
using UnityEngine;

public class SegmentArrangement
{
    private List<Segment> segments;
    public SegmentArrangement(List<Segment> segments)
    {
        this.segments = segments;
    }

    public void ReachTowards(Vector2 pos, Vector2 hinge)
    {
        Vector2 destination = pos;
        foreach (var segment in segments)
        {
            destination = segment.SetPosition(destination);
        }

        Vector2 offset = segments[segments.Count - 1].GetPosition() - hinge;
        foreach (var segment in segments)
        {
            segment.SetPosition(offset);
        }
    }

    public void Follow(Vector2 pos)
    {
        throw new System.NotImplementedException();
    }

    public List<Vector2> GetPoints()
    {
        List<Vector2> points = new();
        foreach (var segment in segments)
        {
            points.Add(segment.GetPosition());
        }
        return points;
    }
    
}


public class Segment
{
    private Vector2 point;
    private float length;

    public Segment(float length)
    {
        this.length = length;
    }
    public Vector2 SetPosition(Vector2 pos)
    {
        Vector2 direction = (point - pos).normalized;
        point = pos;
        return pos + direction * length;
    }

    public Vector2 GetPosition()
    {
        return point;
    }
}
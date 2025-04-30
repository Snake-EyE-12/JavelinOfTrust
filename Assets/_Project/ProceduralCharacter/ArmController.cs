using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmController : MonoBehaviour
{
    [SerializeField] private Transform leftHand;
    [SerializeField] private Transform rightHand;
    [SerializeField] private Transform shoulders;

    private SegmentArrangement leftArmArrangement;
    private SegmentArrangement rightArmArrangement;
    
    [SerializeField] private float forearmLength = 1f;
    [SerializeField] private float armLength = 1f;

    [SerializeField] private LineRenderer leftLine;
    [SerializeField] private LineRenderer rightLine;

    private void Awake()
    {
        List<Segment> segmentsLeft = new List<Segment>();
        segmentsLeft.Add(new Segment(forearmLength));
        segmentsLeft.Add(new Segment(armLength));
        List<Segment> segmentsRight = new List<Segment>();
        segmentsRight.Add(new Segment(forearmLength));
        segmentsRight.Add(new Segment(armLength));
        leftArmArrangement = new SegmentArrangement(segmentsLeft);
        rightArmArrangement = new SegmentArrangement(segmentsRight);
    }

    private void Update()
    {
        leftArmArrangement.ReachTowards(leftHand.position, shoulders.position);
        rightArmArrangement.ReachTowards(rightHand.position, shoulders.position);
        List<Vector3> leftPoints = leftArmArrangement.GetPoints().ToVector3List();
        leftPoints.Add(shoulders.position);
        List<Vector3> rightPoints = rightArmArrangement.GetPoints().ToVector3List();
        leftPoints.Add(shoulders.position);
        leftLine.Draw(leftPoints);
        rightLine.Draw(rightPoints);
    }

    
}

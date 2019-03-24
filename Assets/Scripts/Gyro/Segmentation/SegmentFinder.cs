using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SegmentFinder : MonoBehaviour
{

    public static int FindIndexOfClosestSegment(List<GyroSegment> segmentList, Vector3 currentPosition)
    {
        int closestSegmentIndex = 0;
        float closestDistanceSqr = Mathf.Infinity;
        for (int i = 0; i < segmentList.Count; i++)
        {
            var segmentPoint = segmentList[i].position;
            Vector3 directionToTarget = segmentPoint - currentPosition;
            float dSqrToTarget = directionToTarget.sqrMagnitude;
            if (dSqrToTarget < closestDistanceSqr)
            {
                closestDistanceSqr = dSqrToTarget;
                closestSegmentIndex = i;
            }
        }
        return closestSegmentIndex;
    }

    public static bool IsWithinEffectiveRange(GyroSegment segment, Vector3 currentPositon)
    {
        return Vector3.Distance(segment.position, currentPositon) > segment.radius;
    }

}

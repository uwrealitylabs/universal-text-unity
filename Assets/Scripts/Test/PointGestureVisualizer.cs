using UnityEngine;
using Oculus.Interaction.Input;
using UniversalText.Core;

public class PointGestureVisualizer : MonoBehaviour
{
    public Hand leftHand;
    public Hand rightHand;

    private LineRenderer _lineRendererLeft;
    private LineRenderer _lineRendererRight;

    void Start()
    {
        // Left hand visualizer
        GameObject lineRendererLeftObj = new GameObject("LeftVisualizer", new System.Type[] { typeof(LineRenderer) });
        lineRendererLeftObj.transform.SetParent(this.gameObject.transform);
        _lineRendererLeft = lineRendererLeftObj.GetComponent<LineRenderer>();
        _lineRendererLeft.startWidth = 0.01f;
        _lineRendererLeft.endWidth = 0.01f;
        _lineRendererLeft.enabled = false;

        // Right hand visualizer
        GameObject lineRendererRightObj = new GameObject("RightVisualizer", new System.Type[] { typeof(LineRenderer) });
        lineRendererRightObj.transform.SetParent(this.gameObject.transform);
        _lineRendererRight = lineRendererRightObj.GetComponent<LineRenderer>();
        _lineRendererRight.startWidth = 0.01f;
        _lineRendererRight.endWidth = 0.01f;
        _lineRendererRight.enabled = false;
    }

    void Update()
    {
        if (IsPoking(leftHand))
        {
            CreateRaycast(leftHand, _lineRendererLeft);
        }
        else
        {
            _lineRendererLeft.enabled = false;
        }

        if (IsPoking(rightHand))
        {
            CreateRaycast(rightHand, _lineRendererRight);
        } 
        else
        {
            _lineRendererRight.enabled = false;
        }
    }

    private bool IsPoking(Hand hand)
    {
        if (hand == null || !hand.IsConnected)
        {
            return false;
        }

        bool indexExtended = !IsFingerCurled(hand, HandFinger.Index);

        bool otherFingersCurled = 
            IsFingerCurled(hand, HandFinger.Middle) &&
            IsFingerCurled(hand, HandFinger.Ring) &&
            IsFingerCurled(hand, HandFinger.Pinky);

        return indexExtended && otherFingersCurled;
    }

    private bool IsFingerCurled(Hand hand, HandFinger finger)
    {
        if (hand.GetJointPose(GetFingerTipJoint(finger), out Pose tipPose) && hand.GetJointPose(HandJointId.HandWristRoot, out Pose basePose))
        {
            float distance = Vector3.Distance(tipPose.position, basePose.position);
            return distance < 0.1f;
        }

        return false;
    }

    private HandJointId GetFingerTipJoint(HandFinger finger)
    {
        return finger switch
        {
            HandFinger.Middle => HandJointId.HandMiddleTip,
            HandFinger.Ring => HandJointId.HandRingTip,
            HandFinger.Pinky => HandJointId.HandPinkyTip,
            _ => HandJointId.HandIndexTip
        };
    }

    private void CreateRaycast(Hand hand, LineRenderer lineRenderer)
    {
        if (hand == null) return;

        if (hand.GetJointPose(HandJointId.HandIndexTip, out Pose tipPose) && hand.GetJointPose(HandJointId.HandIndex3, out Pose basePose))
        {
            lineRenderer.enabled = true;
            lineRenderer.SetPosition(0, tipPose.position);
            lineRenderer.SetPosition(1, CalculateEnd(tipPose, basePose));
        }
    }

    private Vector3 CalculateEnd(Pose tipPose, Pose basePose)
    {
        Vector3 rayOrigin = tipPose.position;
        Vector3 rayDirection = (tipPose.position - basePose.position).normalized;
        Ray ray = new Ray(rayOrigin, rayDirection);

        Vector3 endPosition = rayOrigin + ray.direction * 10f;

        if (Physics.Raycast(ray, out RaycastHit hit, 10f))
        {
            endPosition = hit.point;
        }

        return endPosition;
    }
}
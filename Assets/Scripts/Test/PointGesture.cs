using UnityEngine;
using Oculus.Interaction.Input;
using UniversalText.Core;

public class PointGesture : MonoBehaviour
{
    public Hand leftHand;
    public Hand rightHand;
    private LineRenderer lineRenderer;

    void Start()
    {
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.startWidth = 0.01f;
        lineRenderer.endWidth = 0.01f; 
        lineRenderer.enabled = false;
    }

    void Update()
    {
        if (IsPoking(rightHand))
        {
            CreateRaycast(rightHand);
        }
        else if (IsPoking(leftHand))
        {
            CreateRaycast(leftHand);
        }
        else
        {
            lineRenderer.enabled = false; 
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

    private void CreateRaycast(Hand hand)
    {
        if (hand == null) return;

        if (hand.GetJointPose(HandJointId.HandIndexTip, out Pose tipPose) && hand.GetJointPose(HandJointId.HandWristRoot, out Pose basePose))
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
            Debug.Log($"Pointed at: {hit.collider.gameObject}");

            UniversalTextTag textTag = hit.collider.gameObject.GetComponentInParent<UniversalTextTag>();
            if (textTag != null)
            {
                Debug.Log($"Description: {textTag.ToString()}");
            }
            else
            {
                Debug.Log("UniversalTextTag not found on the hit object.");
            }

            endPosition = hit.point;
        }

        return endPosition;
    }
}
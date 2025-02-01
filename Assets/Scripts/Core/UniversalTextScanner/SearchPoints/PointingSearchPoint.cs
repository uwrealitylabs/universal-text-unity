using JetBrains.Annotations;
using Oculus.Interaction.HandGrab;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Oculus.Interaction.Input;
using Meta.XR.Editor.Tags;
using System.Linq;

namespace UniversalText.Core
{
    public class PointingSearchPoint : ISearchPoint
    {
        private Hand _rightHand;
        private Hand _leftHand;
        private float _maxDistance;

        public string Description { get => "The user is pointing at"; }

        public PointingSearchPoint(Hand rightHand, Hand leftHand, float maxDistance = 30f)
        {
            _rightHand = rightHand;
            _leftHand = leftHand;
            _maxDistance = maxDistance;
        }

        public List<UniversalTextTag> Search()
        {
            List<UniversalTextTag> pointedAt = new List<UniversalTextTag>();
            if (IsPoking(_rightHand))
            {
                pointedAt.AddRange(GetPointedAt(_rightHand));
            }
            if (IsPoking(_leftHand))
            {
                pointedAt.AddRange(GetPointedAt(_leftHand));
            }
            return pointedAt.Distinct().ToList();
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

        private List<UniversalTextTag> GetPointedAt(Hand hand)
        {
            List<UniversalTextTag> result = new List<UniversalTextTag>();
            if (hand == null) return result;

            if (hand.GetJointPose(HandJointId.HandIndexTip, out Pose tipPose) && hand.GetJointPose(HandJointId.HandIndex3, out Pose basePose))
            {
                Vector3 rayOrigin = tipPose.position;
                Vector3 rayDirection = (tipPose.position - basePose.position).normalized;
                Ray ray = new Ray(rayOrigin, rayDirection);

                Vector3 endPosition = rayOrigin + ray.direction * _maxDistance;

                if (Physics.Raycast(ray, out RaycastHit hit, _maxDistance))
                {
                    UniversalTextTag textTag = hit.collider.gameObject.GetComponentInParent<UniversalTextTag>();
                    if (textTag != null)
                    {
                        result.Add(textTag);
                    }
                }
            }
            return result;
        }
    }
}


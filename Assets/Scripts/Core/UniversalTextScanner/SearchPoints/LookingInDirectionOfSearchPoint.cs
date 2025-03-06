using Oculus.Interaction.HandGrab;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace UniversalText.Core
{
    public class LookingInDirectionOfSearchPoint : ISearchPoint
    {
        public string Description { get => "The user is looking in the direction of"; }

        private Camera _camera;
        private float _maxDistance;
        private float _angleSpread;

        public LookingInDirectionOfSearchPoint(Camera xrCamera, float maxDistance, float angleSpread)
        {
            _camera = xrCamera;
            _maxDistance = maxDistance;
            _angleSpread = angleSpread;
        }

        public List<UniversalTextTag> Search()
        {
            // Using HashSet to avoid duplicates
            HashSet<UniversalTextTag> seenTags = new HashSet<UniversalTextTag>();

            float[] offsets = { -_angleSpread, 0f, _angleSpread };

            foreach (float verticalOffset in offsets)
            {
                foreach (float horizontalOffset in offsets)
                {
                    // Rotation that offsets the camera's forward direction
                    Quaternion rotation = Quaternion.AngleAxis(horizontalOffset, _camera.transform.up) * 
                                          Quaternion.AngleAxis(verticalOffset, _camera.transform.right);
                    Vector3 rayDirection = rotation * _camera.transform.forward;

                    Ray ray = new Ray(_camera.transform.position, rayDirection);
                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit, _maxDistance))
                    {
                        GameObject obj = hit.collider.gameObject;
                        if (obj == null) continue;
                        UniversalTextTag tag = obj.GetComponentInParent<UniversalTextTag>();
                        if (tag != null)
                        {
                            seenTags.Add(tag);
                        }
                    }
                }
            }

            return new List<UniversalTextTag>(seenTags);
        }
    }
}

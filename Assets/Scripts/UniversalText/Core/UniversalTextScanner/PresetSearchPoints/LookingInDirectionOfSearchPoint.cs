using Oculus.Interaction.HandGrab;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace UniversalText.Core
{
    /// <summary>
    /// SearchPoint that retrieves gameobjects the user is looking in the general direction of, i.e. hit by one of multiple
    /// raycasts sent in a defined angle spread around the ray representing the direction the user is exactly looking in
    /// </summary>
    public class LookingInDirectionOfSearchPoint : ISearchPoint
    {
        public string Description { get => "The user is looking in the direction of"; }

        /// <summary>
        /// Camera representing user's POV (i.e. which direction they are looking in)
        /// </summary>
        private Camera _camera;
        /// <summary>
        /// Max distance that a gameobject can be detected at
        /// </summary>
        private float _maxDistance;
        /// <summary>
        /// Max spread in degrees from the exact direction the user is looking in within which a gameobject can be detected
        /// </summary>
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

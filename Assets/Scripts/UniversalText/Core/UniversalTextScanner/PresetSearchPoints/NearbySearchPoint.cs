using Oculus.Interaction.HandGrab;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace UniversalText.Core
{
    public class NearbySearchPoint : ISearchPoint
    {
        public string Description { get => "The user is near to"; }

        /// <summary>
        /// Camera representing the user's position in space
        /// </summary>
        private Camera _camera;
        /// <summary>
        /// Max distance that a gameobject can be detected at
        /// </summary>
        private float _maxDistance;
        /// <summary>
        /// Max gameobjects that can be detected as "nearby". Closest gameobjects prioritized
        /// </summary>
        private int _maxNumber;

        public NearbySearchPoint(Camera xrCamera, float maxDistance, int maxNumber)
        {
            _camera = xrCamera;
            _maxDistance = maxDistance;
            _maxNumber = maxNumber;
        }

        public List<UniversalTextTag> Search()
        { 

            GameObject[] objects = GameObject.FindObjectsOfType<GameObject>();
            if (objects == null) return new List<UniversalTextTag>();
            if (objects.Length == 0) return new List<UniversalTextTag>();
            List<(UniversalTextTag, float)> seen = new List<(UniversalTextTag, float)>();

            foreach (GameObject obj in objects)
            {
                float distance = Vector3.Distance(obj.transform.position, _camera.transform.position);
                if (distance < _maxDistance)
                {
                    if (obj == null) continue;
                    UniversalTextTag tag = obj.GetComponentInParent<UniversalTextTag>();
                    if (tag != null)
                    {
                        seen.Add((tag, distance));
                    }
                }
            }
            
            // sort by distance
            seen.Sort((x, y) => x.Item2.CompareTo(y.Item2));
            Debug.Log(seen.Count);
            Debug.Log(_maxNumber);
            List<UniversalTextTag> result = new List<UniversalTextTag>();
            for (int i = 0; i < Mathf.Min(_maxNumber, seen.Count); i++)
            {
                result.Add(seen[i].Item1);
            }

            return result;
        }
    }
}


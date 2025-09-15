using Oculus.Interaction.HandGrab;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace UniversalText.Core
{
    /// <summary>
    /// Searchpoint that retrieves the gameobject that the user is looking directly at, i.e.
    /// hit by a raycast from the player's head in the direction they are looking in.
    /// </summary>
    public class LookingSearchPoint : ISearchPoint
    {
        public string Description { get => "The user is looking at"; }

        /// <summary>
        /// Camera representing user's POV (i.e. which direction they are looking in)
        /// </summary>
        private Camera _camera;
        /// <summary>
        /// Max distance that a gameobject can be detected at
        /// </summary>
        private float _maxDistance;

        public LookingSearchPoint(Camera xrCamera, float maxDistance)
        {
            _camera = xrCamera;
            _maxDistance = maxDistance;
        }

        public List<UniversalTextTag> Search()
        { 
            List<UniversalTextTag> seen = new List<UniversalTextTag>();
            Ray ray = new Ray(_camera.transform.position, _camera.transform.forward);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, _maxDistance)) // Check if camera sees something
            {
                GameObject obj = hit.collider.gameObject;
                if (obj == null) return seen;
                UniversalTextTag tag = obj.GetComponentInParent<UniversalTextTag>();
                if (tag != null)
                {
                    seen.Add(tag);
                }
            }
            return seen;
        }
    }
}


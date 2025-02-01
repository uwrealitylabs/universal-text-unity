using Oculus.Interaction.HandGrab;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace UniversalText.Core
{
    public class LookingSearchPoint : ISearchPoint
    {
        public string Description { get => "The user is looking at"; }

        private Camera _camera;
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
                GameObject parentObj = obj.transform.parent.gameObject; //colliders are under parent gameObject that contains tag
                if (parentObj != null){
                    UniversalTextTag tag = parentObj.GetComponent<UniversalTextTag>();
                    if (tag != null){                  
                        seen.Add(tag);
                    }
                }
            }
            return seen;
        }
    }
}


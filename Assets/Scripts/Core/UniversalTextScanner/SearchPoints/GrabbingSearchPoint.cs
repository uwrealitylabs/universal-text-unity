using Oculus.Interaction.HandGrab;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UniversalText.Core
{
    public class GrabbingSearchPoint : ISearchPoint
    {
        private HandGrabInteractor _leftHandGrab;
        private HandGrabInteractor _rightHandGrab;

        public string Description { get => "The user is grabbing"; }

        public GrabbingSearchPoint(HandGrabInteractor leftHandGrab, HandGrabInteractor rightHandGrab)
        {
            _leftHandGrab = leftHandGrab;
            _rightHandGrab = rightHandGrab;
        }

        public List<UniversalTextTag> Search()
        { 
            List<UniversalTextTag> grabbed = new List<UniversalTextTag>();
            if (_leftHandGrab.HasSelectedInteractable) // Check if left hand currently grabbing something
            {
                GameObject grabbedObject = _leftHandGrab.SelectedInteractable.gameObject;
                UniversalTextTag tag = grabbedObject.GetComponentInParent<UniversalTextTag>();
                if (tag != null) // Ensure GameObject has a UTT
                {
                    grabbed.Add(tag);
                }
            }
            // Repeat for right hand
            if (_rightHandGrab.HasSelectedInteractable)
            {
                GameObject grabbedObject = _rightHandGrab.SelectedInteractable.gameObject;
                UniversalTextTag tag = grabbedObject.GetComponentInParent<UniversalTextTag>();
                if (tag != null)
                {
                    grabbed.Add(tag);
                }
            }
            return grabbed;
        }
    }
}


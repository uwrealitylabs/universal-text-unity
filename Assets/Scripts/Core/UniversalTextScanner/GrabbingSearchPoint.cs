using Oculus.Interaction.HandGrab;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UniversalText.Core
{
    public class GrabbingSearchPoint : ISearchPoint
    {
        public string Description { get => "The user is grabbing"; }

        public List<UniversalTextTag> Search()
        {
            List<UniversalTextTag> grabbedGameObjects = new List<UniversalTextTag>();

            // (implementation)

            return grabbedGameObjects;
        }

        //public List<UniversalTextTag> getGrabbedObjects()
        //{
        //    List<UniversalTextTag> tags = new List<UniversalTextTag>();
        //    if (handGrabInteractor.HasSelectedInteractable)
        //    {
        //        HandGrabInteractable grabbedObject = handGrabInteractor.SelectedInteractable;
        //        GameObject grabbedGameObject = grabbedObject.gameObject;
        //        UniversalTextTag tag = grabbedGameObject.GetComponent<UniversalTextTag>();
        //        tags.Add(tag);
        //    }
        //    else if (handGrabInteractorRight.HasSelectedInteractable)
        //    {
        //        HandGrabInteractable grabbedObject = handGrabInteractorRight.SelectedInteractable;
        //        GameObject grabbedGameObject = grabbedObject.gameObject;
        //        UniversalTextTag tag = grabbedGameObject.GetComponent<UniversalTextTag>();
        //        tags.Add(tag);
        //    }
        //    return tags;
        //}
    }
}


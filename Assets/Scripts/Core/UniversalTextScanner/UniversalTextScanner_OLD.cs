//using System;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using System.Linq;
//using Oculus.Interaction;
//using Oculus.Interaction.HandGrab;
//using Oculus.Interaction.GrabAPI;
//using Oculus.Interaction.Input;
//using Oculus.Interaction.Throw;

//namespace UniversalText.Core
//{
//    public class UniversalTextScanner_OLD : MonoBehaviour
//    {
//        //to-do: impl. point, nearby, looking at
//        public HandGrabInteractor handGrabInteractor;
//        public HandGrabInteractor handGrabInteractorRight;

//        public class SearchPoint
//        {
//            private string _description; //desc of search point, e.g. "The user is pointing at"
//            private Func<List<UniversalTextTag>> _search;
//            public SearchPoint(string description, Func<List<UniversalTextTag>> search)
//            {
//                this._description = description;
//                this._search = search;
//            }
//            public List<UniversalTextTag> Search()
//            {
//                return _search();
//            }
//            public override string ToString()
//            {
//                List<UniversalTextTag> tags = Search();
//                string representation = _description + " ";
//                if (tags.Count == 0)
//                {
//                    return "";
//                }
//                else if (tags.Count == 1)
//                {
//                    return representation + tags[0].ToString();
//                }
//                int index = 0;
//                foreach (UniversalTextTag tag in tags)
//                {
//                    string tagRep = tag.ToString();
//                    if (tag == tags.Last())
//                    {
//                        representation += " and" + tagRep + ".";
//                    }
//                    else
//                    {
//                        representation += ", " + tagRep;
//                    }
//                    index++;
//                }
//                /*
//                 * ex {"an apple on top of a plate", "a pencil on the table"} => "The user is pointing at an apple on top of a plate and
//                 * a pencil on the table, which is 60 cm by 90 cm."
//                 */
//                return representation;
//            }
//        }

//        public List<UniversalTextTag> getGrabbedObjects()
//        {
//            List<UniversalTextTag> tags = new List<UniversalTextTag>();
//            if (handGrabInteractor.HasSelectedInteractable)
//            {
//                HandGrabInteractable grabbedObject = handGrabInteractor.SelectedInteractable;
//                GameObject grabbedGameObject = grabbedObject.gameObject;
//                UniversalTextTag tag = grabbedGameObject.GetComponent<UniversalTextTag>();
//                tags.Add(tag);
//            }
//            else if (handGrabInteractorRight.HasSelectedInteractable)
//            {
//                HandGrabInteractable grabbedObject = handGrabInteractorRight.SelectedInteractable;
//                GameObject grabbedGameObject = grabbedObject.gameObject;
//                UniversalTextTag tag = grabbedGameObject.GetComponent<UniversalTextTag>();
//                tags.Add(tag);
//            }
//            return tags;
//        }

//        private void Update()
//        {
//            //grabbing SearchPoint
//            SearchPoint grab = new SearchPoint("The user is grabbing", getGrabbedObjects);
//            if (grab.Search().Count > 0)
//            {
//                Debug.Log(grab.ToString());
//            }
//        }
//    }
//}


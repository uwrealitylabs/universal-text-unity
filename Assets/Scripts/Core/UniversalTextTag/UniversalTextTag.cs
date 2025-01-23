using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace UniversalText.Core
{
    /// <summary>
    /// A tag that marks this GameObject to be recognized by the UTS and thus included in the RTR
    /// </summary>
    public class UniversalTextTag : MonoBehaviour
    {
        /// <summary>
        /// Description of the Object
        /// </summary>
        public string Description;

        /// <summary>
        /// Array containing attributes of the Object
        /// </summary>
        public List<Attribute> Attributes = new List<Attribute>();

        public override string ToString()
        {
            string representation = $"{String.Copy(Description)}";

            // Clean out invalid attributes
            List<Attribute> cleanedAttributes = new List<Attribute>();
            foreach (Attribute attribute in Attributes)
            {
                if (attribute.Valid) cleanedAttributes.Add(attribute);
            }

            if (cleanedAttributes.Count == 0)
            {
                return representation += ".";
            }
            representation += ", which ";
            if (cleanedAttributes.Count == 1)
            {
                return representation += $"{cleanedAttributes.First()}.";
            }
            foreach (Attribute attribute in cleanedAttributes)
            {
                if (attribute == cleanedAttributes.Last())
                {
                    representation += $"and {attribute}.";
                }
                else
                {
                    representation += $"{attribute}, ";
                }
            }
            return representation;
        }
    }
}

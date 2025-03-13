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

        [SerializeField] private List<AttributeConfig> attributeConfigs = new List<AttributeConfig>();

        private void Awake()
        {
            InitAttributes();
        }

        private void InitAttributes()
        {
            foreach (AttributeConfig attributeConfig in attributeConfigs)
            {
                Attribute newAttribute = attributeConfig.CreateAttribute();
                if (newAttribute != null) Attributes.Add(newAttribute);
            }
        }

        public override string ToString()
        {
            string representation = string.Copy(Description);

            // Omit invalid attributes
            List<Attribute> validAttributes = new List<Attribute>();
            foreach (Attribute attribute in Attributes)
            {
                if (attribute.Valid) validAttributes.Add(attribute);
            }

            if (validAttributes.Count == 0)
            {
                return representation;
            }
            representation += ", which ";
            if (validAttributes.Count == 1)
            {
                return representation += $"{validAttributes.First()}";
            }
            foreach (Attribute attribute in validAttributes)
            {
                if (attribute == validAttributes.Last())
                {
                    representation += $"and {attribute}";
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

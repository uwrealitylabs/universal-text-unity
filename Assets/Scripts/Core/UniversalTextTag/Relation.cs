using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UniversalText.Core
{
    /// <summary>
    /// Describes a relationship between this UniversalTextTag's GameObject and zero or more other UniversalTextTags
    /// </summary>
    public class Relation : Attribute
    {
        /// <summary>
        /// List containing UniversalTextTags attached to GameObjects that this GameObject currently holds this relation with
        /// </summary>
        private List<UniversalTextTag> Tags { get => _tagsGetter(); }

        /// <summary>
        /// Delegate referencing a method that returns UniversalTextTags attached to GameObjects that this GameObject 
        /// currently holds this relation with
        /// </summary>
        private Func<List<UniversalTextTag>> _tagsGetter;

        /// <summary>
        /// Initialize a Relation with provided format string 'formattedDescription', and delegates referencing getter functions for
        /// a List of UniversalTextTags that the relation is held with, the value of this relation, and the validity of this relation.
        /// </summary>
        /// <param name="formattedDescription">Formatted description of the Attribute, must be compatible with String.Format(formattedDescription, valueGetter(), tagsString) when Value is not null, where
        /// tagsString is the UTTs listed in natural language</param>
        /// <param name="tagsGetter">Reference to getter function that returns a list of the UniversalTextTags that this relation is currently held with</param>
        /// <param name="validGetter">Reference to getter function that returns a bool value representing whether or not this Attribute is currently relevant to the RTR</param>
        /// <param name="valueGetter">Reference to getter function that retrieves the associated attribute's current value</param>
        public Relation(string description, Func<List<UniversalTextTag>> tagsGetter, Func<bool> validGetter = null, Func<object> valueGetter = null)
            : base(description, valueGetter, validGetter)
        {
            _tagsGetter = tagsGetter;
        }

        public override string ToString()
        {
            string tagsString = "";
            if (Tags.Count == 1) { tagsString = Tags.First().ToString(); }
            else
            {
                foreach (UniversalTextTag tag in Tags)
                {
                    if (tag == Tags.Last())
                    {
                        tagsString += $"and {tag}.";
                    }
                    else
                    {
                        tagsString += $"{tag}, ";
                    }
                }
            }

            if (Value != null)
            {
                return String.Format(_description, Value, tagsString);
            }
            else
            {
                return String.Format(_description, tagsString);
            }
        }
    }
}

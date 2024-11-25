using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEditor;
using Unity.VisualScripting;
using System.Linq;
using Meta.XR.ImmersiveDebugger.UserInterface;
using Meta.XR.Editor.Tags;

/// <summary>
/// A tag that marks this GameObject to be included in the overall real-time text representation
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
        if (Attributes.Count == 0)
        {
            return representation += ".";
        }
        representation += ", which is ";
        if (Attributes.Count == 1)
        {
            return representation += $"{Attributes.First()}.";
        }
        foreach (Attribute attribute in Attributes)
        {
            
            if (attribute == Attributes.Last())
            {
                representation += $"and {attribute}.";
            } else
            {
                representation += $"{attribute}, ";
            }
        }
        return representation;
    }

    /// <summary>
    /// Describes an attribute of this UniversalTextTag's Object. Attribute values can either be initialized with a value type or
    /// a function that returns the current value of the attribute when called.
    /// </summary>
    public class Attribute : StringAggregator
    {
        /// <summary>
        /// Description of the attribute
        /// </summary>
        protected string _description;

        /// <summary>
        /// Delegate referencing a method that returns this Attribute's current value
        /// </summary>
        private Func<object> _valueGetter;

        /// <summary>
        /// Delegate referencing a method that returns whether or not this Attribute is currently valid (i.e. relevant to the RTR)
        /// </summary>
        private Func<bool> _validGetter;

        /// <summary>
        /// Current value of this Attribute
        /// </summary>
        protected object? Value { get => _valueGetter(); }

        /// <summary>
        /// Validity of this Attribute (true if relevant to the RTR, false otherwise)
        /// </summary>
        protected bool Valid { get => _validGetter(); }

        /// <summary>
        /// Initializes an Attribute with provided 'formattedDescription', and delegates referencing getter functions for 
        /// the Attribute's current value and validity.
        /// </summary>
        /// <param name="formattedDescription">Formatted description of the Attribute, must be compatible with String.Format(formattedDescription, valueGetter()) when Value is not null</param>
        /// <param name="valueGetter">Reference to getter function that retrieves the associated attribute's current value</param>
        /// <param name="validGetter">Reference to getter function that returns a bool value representing whether or not this Attribute is currently relevant to the RTR</param>
        public Attribute(string formattedDescription, Func<object> valueGetter, Func<bool> validGetter)
        {
            _description = formattedDescription;
            _valueGetter = valueGetter;
            _validGetter = validGetter;
        }

        /// <summary>
        /// Initializes an Attribute with provided 'formattedDescription', and delegate referencing a getter function for 
        /// the Attribute's current value. Attributes initialized this way are assumed to always be relevant to the RTR.
        /// </summary>
        /// <param name="formattedDescription">Formatted description of the Attribute, must be compatible with String.Format(formattedDescription, valueGetter()) when Value is not null</param>
        /// <param name="valueGetter">Reference to getter function that retrieves the associated attribute's current value</param>
        public Attribute(string formattedDescription, Func<object> valueGetter)
        {
            _description = formattedDescription;
            _valueGetter = valueGetter;
            _validGetter = () => true;
        }

        /// <summary>
        /// Initializes an Attribute with provided string 'description', and delegate referencing getter function for the Attribute's current validity.
        /// Attributes initialized this way are not linked to a value outside of the object, and thus the provided 'description' must describe the attribute 
        /// entirely, including its value, since the description cannot be changed.
        /// </summary>
        /// <param name="description">Description of the Attribute</param>
        /// <param name="validGetter">Reference to getter function that returns a bool value representing whether or not this Attribute is currently relevant to the RTR</param>
        public Attribute(string description, Func<bool> validGetter)
        {
            _description = description;
            _valueGetter = () => null;
            _validGetter = validGetter;
        }

        /// <summary>
        /// Initializes an Attribute with provided string 'description'. Attributes initialized this way are not linked to a value outside of the object, 
        /// and thus the provided 'description' must describe the attribute entirely, including its value, since the description cannot be changed.
        /// Additionally, Attributes initialized this way are assumed to always be relevant to the RTR.
        /// </summary>
        /// <param name="description">Description of the Attribute</param>
        public Attribute(string description)
        {
            _description = description;
            _valueGetter = () => null;
            _validGetter = () => true;
        }

        protected override string Aggregate()
        {
            if (Value == null)
            {
                return _description;
            }
            return String.Format(_description, Value);
        }
    }

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
        public Relation(string description, Func<List<UniversalTextTag>> tagsGetter, Func<bool> validGetter, Func<object> valueGetter)
            : base(description, valueGetter, validGetter)
        {
            _tagsGetter = tagsGetter;
        }

        /// <summary>
        /// Initialize a Relation with provided format string 'formattedDescription', and delegates referencing getter functions for
        /// a List of UniversalTextTags that the relation is held with and the validity of this relation.
        /// </summary>
        /// <param name="formattedDescription">Formatted description of the Attribute, must be compatible with String.Format(formattedDescription, tagsString) where
        /// tagsString is the UTTs listed in natural language</param>
        /// <param name="tagsGetter">Reference to getter function that returns a list of the UniversalTextTags that this relation is currently held with</param>
        /// <param name="validGetter">Reference to getter function that returns a bool value representing whether or not this Attribute is currently relevant to the RTR</param>
        public Relation(string description, Func<List<UniversalTextTag>> tagsGetter, Func<bool> validGetter)
            : base(description, validGetter)
        {
            _tagsGetter = tagsGetter;
        }

        /// <summary>
        /// Initialize a Relation with provided format string 'formattedDescription', and delegates referencing getter functions for
        /// a List of UniversalTextTags that the relation is held with and the value of this relation.
        /// </summary>
        /// <param name="formattedDescription">Formatted description of the Attribute, must be compatible with String.Format(formattedDescription, valueGetter(), tagsString) when Value is not null, where
        /// tagsString is the UTTs listed in natural language</param>
        /// <param name="tagsGetter">Reference to getter function that returns a list of the UniversalTextTags that this relation is currently held with</param>
        /// <param name="valueGetter">Reference to getter function that retrieves the associated attribute's current value</param>
        public Relation(string description, Func<List<UniversalTextTag>> tagsGetter, Func<object> valueGetter)
            : base(description, valueGetter)
        {
            _tagsGetter = tagsGetter;
        }

        /// <summary>
        /// Initialize a Relation with provided format string 'formattedDescription', and delegates referencing a getter function for
        /// a List of UniversalTextTags that the relation is held with.
        /// </summary>
        /// <param name="formattedDescription">Formatted description of the Attribute, must be compatible with String.Format(formattedDescription, tagsString) where
        /// tagsString is the UTTs listed in natural language</param>
        /// <param name="tagsGetter">Reference to getter function that returns a list of the UniversalTextTags that this relation is currently held with</param>
        public Relation(string description, Func<List<UniversalTextTag>> tagsGetter)
            : base(description)
        {
            _tagsGetter = tagsGetter;
        }

        protected override string Aggregate()
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
                    } else
                    {
                        tagsString += $"{tag}, ";
                    }
                }
            }

            if (Value != null)
            {
                return String.Format(_description, Value, tagsString);
            } else
            {
                return String.Format(_description, tagsString);
            }
        }
    }

    private void Start()
    {
        // EXAMPLE
        
        //int legs = 4;
        //Description = "A dining table";
        //Attributes.Add(new Attribute("is {0} inches wide", () => 110));
        //Attributes.Add(new Attribute("is made of {0}", () => "wood"));
        //Attributes.Add(new Attribute("has {0} legs", () => legs));
        //Debug.Log(this);
        //legs = 5;
        //Debug.Log(this);
        
        // Result:
        //  1) "A dining table, which is 110 inches wide, is made of wood, and has 4 legs."
        //  2) "A dining table, which is 110 inches wide, is made of wood, and has 5 legs."
    }
}

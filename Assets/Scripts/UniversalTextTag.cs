using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEditor;
using Unity.VisualScripting;
using System.Linq;

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
    /// Array containing relations that the Object has with other Objects
    /// </summary>
    public List<Relation> Relations = new List<Relation>();
    /// <summary>
    /// Array containing attributes of the Object
    /// </summary>
    public List<Attribute> Attributes = new List<Attribute>();

    private string _representation;
    /// <summary>
    /// Formatted string representation of the Object
    /// </summary>
    public string Representation { get => String.Format(_representation, References.ToArray()); set => _representation = value; }
    [HideInInspector] public List<Attribute> References = new List<Attribute>();

    /// <summary>
    /// Describes a relationship between this UniversalTextTag's Object and another Object
    /// </summary>
    [Serializable]
    public struct Relation
    {
        public string Description;
        public UniversalTextTag ObjectTag;
    }

    /// <summary>
    /// Describes an attribute of this UniversalTextTag's Object. Attribute values can either be initialized with a value type or
    /// a function that returns the current value of the attribute when called.
    /// </summary>
    public class Attribute
    {
        private Func<object> _getter;

        /// <summary>
        /// Description of the attribute
        /// </summary>
        public string Description;

        /// <summary>
        /// Current value of the attribute, converted to a string
        /// </summary>
        public string Value { get => String.Format(Description, _getter().ToString()); }

        /// <summary>
        /// Initializes the TagAttribute with a getter function referring to the associated attribute's value
        /// </summary>
        /// <param name="description">Description of the attribute</param>
        /// <param name="getter">Getter function that retrieves the associated attribute's current value</param>
        public Attribute(string description, Func<object> getter)
        {
            this.Description = description;
            this._getter = getter;
        }

        /// <summary>
        /// Initializes the TagAttribute with a constant value
        /// </summary>
        /// <param name="description">Description of the attribute</param>
        /// <param name="value">Value of the associated attribute</param>
        public Attribute(string description, object value)
        {
            this.Description = description;
            this._getter = () => value;
        }

        public override string ToString()
        {
            return Value;
        }
    }

    private void Start()
    {
        // EXAMPLE
        /*
        int legs = 4;
        Description = "A dining table";
        Attributes.Add(new Attribute("is {0} inches wide", 110));
        Attributes.Add(new Attribute("is made of {0}", "wood"));
        Attributes.Add(new Attribute("has {0} legs", () => legs));
        References = Attributes;
        Representation = Description + ", which {0}, {1}, and {2}.";
        Debug.Log(Representation); 
        legs = 5;
        Debug.Log(Representation); 
        */
        // Result:
        //  1) "A dining table, which is 110 inches wide, is made of wood, and has 4 legs."
        //  2) "A dining table, which is 110 inches wide, is made of wood, and has 5 legs."
        // This Representation string will be created by the UTC at Awake() for all UTTs
        // Notice that the Representation string is re-evaluated whenever its value is retrieved
        Debug.Log(ToString());
    }
}

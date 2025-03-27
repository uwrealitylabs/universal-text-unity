using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UniversalText.Core
{
    /// <summary>
    /// Describes an attribute of a UniversalTextTag's GameObject. Attribute values can either be initialized with a value type or
    /// a function that returns the current value of the attribute when called.
    /// </summary>
    public class Attribute
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
        protected object Value { get => _valueGetter(); }

        /// <summary>
        /// Validity of this Attribute (true if relevant to the RTR, false otherwise)
        /// </summary>
        public bool Valid { get => _validGetter(); }

        /// <summary>
        /// Initializes an Attribute with provided 'formattedDescription', and delegates referencing getter functions for 
        /// the Attribute's current value and validity.
        /// </summary>
        /// <param name="formattedDescription">Formatted description of the Attribute, must be compatible with String.Format(formattedDescription, valueGetter()) when Value is not null</param>
        /// <param name="valueGetter">Reference to getter function that retrieves the associated attribute's current value</param>
        /// <param name="validGetter">Reference to getter function that returns a bool value representing whether or not this Attribute is currently relevant to the RTR</param>
        public Attribute(string formattedDescription, Func<object> valueGetter = null, Func<bool> validGetter = null)
        {
            _description = formattedDescription;

            if (valueGetter != null) _valueGetter = valueGetter;
            else _valueGetter = () => null;

            if (validGetter != null) _validGetter = validGetter;
            else _validGetter = () => true;
        }

        public override string ToString()
        {
            if (Value == null)
            {
                return _description;
            }
            return String.Format(_description, Value);
        }
    }
}

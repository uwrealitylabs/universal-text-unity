using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace UniversalText.Core
{
    [Serializable]
    public class AttributeConfig
    {
        /// <summary>
        /// Formatted description of attribute. Should format with the provided value, e.g. "contains {0} fruits"
        /// </summary>
        public string formattedDescription;

        /// <summary>
        /// Component that contains the member referenced by this Attribute
        /// </summary>
        public Component targetComponent;

        /// <summary>
        /// Name of the member referenced by this Attribute
        /// </summary>
        public string memberName;


        /// <summary>
        /// Creates attribute using given config parameters
        /// </summary>
        /// <returns></returns>
        public Attribute CreateAttribute()
        {
            Type componentType = targetComponent.GetType();

            FieldInfo fieldInfo = componentType.GetField(memberName);
            PropertyInfo propertyInfo = componentType.GetProperty(memberName);

            Func<object> valueGetter;
 
            if (fieldInfo != null)
            {
                valueGetter = () => fieldInfo.GetValue(targetComponent);
            }
            else if (propertyInfo != null)
            {
                valueGetter = () => propertyInfo.GetValue(targetComponent);
            } else
            {
                Debug.LogWarning("Attempted to create attribute with invalid attribute config");
                return null;
            }
            return new Attribute(formattedDescription, valueGetter);
        }
    }
}

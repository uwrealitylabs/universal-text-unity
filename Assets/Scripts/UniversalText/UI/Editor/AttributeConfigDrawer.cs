using UnityEditor;
using UnityEngine;
using System;
using System.Reflection;
using System.Collections.Generic;
using UniversalText.Core;
using System.Linq;

[CustomPropertyDrawer(typeof(AttributeConfig))]
public class AttributeConfigDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // Calculate rects for each property field
        Rect targetRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
        Rect fieldRect = new Rect(position.x, position.y + EditorGUIUtility.singleLineHeight + 2, position.width, EditorGUIUtility.singleLineHeight);
        Rect descRect = new Rect(position.x, position.y + 2 * (EditorGUIUtility.singleLineHeight + 2), position.width, EditorGUIUtility.singleLineHeight);

        // Draw targetComponent field
        SerializedProperty compProp = property.FindPropertyRelative("targetComponent");
        EditorGUI.PropertyField(targetRect, compProp);

        // Prepare dropdown for fieldName
        SerializedProperty fieldNameProp = property.FindPropertyRelative("memberName");
        string[] options = new string[0];

        // Draw formattedDescription field
        SerializedProperty descProp = property.FindPropertyRelative("formattedDescription");
        EditorGUI.PropertyField(descRect, descProp);

        if (compProp.objectReferenceValue == null)
        {
            EditorGUI.LabelField(fieldRect, "Select a Component.");
            return;
        }

        Component comp = compProp.objectReferenceValue as Component;
        Type compType = comp.GetType();
        FieldInfo[] compFields = compType.GetFields(BindingFlags.Instance | BindingFlags.Public);
        PropertyInfo[] compProperties = compType.GetProperties(BindingFlags.Instance | BindingFlags.Public);
        if (compFields.Length == 0 && compProperties.Length == 0)
        {
            EditorGUI.LabelField(fieldRect, "Select a Component with fields/properties.");
            return;
        }

        List<string> optionsList = new List<string>();
        foreach (var compField in compFields)
        {
            optionsList.Add(compField.Name);
        }
        foreach (var compProperty in compProperties)
        {
            optionsList.Add(compProperty.Name);
        }
        options = optionsList.ToArray();
        // Find current selection index if already set.
        int index = Array.IndexOf(options, fieldNameProp.stringValue);
        if (index < 0) index = 0;
        index = EditorGUI.Popup(fieldRect, "Target Member", index, options);
        fieldNameProp.stringValue = options[index];
    }

    // Override GetPropertyHeight to ensure proper spacing.
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return 3 * (EditorGUIUtility.singleLineHeight + 2);
    }
}

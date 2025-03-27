using System;
using System.Collections.Generic;
using System.Reflection;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

namespace UniversalText.UI
{
    [CustomEditor(typeof(UniversalTextManager))]
    public class UniversalTextManagerEditor : Editor
    {
        private SerializedProperty _searchPointConfigsProp;

        private void OnEnable()
        {
            _searchPointConfigsProp = serializedObject.FindProperty("searchPointConfigs");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            DrawDefaultInspector();

            EditorGUILayout.LabelField("Search Point Config Parameters", EditorStyles.boldLabel);
            for (int i = 0; i < _searchPointConfigsProp.arraySize; ++i)
            {
                SerializedProperty element = _searchPointConfigsProp.GetArrayElementAtIndex(i);
                EditorGUILayout.PropertyField(element, new GUIContent(element.managedReferenceValue.GetType().Name), true);
            }

            if (GUILayout.Button("Add Search Point"))
            {
                GenericMenu menu = new GenericMenu();

                foreach (Type type in GetAllSearchPointConfigTypes())
                {
                    string menuName = type.Name;
                    menu.AddItem(new GUIContent(menuName), false, () => AddSearchPoint(type));
                }
                menu.ShowAsContext();
            }
            serializedObject.ApplyModifiedProperties();
        }

        private void AddSearchPoint(Type configType)
        {
            object newConfig = Activator.CreateInstance(configType);
            _searchPointConfigsProp.arraySize++;
            SerializedProperty newElement = _searchPointConfigsProp.GetArrayElementAtIndex(_searchPointConfigsProp.arraySize - 1);
            newElement.managedReferenceValue = newConfig;
            serializedObject.ApplyModifiedProperties();
        }

        private List<Type> GetAllSearchPointConfigTypes()   
        {
            List<Type> configTypes = new List<Type>();

            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (Assembly assembly in assemblies)
            {
                foreach (Type type in assembly.GetTypes())
                {
                    if (typeof(SearchPointConfig).IsAssignableFrom(type) && !type.IsAbstract)
                    {
                        configTypes.Add(type);
                    }
                }
            }

            return configTypes;
        }
    }
}
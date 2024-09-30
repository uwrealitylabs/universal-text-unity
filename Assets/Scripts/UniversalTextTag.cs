using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEditor;

// THIS IS A TEMPLATE FOR UniversalTextTag

/// <summary>
/// A tag that marks this GameObjecct to be included in the overall real-time text representation created by Universal Text
/// </summary>
public class UniversalTextTag : MonoBehaviour
{
    private int _Id;
    public string Description;
    public TagLink[] Relationships;
    /*
    public TagAttributeContainer Attributes = new TagAttributeContainer();
    */
    [HideInInspector]
    public string Representation;
    /*
    [HideInInspector]
    public TagAttributeContainer References;
    */
}

/// <summary>
/// Describes a relationship between this UniversalTextTag's Object and another Object
/// </summary>
[Serializable]
public struct TagLink
{
    public string Description;
    public UniversalTextTag ObjectTag;
}

/// <summary>
/// An iterable container for TagAttributes. 
/// </summary>
/*
[Serializable]
public class TagAttributeContainer : IEnumerable<KeyValuePair<string, ITagAttribute>>
{
    // Implementation
}
*/

/// <summary>
/// A reference to a field or a value, intended to be paired with a string in the Attributes field of a UniversalTextTag
/// </summary>
/// <typeparam name="T">Type that this TagAttribute contains</typeparam>
/*
public class TagAttribute<T> : ITagAttribute
{
    // Implementation
}
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Deepwell;
using UnityEditor;
using UnityEngine;

// CustomPropertyDrawer
[CustomPropertyDrawer(typeof(DWTagValue), true)]
public class DWTagsPropertyDrawer : PropertyDrawer
{
    // Draw the property inside the given rect
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // Using BeginProperty / EndProperty on the parent property means that
        // prefab override logic works on the entire property.
        EditorGUI.BeginProperty(position, label, property);

        // Draw label
        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

        // Don't make child fields be indented
        var indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;

        var varName = property.name;
        var parentTypeName = property.serializedObject.targetObject.GetType().Name;
        List<String> tagNames;
        if (property.serializedObject.targetObject is DWTags dwTags)
        {
            var subclass = dwTags.GetType();
            tagNames = dwTags.TageNamesForInstance();
        }
        else
        {
            var className = varName.Replace("Mask", "");
            var assemblyName = property.serializedObject.targetObject.GetType().Assembly.FullName;
            className = className.First().ToString().ToUpper() + className.Substring(1) + ", " + assemblyName;
            var theClass = Type.GetType(className);
            tagNames = DWTags.TagNamesForClass(theClass);
        }
        property.Next(true); // step into struct, to value
        var tagValue = property.intValue;
        var newTagValue = EditorGUI.MaskField(position, "", tagValue, tagNames.ToArray());
        if (newTagValue != tagValue)
        {
            property.intValue = newTagValue;
        }

        // Set indent back to what it was
        EditorGUI.indentLevel = indent;

        EditorGUI.EndProperty();
    }
}
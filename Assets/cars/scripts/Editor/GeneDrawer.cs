using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomPropertyDrawer(typeof(Gene))]
public class GeneDrawer : PropertyDrawer {
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
        SerializedProperty value = property.FindPropertyRelative("value");

        EditorGUI.BeginProperty(position, label, property);

        label.text = property.FindPropertyRelative("name").stringValue;
        position = EditorGUI.PrefixLabel(position, 
            GUIUtility.GetControlID(FocusType.Passive), label);

        var indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;

        var valueRect = new Rect(position.x, position.y, 150, position.height);

        EditorGUI.Slider(valueRect, value, 
            property.FindPropertyRelative("min").floatValue, 
            property.FindPropertyRelative("max").floatValue, GUIContent.none);

        EditorGUI.indentLevel = indent;
        EditorGUI.EndProperty();
    }
}

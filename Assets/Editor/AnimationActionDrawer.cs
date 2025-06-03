using UnityEditor;
using UnityEngine;
using System.Reflection;
using System.Collections.Generic;

[CustomPropertyDrawer(typeof(AnimationAction))]
public class AnimationActionDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        float lineHeight = EditorGUIUtility.singleLineHeight;
        Rect targetRect = new Rect(position.x, position.y, position.width, lineHeight);
        Rect methodRect = new Rect(position.x, position.y + lineHeight + 2, position.width, lineHeight);
        Rect delayRect = new Rect(position.x, position.y + (lineHeight + 2) * 2, position.width, lineHeight);
        Rect parallelRect = new Rect(position.x, position.y + (lineHeight + 2) * 3, position.width, lineHeight);

        SerializedProperty targetObjProp = property.FindPropertyRelative("targetObject");
        SerializedProperty methodNameProp = property.FindPropertyRelative("methodName");
        SerializedProperty delayProp = property.FindPropertyRelative("delayAfter");
        SerializedProperty parallelProp = property.FindPropertyRelative("runParallel");

        EditorGUI.PropertyField(targetRect, targetObjProp);

        GameObject targetObj = targetObjProp.objectReferenceValue as GameObject;

        List<string> methodOptions = new List<string>();

        if (targetObj != null)
        {
            MonoBehaviour[] scripts = targetObj.GetComponents<MonoBehaviour>();
            foreach (var script in scripts)
            {
                MethodInfo[] methods = script.GetType().GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);
                foreach (var method in methods)
                {
                    if (method.GetParameters().Length == 0) // 파라미터 없는 메서드만
                        methodOptions.Add($"{script.GetType().Name}/{method.Name}");
                }
            }
        }

        int selectedIndex = 0;
        if (!string.IsNullOrEmpty(methodNameProp.stringValue))
        {
            selectedIndex = methodOptions.IndexOf(methodNameProp.stringValue);
            if (selectedIndex < 0) selectedIndex = 0;
        }

        if (methodOptions.Count > 0)
        {
            selectedIndex = EditorGUI.Popup(methodRect, "Method", selectedIndex, methodOptions.ToArray());
            methodNameProp.stringValue = methodOptions[selectedIndex];
        }
        else
        {
            EditorGUI.LabelField(methodRect, "Method", "No Methods Found");
        }

        EditorGUI.PropertyField(delayRect, delayProp);
        EditorGUI.PropertyField(parallelRect, parallelProp);

        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return (EditorGUIUtility.singleLineHeight + 2) * 4;
    }
}

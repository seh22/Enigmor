using UnityEditor;
using UnityEngine;
using UnityEditorInternal;
using System.Collections.Generic;

[CustomEditor(typeof(CutsceneSequence))]
public class CutsceneSequenceEditor : Editor
{
    private ReorderableList reorderableList;
    private List<bool> foldouts = new List<bool>();

    // 키워드 드롭다운 목록
    private string[] availableKeywords = { "(None)", "slot0", "slot1", "slot2", "characterUI", "narrationUI", "fadeOverlay", "background", "spawnRoot" };

    private void OnEnable()
    {
        SerializedProperty actionsProp = serializedObject.FindProperty("actions");

        reorderableList = new ReorderableList(serializedObject, actionsProp, true, true, true, true);

        reorderableList.drawHeaderCallback = rect =>
        {
            EditorGUI.LabelField(rect, "Cutscene Actions");
        };

        reorderableList.elementHeightCallback = GetElementHeight;
        reorderableList.drawElementCallback = DrawElement;

        foldouts.Clear();
        for (int i = 0; i < actionsProp.arraySize; i++)
            foldouts.Add(false);
    }
    private float GetElementHeight(int index)
    {
        EnsureFoldoutListSize(index);
        if (!foldouts[index])
            return EditorGUIUtility.singleLineHeight + 12f;

        SerializedProperty element = reorderableList.serializedProperty.GetArrayElementAtIndex(index);
        float height = EditorGUIUtility.singleLineHeight + 6f;

        height += GetPropertyHeight(element.FindPropertyRelative("actionName"));
        height += GetPropertyHeight(element.FindPropertyRelative("type"));
        height += GetPropertyHeight(element.FindPropertyRelative("delayBefore"));

        height += EditorGUIUtility.singleLineHeight + 4f;
        height += GetPropertyHeight(element.FindPropertyRelative("setActive"));
        height += GetPropertyHeight(element.FindPropertyRelative("activationTiming"));

        ActionType type = (ActionType)element.FindPropertyRelative("type").enumValueIndex;
        switch (type)
        {
            case ActionType.CharacterSpeak:
                height += GetPropertyHeight(element.FindPropertyRelative("characterSprite"));
                height += GetPropertyHeight(element.FindPropertyRelative("characterName"));
                height += GetPropertyHeight(element.FindPropertyRelative("enableCharacterFade"));
                height += GetDynamicArrayHeight(element.FindPropertyRelative("dialogueTexts"));
                break;
            case ActionType.NarrationOnly:
                height += GetDynamicArrayHeight(element.FindPropertyRelative("narrationTexts"));
                break;
            case ActionType.ShowImage:
                height += GetPropertyHeight(element.FindPropertyRelative("imageSlotIndex"));
                height += GetPropertyHeight(element.FindPropertyRelative("imageSprite"));
                height += GetPropertyHeight(element.FindPropertyRelative("imageSize"));
                height += GetPropertyHeight(element.FindPropertyRelative("imagePosition"));
                break;
            case ActionType.BackgroundChange:
                height += GetPropertyHeight(element.FindPropertyRelative("backgroundSprite"));
                break;
            case ActionType.Fade:
                height += GetPropertyHeight(element.FindPropertyRelative("fadeMode"));
                height += GetPropertyHeight(element.FindPropertyRelative("duration"));

                break;
            case ActionType.Wait:
                height += GetPropertyHeight(element.FindPropertyRelative("duration"));
                break;
            case ActionType.WaitForClick:
                height += EditorGUIUtility.singleLineHeight + 4f;
                break;
            case ActionType.ChangeColor:
                height += EditorGUIUtility.singleLineHeight + 4f; // dropdown
                height += GetPropertyHeight(element.FindPropertyRelative("targetColor"));
                height += GetPropertyHeight(element.FindPropertyRelative("useFade"));
                height += GetPropertyHeight(element.FindPropertyRelative("colorChangeDuration"));
                
                break;
            case ActionType.SpawnPrefab:
                height += GetPropertyHeight(element.FindPropertyRelative("prefabToSpawn"));
                height += EditorGUIUtility.singleLineHeight + 4f;
                height += GetPropertyHeight(element.FindPropertyRelative("localPosition"));
                height += GetPropertyHeight(element.FindPropertyRelative("localScale"));
                height += GetPropertyHeight(element.FindPropertyRelative("deactivateAfterSpawn"));
                break;
            case ActionType.DespawnPrefab:
                height += EditorGUIUtility.singleLineHeight + 4f; // keyword dropdown
                height += GetPropertyHeight(element.FindPropertyRelative("immediateDestroy"));
                break;
            case ActionType.SetTitleWithAnimation:
                height += GetPropertyHeight(element.FindPropertyRelative("topbarText"));
                height += GetPropertyHeight(element.FindPropertyRelative("waitAfterPlay"));
                break;
            case ActionType.ShowChoices:
                height += GetDynamicArrayHeight(element.FindPropertyRelative("choiceTexts"));
                height += GetPropertyHeight(element.FindPropertyRelative("choiceUIPrefab"));
                height += GetPropertyHeight(element.FindPropertyRelative("buttonSpacing"));
                break;
        }

        return height + 12f;
    }
    private void DrawElement(Rect rect, int index, bool isActive, bool isFocused)
    {
        EnsureFoldoutListSize(index);
        SerializedProperty element = reorderableList.serializedProperty.GetArrayElementAtIndex(index);
        SerializedProperty nameProp = element.FindPropertyRelative("actionName");
        SerializedProperty typeProp = element.FindPropertyRelative("type");

        Rect r = new Rect(rect.x, rect.y + 4, rect.width, EditorGUIUtility.singleLineHeight);
        foldouts[index] = EditorGUI.Foldout(r, foldouts[index], string.IsNullOrEmpty(nameProp.stringValue) ? "(이름 없음)" : nameProp.stringValue, true);
        r.y += EditorGUIUtility.singleLineHeight + 6;

        if (!foldouts[index]) return;

        DrawPropertyField(element.FindPropertyRelative("actionName"), ref r, "Action Name");
        DrawPropertyField(typeProp, ref r);
        DrawPropertyField(element.FindPropertyRelative("delayBefore"), ref r);



        ActionType type = (ActionType)typeProp.enumValueIndex;

        // 드롭다운으로 targetKeyword 선택
        if (type == ActionType.ChangeColor)
            DrawKeywordDropdown(element.FindPropertyRelative("colorTargetKeyword"), ref r, "Target Keyword");
        else
            DrawKeywordDropdown(element.FindPropertyRelative("targetKeyword"), ref r, "Target Keyword");

        DrawPropertyField(element.FindPropertyRelative("setActive"), ref r);
        DrawPropertyField(element.FindPropertyRelative("activationTiming"), ref r);

        switch (type)
        {
            case ActionType.CharacterSpeak:
                DrawPropertyField(element.FindPropertyRelative("characterSprite"), ref r);
                DrawPropertyField(element.FindPropertyRelative("characterName"), ref r);
                DrawPropertyField(element.FindPropertyRelative("enableCharacterFade"), ref r);
                DrawDynamicStringArray(element.FindPropertyRelative("dialogueTexts"), ref r);
                break;
            case ActionType.NarrationOnly:
                DrawDynamicStringArray(element.FindPropertyRelative("narrationTexts"), ref r);
                break;
            case ActionType.ShowImage:
                DrawPropertyField(element.FindPropertyRelative("imageSlotIndex"), ref r);
                DrawPropertyField(element.FindPropertyRelative("imageSprite"), ref r);
                DrawPropertyField(element.FindPropertyRelative("imageSize"), ref r);
                DrawPropertyField(element.FindPropertyRelative("imagePosition"), ref r);
                break;
            case ActionType.BackgroundChange:
                DrawPropertyField(element.FindPropertyRelative("backgroundSprite"), ref r);
                break;
            case ActionType.Fade:
                DrawPropertyField(element.FindPropertyRelative("fadeMode"), ref r);
                DrawPropertyField(element.FindPropertyRelative("duration"), ref r);
                break;
            case ActionType.Wait:
                DrawPropertyField(element.FindPropertyRelative("duration"), ref r);
                break;
            case ActionType.WaitForClick:
                EditorGUI.LabelField(r, "사용자 입력을 기다립니다.");
                r.y += EditorGUIUtility.singleLineHeight + 4;
                break;
            case ActionType.ChangeColor:
                DrawPropertyField(element.FindPropertyRelative("targetColor"), ref r);
                DrawPropertyField(element.FindPropertyRelative("useFade"), ref r);
                DrawPropertyField(element.FindPropertyRelative("colorChangeDuration"), ref r);
                
                break;
            case ActionType.SpawnPrefab:
                DrawPropertyField(element.FindPropertyRelative("prefabToSpawn"), ref r);
                DrawKeywordDropdown(element.FindPropertyRelative("parentKeyword"), ref r, "Parent Keyword");
                DrawPropertyField(element.FindPropertyRelative("localPosition"), ref r);
                DrawPropertyField(element.FindPropertyRelative("localScale"), ref r);
                DrawPropertyField(element.FindPropertyRelative("deactivateAfterSpawn"), ref r);
                break;
            case ActionType.DespawnPrefab:
                DrawKeywordDropdown(element.FindPropertyRelative("despawnTargetKeyword"), ref r, "Target Keyword");
                DrawPropertyField(element.FindPropertyRelative("immediateDestroy"), ref r);
                break;
            case ActionType.SetTitleWithAnimation:
                DrawPropertyField(element.FindPropertyRelative("topbarText"), ref r, "Title Text");
                DrawPropertyField(element.FindPropertyRelative("waitAfterPlay"), ref r, "Wait After Play (sec)");
                break;
            case ActionType.ShowChoices:
                DrawDynamicStringArray(element.FindPropertyRelative("choiceTexts"), ref r);
                DrawPropertyField(element.FindPropertyRelative("choiceUIRootPrefab"), ref r, "Choice UI Root");
                DrawPropertyField(element.FindPropertyRelative("choiceButtonPrefab"), ref r, "Choice Button");
                break;

        }
    }
    private void DrawDynamicStringArray(SerializedProperty arrayProp, ref Rect r)
    {
        DrawPropertyField(arrayProp.FindPropertyRelative("Array.size"), ref r);
        for (int i = 0; i < arrayProp.arraySize; i++)
        {
            SerializedProperty textProp = arrayProp.GetArrayElementAtIndex(i);
            float h = EditorGUIUtility.singleLineHeight * 3f;
            textProp.stringValue = EditorGUI.TextArea(new Rect(r.x, r.y, r.width, h), textProp.stringValue);
            r.y += h + 4f;
        }
    }
    private void DrawKeywordDropdown(SerializedProperty prop, ref Rect r, string label)
    {
        string current = prop.stringValue;
        int index = Mathf.Max(0, System.Array.IndexOf(availableKeywords, current));
        int selected = EditorGUI.Popup(
            new Rect(r.x, r.y, r.width, EditorGUIUtility.singleLineHeight),
            label,
            index,
            availableKeywords
        );
        prop.stringValue = selected == 0 ? "" : availableKeywords[selected];
        r.y += EditorGUIUtility.singleLineHeight + 4;
    }
    private float GetPropertyHeight(SerializedProperty prop)
    {
        if (prop == null)
            return EditorGUIUtility.singleLineHeight + 4f;

        return EditorGUI.GetPropertyHeight(prop, true) + 4f;
    }
    private float GetDynamicArrayHeight(SerializedProperty arrayProp)
    {
        float total = EditorGUIUtility.singleLineHeight + 6f;
        for (int i = 0; i < arrayProp.arraySize; i++)
            total += EditorGUIUtility.singleLineHeight * 3f + 4f;
        return total;
    }
    private void DrawPropertyField(SerializedProperty prop, ref Rect r, string label = null)
    {
        if (prop == null)
            return;

        float h = EditorGUI.GetPropertyHeight(prop, true);
        if (label != null)
            EditorGUI.PropertyField(new Rect(r.x, r.y, r.width, h), prop, new GUIContent(label), true);
        else
            EditorGUI.PropertyField(new Rect(r.x, r.y, r.width, h), prop, true);

        r.y += h + 4f;



    }
    private void EnsureFoldoutListSize(int index)
    {
        while (foldouts.Count <= index)
            foldouts.Add(false);
    }
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        reorderableList.DoLayoutList();
        serializedObject.ApplyModifiedProperties();
    }
}

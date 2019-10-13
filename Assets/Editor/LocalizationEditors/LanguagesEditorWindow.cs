using UnityEngine;
using UnityEditor;

public class LanguagesEditorWindow : EditorWindow {

    LocalizationText localizationText;

    Vector2 leftScrollPos;
    Vector2 rightScrollPos;

    float leftSizeWidth = 200;

    public void init(LocalizationText localizationTextToUse)
    {
        localizationText = localizationTextToUse;
        // display localizedText edition
        if (localizationText != null)
        {
            EditorGUI.FocusTextInControl("");
            EditorGUILayout.BeginHorizontal();
            displayLanguage();
            displayLanguageFields();
            EditorGUILayout.EndHorizontal();
        }

    }

    /// <summary>
    /// display the list of the current available languages
    /// </summary>
    void displayLanguage()
    {
        EditorGUILayout.BeginVertical("Box", GUILayout.Width(leftSizeWidth));

        EditorGUILayout.LabelField("Languages : ", new GUIStyle(GUI.skin.label) { fontStyle = FontStyle.Bold, alignment = TextAnchor.MiddleCenter });
        EditorGUILayout.Space();

        leftScrollPos = EditorGUILayout.BeginScrollView(leftScrollPos, GUILayout.Width(leftSizeWidth + 50));

        for (int i = 0; i < localizationText.fileAndLang.Count; i++)
        {
            if (localizationText.fileAndLang[i].file == null)
                continue;

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(localizationText.fileAndLang[i].language);
            if (GUILayout.Button("Edit"))
            {
                localizationText.loadLocalizedText(localizationText.fileAndLang[i].language);
            }
            EditorGUILayout.EndHorizontal();
        }

        EditorGUILayout.EndScrollView();
        EditorGUILayout.EndVertical();

    }

    /// <summary>
    /// Display the form to edit a language
    /// </summary>
    void displayLanguageFields()
    {
        EditorGUILayout.BeginVertical();
        rightScrollPos = EditorGUILayout.BeginScrollView(leftScrollPos);
   
        if (!localizationText.hasLanguageDataLoaded())
        {
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndScrollView();
            return;
        }

        EditorGUILayout.LabelField("List of the " + localizationText.currentLangLoaded  + " language text : ", new GUIStyle(GUI.skin.label) { fontStyle = FontStyle.Bold, alignment = TextAnchor.MiddleCenter });
        EditorGUILayout.Space();


        for (int i = 0; i < localizationText.localizationDatas.elements.Count; i++)
        {
            localizationText.localizationDatas.elements[i].text = EditorGUILayout.TextField(localizationText.localizationDatas.elements[i].key, localizationText.localizationDatas.elements[i].text);
        }

        EditorGUILayout.EndScrollView();


        if (GUILayout.Button("Save language"))
        {
            localizationText.saveLocalizedText();
        }

        EditorGUILayout.Space();

        EditorGUILayout.EndVertical();
    }
}
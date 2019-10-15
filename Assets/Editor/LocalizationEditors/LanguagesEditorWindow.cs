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

        leftScrollPos = EditorGUILayout.BeginScrollView(leftScrollPos, GUILayout.Width(leftSizeWidth + 100));

        for (int i = 0; i < localizationText.fileAndLang.Count; i++)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(localizationText.fileAndLang[i].language);
            if (GUILayout.Button("Edit"))
            {
                localizationText.loadLocalizedText(localizationText.fileAndLang[i].language);
            }

            if (GUILayout.Button("Remove"))
            {
                if (EditorUtility.DisplayDialog("Are you sure ?", "Do you want to delete " + localizationText.fileAndLang[i].language + " language ?", "Yes", "No"))
                {
                    localizationText.fileAndLang.RemoveAt(i);
                }
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
        rightScrollPos = EditorGUILayout.BeginScrollView(rightScrollPos);
   
        if (!localizationText.hasLanguageDataLoaded())
        {
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndScrollView();
            return;
        }

        if (localizationText.fileAndLang.Find(x => x.language == localizationText.currentLangLoaded).file == null)
        {
            EditorGUILayout.LabelField("No JSON file in the Localized Text  ", new GUIStyle(GUI.skin.label) { fontStyle = FontStyle.Bold, alignment = TextAnchor.MiddleCenter });
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
            EditorGUI.FocusTextInControl("");
            localizationText.unloadLocalizationData();
        }

        EditorGUILayout.Space();

        EditorGUILayout.EndVertical();

    }
}
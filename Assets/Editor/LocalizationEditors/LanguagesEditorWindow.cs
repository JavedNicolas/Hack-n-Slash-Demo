using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class LanguagesEditorWindow : EditorWindow {

    LocalizationText localizationText;

    Vector2 leftScrollPos;
    Vector2 rightScrollPos;

    float leftSizeWidth = 200;
    float keyAndTextHeight = 50;

    // keys and categories
    bool[] displayKeyTypeCategory;
    List<List<LocalizationElement>> localizationTextOrdererByKey;

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

        leftScrollPos = EditorGUILayout.BeginScrollView(leftScrollPos, GUILayout.Width(leftSizeWidth + 130));

        for (int i = 0; i < localizationText.fileAndLang.Count; i++)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(localizationText.fileAndLang[i].language);
            if (GUILayout.Button("Edit"))
            {
                localizationText.loadLocalizedText(localizationText.fileAndLang[i].language);
                localizationTextOrdererByKey = localizationText.getElementsByType();
                displayKeyTypeCategory = new bool[localizationTextOrdererByKey.Count];
                // display the first elements
                if(displayKeyTypeCategory.Length> 0)
                    displayKeyTypeCategory[0] = true;
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

        // display text order by key type


        for(int j = 0; j < localizationTextOrdererByKey.Count; j++)
        {
            List<LocalizationElement> itemsForKeyType = localizationTextOrdererByKey[j];
            string[] keySplitted = itemsForKeyType[0].key.Split('_');

            EditorGUILayout.BeginHorizontal("Box");
            EditorGUILayout.LabelField(keySplitted[keySplitted.Length -1] + " : ", new GUIStyle(GUI.skin.label) { fontStyle = FontStyle.Bold, alignment = TextAnchor.MiddleCenter });

            // hide or display the category
            if (GUILayout.Button(displayKeyTypeCategory[j] ? "Hide" : "Display"))
                displayKeyTypeCategory[j] = !displayKeyTypeCategory[j];
            EditorGUILayout.EndHorizontal();

            if(displayKeyTypeCategory[j])
                for (int i = 0; i < itemsForKeyType.Count; i++)
                {
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField(itemsForKeyType[i].key, GUILayout.Height(keyAndTextHeight), GUILayout.Width(200));
                    itemsForKeyType[i].text = EditorGUILayout.TextArea(itemsForKeyType[i].text, GUILayout.Height(keyAndTextHeight));
                    EditorGUILayout.EndHorizontal();
                }
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
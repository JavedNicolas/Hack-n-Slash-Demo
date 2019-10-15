using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;

public class LocalizationKeyEditorWindow : EditorWindow
{
    static LocalizationText localizationText;

    // new key attributs
    string newKey;
    string[] newTexts;
    string[] langs;

    // scroll view positions
    Vector2 scrollPos;
    Vector2 scrollPosForKeys;

    // displayer boolean
    bool displayList;
    bool displayNewKeyForm;

    // size 
    float keyAndTextHeight = 50;
    float keyWidth = 150;

    // key editor
    bool keyEditorMode = false;
    List<string> keysName;
    string[] keys;

    public void init(LocalizationText localizationTextToUse)
    {
        localizationText = localizationTextToUse;
        
        if (!displayList && !displayNewKeyForm)
        {
            if (GUILayout.Button("List of the keys"))
                displayList = true;

            if (GUILayout.Button("Add a key"))
            {
                newTexts = new string[localizationText.fileAndLang.Count];
                langs = new string[localizationText.fileAndLang.Count];
                displayNewKeyForm = true;
            }
        }

        if (displayList)
            displayKey();

        if (displayNewKeyForm)
            addKey();
    }

    void displayKey()
    {
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button(" < Back", GUILayout.Width(50)))
            displayList = false;
       
        if (GUILayout.Button(keyEditorMode ? "Validate Change" : "Edit Key Names", GUILayout.Width(150)))
        {
            if (keyEditorMode)
                updateKeysName();
            else
                keysName = localizationText.getKeys().ToList();
            keyEditorMode = !keyEditorMode;
        }
        EditorGUILayout.EndHorizontal();

        keys = localizationText.getKeys();

        scrollPosForKeys = EditorGUILayout.BeginScrollView(scrollPosForKeys);

        // Editor mode
        if (keyEditorMode)
        {
            keys = localizationText.getKeys();
            for (int i = 0; i < keys.Length; i++)
            {
                EditorGUILayout.BeginHorizontal("Box");
                GUILayout.FlexibleSpace();
                keysName[i] = EditorGUILayout.TextField(keysName[i], GUILayout.Width(350));
                GUILayout.FlexibleSpace();
                EditorGUILayout.EndHorizontal();
            }
        }
        // classic mode
        else
        {
            for (int i = 0; i < keys.Length; i++)
            {
                EditorGUILayout.BeginHorizontal("Box");

                EditorGUILayout.LabelField(keys[i], new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter });
                if (GUILayout.Button("Remove", GUILayout.Width(150)) && EditorUtility.DisplayDialog("Are you sure ?", "Do you want to delete the key : " + keys[i] + " ?", "Yes", "No"))
                {
                    removeKey(keys[i]);
                }
                EditorGUILayout.EndHorizontal();
            }
        }

        EditorGUILayout.EndScrollView();
    }

    void removeKey(string key)
    {
        localizationText.removeKey(key);
    }

    void addKey()
    {
        for(int i = 0; i < langs.Length; i++)
            langs[i] = localizationText.fileAndLang[i].language;

        EditorGUILayout.BeginHorizontal("Box");
        GUILayout.FlexibleSpace();
        newKey = EditorGUILayout.TextField("New Key : ", newKey, GUILayout.Width(350));
        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();

        if (localizationText.baseLocalizationDatas.elements.Exists(x => x.key == newKey) && EditorUtility.DisplayDialog("Error", "The key already exist !", "Ok"))
        {
            newKey = "";
            
            EditorGUI.FocusTextInControl("");
        }

        EditorGUILayout.Space();

        EditorGUILayout.BeginVertical();

        EditorGUILayout.LabelField("Set a value for each language", new GUIStyle(GUI.skin.label) { fontStyle = FontStyle.Bold });

        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
        for (int i = 0; i < newTexts.Length; i++)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Text for " + langs[i] + ": ", new GUIStyle(GUI.skin.label) { fontStyle = FontStyle.Bold }, GUILayout.Height(keyAndTextHeight), GUILayout.Width(keyWidth));
            newTexts[i] = EditorGUILayout.TextArea(newTexts[i], GUILayout.Height(keyAndTextHeight));
            EditorGUILayout.EndHorizontal();
        }

        EditorGUILayout.EndScrollView();

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Validate"))
                saveNewKey();

            if (GUILayout.Button("Cancel"))
                displayNewKeyForm = false;

            EditorGUILayout.EndHorizontal();

        EditorGUILayout.EndVertical();
    }

    void saveNewKey()
    {
        localizationText.addKeyToLanguages(newKey, newTexts, langs);
        displayNewKeyForm = false;
        newKey = "";
        EditorGUI.FocusTextInControl("");
    }

    void updateKeysName()
    {
        localizationText.updateKeysName(keysName, keys);
    }
}
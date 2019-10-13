using UnityEngine;
using UnityEditor;

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

    public void init(LocalizationText localizationTextToUse)
    {
        localizationText = localizationTextToUse;


        if(!displayList && !displayNewKeyForm)
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
        if (GUILayout.Button("Back", GUILayout.Width(50)))
            displayList = false;

        localizationText.loadLocalizedText("English");

        scrollPosForKeys = EditorGUILayout.BeginScrollView(scrollPosForKeys);

        for (int i = 0; i < localizationText.localizationDatas.elements.Count; i++)
        {
            EditorGUILayout.BeginHorizontal("Box");
            EditorGUILayout.LabelField(localizationText.localizationDatas.elements[i].key, new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter });
            if (GUILayout.Button("Remove"))
                removeKey(localizationText.localizationDatas.elements[i].key);
            EditorGUILayout.EndHorizontal();
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

        newKey = EditorGUILayout.TextField("New Key : ", newKey);
        EditorGUILayout.Space();

        EditorGUILayout.BeginVertical("Box");

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
        EditorGUI.FocusTextInControl("");
    }
}
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class NewLanguageEditorWindow : EditorWindow
{
    static LocalizationText localizationText;

    string newLang;
    LocalizationElement[] newTexts;

    Vector2 scrollPos;

    float keyAndValueHeight = 50;
    float keyWidth = 300;

    public void init(LocalizationText localizationTextToUse)
    {
        localizationText = localizationTextToUse;

        if(newTexts == null || newTexts.Length != localizationText.getTextElementCount())
        {
            newTexts = new LocalizationElement[localizationText.getTextElementCount()];
            for (int i = 0; i < newTexts.Length; i++)
                newTexts[i] = new LocalizationElement();
        }
     
        displayForm();
    }

    private void displayForm()
    {
        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
        localizationText.loadLocalizedText("English");
        newLang = EditorGUILayout.TextField("Language name : ", newLang);

        EditorGUILayout.LabelField("Please set those key's texts : ", new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter, fontStyle = FontStyle.Bold });
        EditorGUILayout.Space();

        for (int i = 0; i < newTexts.Length; i++)
        {
            string text = newTexts[i].text;
            string key = localizationText.localizationDatas.elements[i].key;
            string englishText = localizationText.localizationDatas.elements[i].text;

            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.BeginHorizontal("Box");
            // display the key with a tool showing the english value for this key
            EditorGUILayout.LabelField(
                new GUIContent(key + " : \n (hover for the current english value)", "The English Value is : \n" + englishText),
                new GUIStyle(GUI.skin.label) { fontStyle = FontStyle.BoldAndItalic, alignment = TextAnchor.MiddleCenter },
                GUILayout.Height(keyAndValueHeight),
                GUILayout.Width(keyWidth));
            EditorGUILayout.EndHorizontal();

            // value
            newTexts[i].text = EditorGUILayout.TextArea(newTexts[i].text, GUILayout.Height(keyAndValueHeight));
            newTexts[i].key = key;

            EditorGUILayout.EndHorizontal();
        }

        EditorGUILayout.EndScrollView();

        if (GUILayout.Button("Validate"))
        {
            createLang();
        }
    }

    void createLang()
    {
        string fileNameWithoutExtension = newLang + "LocalizedText";
        string fileName = fileNameWithoutExtension + ".json";
        string filePath = ScriptableObjectConstant.FullLocalizedJsonFolder + fileName;

        JsonWrappingClass<LocalizationElement> jsonWrapper = new JsonWrappingClass<LocalizationElement>(newTexts.ToList());
        string dataAsJson = JsonUtility.ToJson(jsonWrapper);
        File.WriteAllText(filePath, dataAsJson);

        localizationText.addLanguage(newLang, (Object)Resources.Load<Object>(ScriptableObjectConstant.ResourceLocalizedJsonFolder + fileNameWithoutExtension));
        newLang = "";
        newTexts = new LocalizationElement[0];
        EditorGUI.FocusTextInControl("");
    }
}
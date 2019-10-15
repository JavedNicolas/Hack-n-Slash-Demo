using UnityEngine;
using UnityEditor;

public class LocalizationTextHubEditorWindow : EditorWindow
{
    static LocalizationTextHubEditorWindow window;
    [SerializeField] LocalizationText localizationText;

    // Tabs
    static LanguagesEditorWindow languagesEditor;
    static NewLanguageEditorWindow newLanguageEditor;
    static LocalizationKeyEditorWindow newLocalizationKeyEditor;

    static int currentTab;

    [MenuItem("Localization/Edit Text")]
    static void showWindow()
    {
        window = new LocalizationTextHubEditorWindow();

        languagesEditor = new LanguagesEditorWindow();
        newLanguageEditor = new NewLanguageEditorWindow();
        newLocalizationKeyEditor = new LocalizationKeyEditorWindow();

        window.minSize = new Vector2(700, 500);
        window.Show();
    }

    #region localization menu items
    [MenuItem("Localization/Edit languages")]
    static void showAbilityDB()
    {
        showWindow();
        currentTab = 0;
    }

    [MenuItem("Localization/Localizaion Keys")]
    static void showEnemyDB()
    {
        showWindow();
        currentTab = 1;
    }

    [MenuItem("Localization/Add language")]
    static void showItemDB()
    {
        showWindow();
        currentTab = 2;
    }
    #endregion

    private void OnGUI()
    {
        // set localizedText 
        if (localizationText == null)
        {
            localizationText = Resources.Load<LocalizationText>(ScriptableObjectConstant.localizedTextPath);
            localizationText.unloadLocalizationData();
        }

        localizationText = (LocalizationText)EditorGUILayout.ObjectField(localizationText, typeof(LocalizationText), false);

        currentTab = GUILayout.Toolbar(currentTab, new string[] { "Edit Languages", "Add / Remove Keys", "Add languages" });
        EditorGUILayout.Space();
        switch (currentTab)
        {
            case 0: languagesEditor.init(localizationText); ; break;
            case 1: newLocalizationKeyEditor.init(localizationText); ; break;
            case 2: newLanguageEditor.init(localizationText);  break;
        }
    }

}
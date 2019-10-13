using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;
using System.IO;

[CreateAssetMenu(fileName = "LocalizedText", menuName = "Localization/LocalizedText")]
public class LocalizationText : ScriptableObject
{
    [SerializeField] List<LanguageFiles> _fileAndLang;
    public List<LanguageFiles> fileAndLang => _fileAndLang;

    LocalizationDatas _localizationDatas;
    public LocalizationDatas localizationDatas => _localizationDatas;

    string _currentLangLoaded = "";
    public string currentLangLoaded => _currentLangLoaded;

    public void addLanguage(string name, Object file)
    {
        _fileAndLang.Add(new LanguageFiles(name, file));
    }

    /// <summary>
    /// return the number of element of the english Language
    /// </summary>
    public int getTextElementCount()
    {
        loadLocalizedText("English");
        return _localizationDatas == null || localizationDatas.elements == null ? 0 :  localizationDatas.elements.Count;
    }

    /// <summary>
    /// Add a new key to all the language
    /// </summary>
    /// <param name="key">The key to add</param>
    /// <param name="newText">The text for each language</param>
    /// <param name="langs">The language in the same order as the text array</param>
    public void addKeyToLanguages(string key, string[] newText, string[] langs)
    {
        for(int i =0; i < langs.Length; i++)
        {
            loadLocalizedText(langs[i]);
            _localizationDatas.elements.Add(new LocalizationItem(key, newText[i]));
            saveLocalizedText();
        }
    }

    /// <summary>
    /// remove a key from all languages
    /// </summary>
    /// <param name="key"></param>
    public void removeKey(string key)
    {
        for (int i = 0; i < _fileAndLang.Count; i++)
        {
            loadLocalizedText(_fileAndLang[i].language);
            int index = localizationDatas.elements.FindIndex(x => x.key == key);
            if (index != -1)
                localizationDatas.elements.RemoveAt(index);
            saveLocalizedText();
        }
    }

    /// <summary>
    /// Load the localized text from the json
    /// </summary>
    /// <param name="lang">The language to load</param>
    public void loadLocalizedText(string lang)
    {
        LanguageFiles languageFiles = _fileAndLang.Find(x => x.language == lang);
        if (languageFiles.file != null)
        {
            string filePath = AssetDatabase.GetAssetPath(languageFiles.file);
            string dataAsJson = File.ReadAllText(filePath);
            _localizationDatas = JsonUtility.FromJson<LocalizationDatas>(dataAsJson);
            _currentLangLoaded = lang;
        }
    }

    /// <summary>
    /// Save the localized text currently opened
    /// </summary>
    public void saveLocalizedText()
    {
        string dataAsJson = JsonUtility.ToJson(localizationDatas);

        LanguageFiles languageFiles = _fileAndLang.Find(x => x.language == _currentLangLoaded);
        string filePath = AssetDatabase.GetAssetPath(languageFiles.file);
        File.WriteAllText(filePath, dataAsJson);
    }

    public bool hasLanguageDataLoaded()
    {
        return localizationDatas != null ? true : false;
    }

    public void unloadLocalizationData()
    {
        _localizationDatas = null;
        _currentLangLoaded = "";
    }
}

[System.Serializable]
public struct LanguageFiles
{
    [SerializeField] public string language;
    [SerializeField] public Object file;

    public LanguageFiles(string language, Object file)
    {
        this.language = language;
        this.file = file;
    }
}

using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;
using System.IO;

[CreateAssetMenu(fileName = "LocalizedText", menuName = "Localization/LocalizedText")]
public class LocalizationText : ScriptableObject
{
    [SerializeField] List<LanguageFiles> _fileAndLang;
    LocalizationDatas _localizationDatas;
    LocalizationDatas _baseLocalizationDatas;
    string _currentLangLoaded = "";

    public List<LanguageFiles> fileAndLang => _fileAndLang;
    public LocalizationDatas localizationDatas => _localizationDatas;
    public LocalizationDatas baseLocalizationDatas => _baseLocalizationDatas;
    public string currentLangLoaded => _currentLangLoaded;


    /// <summary>
    /// Load the base language which is english
    /// </summary>
    public void loadBaseLanguage()
    {
        loadLocalizedText("English", true);
    }

    /// <summary>
    /// get the key for a key
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public string getTextForKey(string key)
    {
        LocalizationElement localizationItem = _localizationDatas.elements.Find(x => x.key == key);
        return localizationItem == null ? "" : localizationItem.text;
    }

    /// <summary>
    /// Add a ned language
    /// </summary>
    /// <param name="name"></param>
    /// <param name="file"></param>
    public void addLanguage(string name, Object file)
    {
        _fileAndLang.Add(new LanguageFiles(name, file));
    }

    /// <summary>
    /// return the number of element of the english Language
    /// </summary>
    public int getTextElementCount()
    {
        return _baseLocalizationDatas == null || _baseLocalizationDatas.elements == null ? 0 : _baseLocalizationDatas.elements.Count;
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
            _localizationDatas.elements.Add(new LocalizationElement(key, newText[i]));
            saveLocalizedText();
            loadBaseLanguage();
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
            loadBaseLanguage();
        }
    }

    /// <summary>
    /// Load the localized text from the json
    /// </summary>
    /// <param name="lang">The language to load</param>
    public void loadLocalizedText(string lang, bool baseLang = false)
    {
        LanguageFiles languageFiles = _fileAndLang.Find(x => x.language == lang);
        if (languageFiles.file != null)
        {
            string filePath = AssetDatabase.GetAssetPath(languageFiles.file);
            string dataAsJson = File.ReadAllText(filePath);
            if(baseLang)
                _baseLocalizationDatas = JsonUtility.FromJson<LocalizationDatas>(dataAsJson);
            else
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

    /// <summary>
    /// check if there is a language loaded
    /// </summary>
    /// <returns></returns>
    public bool hasLanguageDataLoaded()
    {
        return localizationDatas != null ? true : false;
    }

    /// <summary>
    /// Unload localizatation
    /// </summary>
    public void unloadLocalizationData()
    {
        _localizationDatas = null;
        _currentLangLoaded = "";
    }

    /// <summary>
    /// Get a list of all the keys
    /// </summary>
    /// <returns></returns>
    public string[] getKeys()
    {
        List<string> keys = new List<string>();
        foreach(LocalizationElement elements in _baseLocalizationDatas.elements)
        {
            keys.Add(elements.key);
        }

        return keys.ToArray();
    }

    /// <summary>
    /// get all the localization element organized by type
    /// </summary>
    /// <returns></returns>
    public List<List<LocalizationElement>> getElementsByType()
    {
        List<List<LocalizationElement>> elementByType = new List<List<LocalizationElement>>();

        List<string> keyTypes = new List<string>();
        foreach(string key in getKeys())
        {
            // split the key
            string[] splittedKey = key.Split('_');
            // get the last part
            string lastSplittedKeyPart = splittedKey[splittedKey.Length - 1];

            // if it's not already in the array add it to the list
            if (!keyTypes.Contains(lastSplittedKeyPart))
                keyTypes.Add(lastSplittedKeyPart);
        }

        foreach(string keyType in keyTypes)
        {
            List<LocalizationElement> localizationElements = new List<LocalizationElement>();
            localizationElements.AddRange(localizationDatas.elements.FindAll(x => x.key.Contains(keyType)));
            elementByType.Add(localizationElements);
        }

        return elementByType;
    }

    public void updateKeysName(List<string> keysNameModifed, string[] oldKeys)
    {
        foreach(LanguageFiles languageFiles in _fileAndLang)
        {
            loadLocalizedText(languageFiles.language);
            for (int i = 0; i < keysNameModifed.Count; i++)
            {
                LocalizationElement element = localizationDatas.elements.Find(x => x.key == oldKeys[i]);
                if (element != null)
                    element.key = keysNameModifed[i];

            }
            saveLocalizedText();
        }    
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

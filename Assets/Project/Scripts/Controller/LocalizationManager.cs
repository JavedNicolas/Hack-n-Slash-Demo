using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LocalizationManager : MonoBehaviour
{
    [SerializeField] LocalizationText _localizationText;

    static public LocalizationManager instance;

    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(this);
        _localizationText.unloadLocalizationData();
    }

    public string getText(string key)
    {
        return _localizationText.getTextForKey(key);
    }

    public void changeLang(string lang)
    {
        _localizationText.loadLocalizedText(lang);
    }

    public string[] getAvailableLang()
    {
        List<string> langs = new List<string>();
        foreach(LanguageFiles languageFiles in _localizationText.fileAndLang)
        {
            langs.Add(languageFiles.language);
        }

        return langs.ToArray();
    }
}

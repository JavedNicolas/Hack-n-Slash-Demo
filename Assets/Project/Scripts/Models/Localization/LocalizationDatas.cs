using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LocalizationDatas
{
    [SerializeField] public List<LocalizationElement> elements = new List<LocalizationElement>();
}

[System.Serializable]
public class LocalizationElement
{
    [SerializeField] public string key;
    [SerializeField] public string text;

    public LocalizationElement()
    {
    }

    public LocalizationElement(string key, string text)
    {
        this.key = key;
        this.text = text;
    }
}

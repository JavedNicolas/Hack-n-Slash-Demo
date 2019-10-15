using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LocalizationDatas
{
    [SerializeField] public List<LocalizationItem> elements = new List<LocalizationItem>();
}

[System.Serializable]
public class LocalizationItem
{
    [SerializeField] public string key;
    [SerializeField] public string text;

    public LocalizationItem()
    {
    }

    public LocalizationItem(string key, string text)
    {
        this.key = key;
        this.text = text;
    }
}

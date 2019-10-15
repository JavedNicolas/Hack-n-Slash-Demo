using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;

public static class StringExtension
{
    public static string putValue(this string str, List<float> remplaceWith)
    {
        string bufferString = "";
        for(int i = 0; i < remplaceWith.Count; i++)
        {
            string remplacableString = "{" + i.ToString() + "}";
            if (str.Contains(remplacableString))
            {
                bufferString += str.Replace(remplacableString, remplaceWith[i].ToString());
            }
        }
        return bufferString;
    }

    public static string localize(this string str)
    {
        return LocalizationManager.instance == null ? "" : LocalizationManager.instance.getText(str);
    }

    /// <summary>
    /// return the string in lowercase with a first character in upper case
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static string sentenceFormat(this string str)
    {
        string formatedString = str.ToLower();
        return char.ToUpper(formatedString[0]) + formatedString.Substring(1);
    }
}

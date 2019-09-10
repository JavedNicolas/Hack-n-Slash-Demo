using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

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
}

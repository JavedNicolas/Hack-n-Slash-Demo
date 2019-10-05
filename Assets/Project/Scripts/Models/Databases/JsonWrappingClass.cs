using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class JsonWrappingClass<T>
{
    [SerializeField] public List<T> elements = new List<T>();

    public JsonWrappingClass(List<T> elements)
    {
        this.elements = elements;
    }
}
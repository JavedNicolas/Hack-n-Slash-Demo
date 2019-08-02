using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Database<T> : ScriptableObject
{
    [SerializeField] List<T> elements = new List<T>();

    public T getElementAt(int index)
    {
        if (index >= elements.Count)
            return default(T);
        else
            return elements[index];
    }

    public T getRandomElement()
    {
        return elements[Random.Range(0, elements.Count)];
    }

    public int getDatabaseSize()
    {
        return elements.Count;
    }

    public void addElement(T element)
    {
        elements.Add(element);
    }

    public void removeElement(T element)
    {
        elements.Remove(element);
    }

    public void updateElement(T element, int index)
    {
        elements[index] = element;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[System.Serializable]
public class Database<DatabaseElement> : ScriptableObject
{
    [SerializeField] protected List<DatabaseElement> _elements = new List<DatabaseElement>();
    public List<DatabaseElement> elements { get => _elements; }

    public DatabaseElement getElementAt(int index)
    {
        if (index >= _elements.Count)
            return default(DatabaseElement);
        else
            return _elements[index];
    }

    public DatabaseElement getRandomElement()
    {
        return _elements[Random.Range(0, _elements.Count)];
    }

    public int getDatabaseSize()
    {
        return _elements.Count;
    }

    public void addElement(DatabaseElement element)
    {
        _elements.Add(element);
    }

    public void removeElement(DatabaseElement element)
    {
        _elements.Remove(element);
    }

    public void updateElementAt(DatabaseElement element, int index)
    {
        _elements[index] = element;
    }

    public virtual int getFreeId()
    {
        return _elements.Count;
    }



}

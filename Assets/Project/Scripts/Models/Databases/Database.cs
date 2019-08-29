using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[System.Serializable]
public class Database<T> : ScriptableObject where T : DatabaseElement
{
    [SerializeField] protected List<T> _elements = new List<T>();
    public List<T> elements { get => _elements; }

    private void OnEnable()
    {
        orderDB();
    }

    private void orderDB()
    {
        _elements = _elements.OrderBy(x => x.databaseID).ToList();
    }

    public T getElementAt(int index)
    {
        orderDB();
        if (index >= _elements.Count)
            return default(T);
        else
            return _elements[index];
    }

    public T getRandomElement()
    {
        return _elements[Random.Range(0, _elements.Count)];
    }

    public int getDatabaseSize()
    {
        return _elements.Count;
    }

    public void addElement(T element)
    {
        _elements.Add(element);
    }

    public void removeElement(T element)
    {
        orderDB();
        _elements.Remove(element);
    }

    public void updateElementAt(T element, int index)
    {
        orderDB();
        _elements[index] = element;
    }

    public virtual int getFreeId()
    {
        orderDB();
        int lastID = -1;
        for (int i = 0; i < _elements.Count; i++)
        {
            int currentID = _elements[i].databaseID;
            if (lastID != -1 && currentID - lastID > 1)
                return lastID + 1;
            lastID = currentID;
        }

        return _elements.Count;
    }



}

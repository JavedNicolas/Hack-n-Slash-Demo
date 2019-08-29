using UnityEngine;
using System.Collections;

[System.Serializable]
[CreateAssetMenu(fileName = "ItemDatabase", menuName = "Database/Item")]
public class ItemDatabase : Database<Item>
{
    public override int getFreeId()
    {
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

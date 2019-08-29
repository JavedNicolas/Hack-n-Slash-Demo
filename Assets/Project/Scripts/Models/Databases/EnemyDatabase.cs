using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
[CreateAssetMenu(fileName = "EnemyDatabase", menuName = "Database/Enemies")]
public class EnemyDatabase : Database<Enemy>
{
    public override int getFreeId()
    {
        int lastID = -1;
        for (int i = 0; i < _elements.Count; i++)
        {
            int currentID = _elements[i].databaseID;
            if (lastID != -1 && currentID - lastID > 1)
                return lastID + 1;
        }

        return _elements.Count;
    }

}

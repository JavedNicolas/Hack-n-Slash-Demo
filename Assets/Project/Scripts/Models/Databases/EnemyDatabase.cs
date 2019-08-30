using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
[CreateAssetMenu(fileName = DatabaseConstant.enemyDatabaseFileName, menuName = "Database/Enemies")]
public class EnemyDatabase : Database<Enemy>
{

}

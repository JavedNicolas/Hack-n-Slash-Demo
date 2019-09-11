using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrentMapInstanceController
{
    #region singleton
    public static CurrentMapInstanceController instance = new CurrentMapInstanceController();

    private CurrentMapInstanceController() {

    }
    #endregion

    List<GameObject> enemies = new List<GameObject>();

    public void loadZone()
    {
        spawnEnemies();
        
    }

    void spawnEnemies()
    {
        int numberOfEnemies = Random.Range(30, 100);

        for (int i = 0; i < numberOfEnemies; i++)
        {
            Enemy enemy = GameManager.instance.enemyDatabase.getRandomElement();
            GameObject enemyGO = SpawningController.instance.spawnEnemy(enemy);
            enemies.Add(enemyGO);
        }
    }
}

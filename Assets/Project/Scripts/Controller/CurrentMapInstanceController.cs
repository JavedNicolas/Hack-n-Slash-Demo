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

    List<Enemy> enemies = new List<Enemy>();

    public void generateEnemies()
    {
        int numberOfEnemies = Random.Range(1, 10);

        for (int i = 0; i < numberOfEnemies; i++)
        {
            enemies.Add(GameManager.instance.getEnemyDatabase().getRandomElement());
        }

        GameManager.instance.spawnBeings(enemies);
    }
}

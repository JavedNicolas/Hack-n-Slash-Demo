using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrentMapInstance
{
    GameObject _playerGameObject;
    List<GameObject> _enemies = new List<GameObject>();

    public GameObject playerGameObject => _playerGameObject;
    public List<GameObject> enemies => _enemies;

    public void loadInstance(Spawner spawningController, Player player)
    {
        spawnEnemies(spawningController);
        spawnPlayer(spawningController, player);
        loadUI(player);
    }

    public void loadUI(Player player)
    {
        GameUI.instance.loadUI(player);
    }

    void spawnEnemies(Spawner spawningController)
    {
        int numberOfEnemies = Random.Range(10, 20);

        for (int i = 0; i < numberOfEnemies; i++)
        {
            Enemy enemy = GameManager.instance.enemyDatabase.getRandomElement().databaseModelToEnemy(GameManager.instance.resourcesList, GameManager.instance.itemDatabase);
            GameObject enemyGO = spawningController.spawnEnemy(enemy);
            _enemies.Add(enemyGO);
        }
    }

    /// <summary>
    /// Spawn the player and return it's GameObject
    /// </summary>
    /// <returns></returns>
    public void spawnPlayer(Spawner spawningController, Player player)
    {
        _playerGameObject = spawningController.spawnPlayer(player);
    }

}

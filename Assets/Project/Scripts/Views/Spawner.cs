using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public Transform beingParentObject;

    public GameObject spawnEnemy(Enemy enemy)
    {
        Enemy newEnemy = new Enemy(enemy);
        GameObject enemyGO = Instantiate(newEnemy.model);
        enemyGO.GetComponent<EnemyBehavior>().being = newEnemy;
        enemyGO.transform.position = new Vector3(Random.Range(-90, -20), 2, 50);
        enemyGO.transform.SetParent(beingParentObject);
        return enemyGO;
    }

    public GameObject spawnPlayer(Player player)
    {
        GameObject playerGO = Instantiate(player.model);
        playerGO.GetComponentInChildren<PlayerBehavior>().being = player;
        playerGO.transform.position = new Vector3(-87, 2, 8);
        playerGO.transform.SetParent(beingParentObject);
        return playerGO;
    }
}

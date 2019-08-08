using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawningController : MonoBehaviour
{
    #region singleton
    public static SpawningController instance;
    #endregion

    public Transform beingParentObject;

    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(this);
    }

    public GameObject spawnEnemy(Enemy enemy)
    {
        Enemy newEnemy = new Enemy(enemy);
        GameObject enemyGO = Instantiate(newEnemy.prefab);
        enemyGO.GetComponent<EnemyBehavior>().being = newEnemy;
        enemyGO.transform.position = new Vector3(Random.Range(-90, -20), 2, 50);
        enemyGO.transform.SetParent(beingParentObject);
        return enemyGO;
    }

    public GameObject spawnPlayer(Player player)
    {
        GameObject playerGO = Instantiate(player.prefab);
        playerGO.GetComponentInChildren<PlayerBehavior>().being = player;
        playerGO.transform.position = new Vector3(-87, 2, 8);
        playerGO.transform.SetParent(beingParentObject);
        return playerGO;
    }
}

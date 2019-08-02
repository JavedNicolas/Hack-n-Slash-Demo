using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawningController : MonoBehaviour
{
    #region singleton
    public static SpawningController instance;
    #endregion

    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(this);
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void spawnEnemy(Enemy enemy)
    {
        GameObject enemyGO = Instantiate(enemy.prefab);
        enemyGO.GetComponent<EnemyController>().enemy = enemy;
        enemyGO.transform.position = new Vector3(Random.Range(-90, -20), 2, 50);
    }

    public void spawnPlayer(Player player)
    {
        GameObject playerGO = Instantiate(player.prefab);
        playerGO.GetComponentInChildren<PlayerController>().player = player;
        playerGO.transform.position = new Vector3(-87, 2, 8);
    }
}

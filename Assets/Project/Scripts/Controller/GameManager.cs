using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Database")]
    [SerializeField] EnemyDatabase enemyDatabase;

    public static GameManager instance;
    public GameObject playerPrefab;

    // Players & Enemies
    Player player = new Player(100, 100, 0, 10, 10, 0, new List<Skill>(), 150);


    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(this);
        player.prefab = playerPrefab;
        player.skills.Add(new BasicAttack(50));
    }

    // Start is called before the first frame update
    void Start()
    {
        CurrentMapInstanceController.instance.generateEnemies();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void spawnBeings(List<Enemy> enemies)
    {
        for (int i = 0; i < enemies.Count; i++)
            SpawningController.instance.spawnEnemy(enemies[i]);

        SpawningController.instance.spawnPlayer(player);
    }

    // Getter
    public EnemyDatabase getEnemyDatabase() { return enemyDatabase; }
}

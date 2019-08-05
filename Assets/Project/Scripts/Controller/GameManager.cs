using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Database")]
    [SerializeField] EnemyDatabase enemyDatabase;

    public static GameManager instance;
    GameObject player;

    // TMP------------------
    public GameObject playerPrefab;
    public GameObject projectilePrefab;
    Player playerTMp;


    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(this);
        playerTMp = new Player("Test", 100, 100, 0, 10, 10, 0, new List<Skill>(), 50, playerPrefab, 0);
        playerTMp.skills.Add(new BasicAttack(10));
        playerTMp.skills.Add(new LightningBall(50, projectilePrefab));
    }

    // Start is called before the first frame update
    void Start()
    {
        CurrentMapInstanceController.instance.loadZone();
        spawnPlayer();
        GameUI.instance.displayUI(player.GetComponentInChildren<PlayerBehavior>().being);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void spawnPlayer()
    {
        player = SpawningController.instance.spawnPlayer(playerTMp);
    }

    // Getter
    public EnemyDatabase getEnemyDatabase() { return enemyDatabase; }
    public Being getPlayer() { return player.GetComponentInChildren<PlayerBehavior>().being; }
}

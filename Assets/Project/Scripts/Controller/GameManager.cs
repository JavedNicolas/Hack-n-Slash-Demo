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
    Player playerTMp = new Player(100, 100, 0, 10, 10, 0, new List<Skill>(), 150);


    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(this);
        playerTMp.prefab = playerPrefab;
        playerTMp.skills.Add(new BasicAttack(50));
    }

    // Start is called before the first frame update
    void Start()
    {
        CurrentMapInstanceController.instance.loadZone();
        spawnPlayer();
        GameUI.instance.displayUI(player.GetComponentInChildren<PlayerController>().player);
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
    public Player getPlayer() { return player.GetComponentInChildren<PlayerController>().player; }
}

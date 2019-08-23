using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Database")]
    [SerializeField] EnemyDatabase enemyDatabase;
    [SerializeField] AbilityDatabase skillDatabase;
    [SerializeField] ItemDatabase itemDatabase;

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
        playerTMp = new Player("Player Test", 100, 100, 0, 10, 11, 0, new List<Ability>(), 50, playerPrefab, 100);
        playerTMp.skills.Add(skillDatabase.getElementAt(1));
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
    public Player getPlayer() { return (Player)player.GetComponentInChildren<PlayerBehavior>().being; }

    public Inventory getPlayerInventory() { return getPlayer().inventory; }
}

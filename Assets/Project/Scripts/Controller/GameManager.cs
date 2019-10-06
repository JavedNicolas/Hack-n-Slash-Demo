using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("Database")]
    [SerializeField] EnemyDatabase _enemyDatabase;
    public EnemyDatabase enemyDatabase { get => _enemyDatabase; set => _enemyDatabase = value; }
    [SerializeField] AbilityDatabase _abilityDatabase;
    public AbilityDatabase abilityDatabase { get => _abilityDatabase; set => _abilityDatabase = value; }
    [SerializeField] ItemDatabase _itemDatabase;
    public ItemDatabase itemDatabase { get => _itemDatabase; set => _itemDatabase = value; }
    [SerializeField] ResourcesList _resourcesList;
    public ResourcesList resourcesList { get => _resourcesList; set => _resourcesList = value; }

    [SerializeField] GameObject _itemObject;
    public GameObject itemObjetPrefab { get => _itemObject; }

    public static GameManager instance;
    GameObject player;

    // TMP------------------
    public GameObject playerPrefab;
    public GameObject projectilePrefab;
    Player playerTMp;

    #region clicks
    bool _canLeftClick = true;
    public bool canLeftClick { get => _canLeftClick; }

    bool _canRightClick = true;
    public bool canRightClick { get => _canRightClick; }

    public delegate void LeftClickDelegate(bool overInterface);
    public LeftClickDelegate leftClickDelegate;

    public delegate void RightClickDelegate(bool overInterface);
    public RightClickDelegate rightClickDelegate;
    #endregion

    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(this);
    }

    // Start is called before the first frame update
    void Start()
    {
        loadDatabases();

        playerTMp = new Player("Player Test", 200, 100, 1, 1, 25, new List<int>() { 0, 1, 2 }, playerPrefab, 1);
        playerTMp.stats.addStat(new Stat(StatType.Life, StatBonusType.additional, 20, "Test2"));
        playerTMp.stats.addStat(new Stat(StatType.Life, StatBonusType.Multiplied, 20, "Test2"));
        playerTMp.stats.addStat(new Stat(StatType.AreaSize, StatBonusType.additional, 50, "Test2", StatInfluencedBy.Level, 1));
        playerTMp.stats.addStat(new Stat(StatType.CastSpeed, StatBonusType.Pure, 1, "Test2"));
        playerTMp.allocatedNodesInfo.Add(new PlayerAllocatedNode("cb8a97f6-996f-48c4-a257-b2b3b0e2f37a", 1));
        playerTMp.allocatedNodesInfo.Add(new PlayerAllocatedNode("675c6d39-d802-4419-90d3-0bc445086335", 2));
        playerTMp.allocatedNodesInfo.Add(new PlayerAllocatedNode("03461af9-e2b6-4700-b160-2168b7798e16", 1));

        CurrentMapInstanceController.instance.loadZone();
        spawnPlayer();
        GameUI.instance.loadUI();
    }

    // Update is called once per frame
    void Update()
    {
        getMouseClick();
    }

    public void loadDatabases()
    {
        _enemyDatabase?.loadDB();
        _abilityDatabase?.loadDB();
        _itemDatabase?.loadDB();
    }

    /// <summary>
    /// Get mouse click and fire the delegate
    /// </summary>
    public void getMouseClick()
    {
        bool overInterface;
        if (Input.GetButton(InputConstant.leftMouseButtonName))
        {
            overInterface = EventSystem.current.IsPointerOverGameObject();
            leftClickDelegate(overInterface);
        }

        if (Input.GetButtonDown(InputConstant.rightMouseButtonName))
        {
            overInterface = EventSystem.current.IsPointerOverGameObject();
            rightClickDelegate(overInterface);
        }
    }

    /// <summary>
    /// Block the click possibility
    /// </summary>
    /// <param name="lockClick"></param>
    public void lockClick(bool lockClick, bool leftClick)
    {
        if (leftClick)
            _canLeftClick = !lockClick;
        else if (!leftClick)
            _canRightClick = !lockClick;
    }

    public void spawnPlayer()
    {
        player = SpawningController.instance.spawnPlayer(playerTMp);
    }

    // Getter
    public Player getPlayer() {
        if(GetPlayerBehavior() != null)
            return (Player)player.GetComponentInChildren<PlayerBehavior>().being;

        return null;
    }
    public PlayerBehavior GetPlayerBehavior() { return player.GetComponentInChildren<PlayerBehavior>(); }
    public Inventory getPlayerInventory() { return getPlayer().inventory; }
}

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(Spawner))]
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

    [Header("Player options")]
    [SerializeField] PlayerOptions _playerOptions;

    public static GameManager instance;

    CurrentMapInstance _currentMapInstanceController;
    Spawner _spawningController;

    // TMP------------------
    [SerializeField] GameObject _itemObject;
    public GameObject itemObjetPrefab { get => _itemObject; }

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
        _currentMapInstanceController = new CurrentMapInstance();
        _spawningController = GetComponent<Spawner>();
        loadPlayerOptions();
        loadDatabases();
        initplayer();
        _currentMapInstanceController.loadInstance(_spawningController, playerTMp);
    }

    // Update is called once per frame
    void Update()
    {
        getMouseClick();
    }

    void initplayer()
    {
        playerTMp = new Player("Player Test", 200, 100, 1, 1, 25, new List<int>() { 0, 1, 2 }, playerPrefab, 1);
        playerTMp.stats.addStat(new Stat(StatType.Life, StatBonusType.additional, 20, "Test2"));
        playerTMp.stats.addStat(new Stat(StatType.Life, StatBonusType.Multiplied, 20, "Test2"));
        playerTMp.stats.addStat(new Stat(StatType.AreaSize, StatBonusType.additional, 50, "Test2", StatInfluencedBy.Level, 1));
        playerTMp.stats.addStat(new Stat(StatType.CastSpeed, StatBonusType.Pure, 1, "Test2"));
    }

    void loadPlayerOptions()
    {
        LocalizationManager.instance.changeLang(_playerOptions.lang);
    }

    /// <summary>
    /// load database from jsons
    /// </summary>
    public void loadDatabases()
    {
        _enemyDatabase?.loadDB();
        _abilityDatabase?.loadDB();
        _itemDatabase?.loadDB();
    }

    /// <summary>
    /// Get mouse click and fire the delegate
    /// </summary>
    void getMouseClick()
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

    // Getter
    public Player getPlayer() {
        return GetPlayerBehavior().being;
    }

    public PlayerBehavior GetPlayerBehavior() { return _currentMapInstanceController.playerGameObject.GetComponentInChildren<PlayerBehavior>(); }
    public Inventory getPlayerInventory() { return getPlayer().inventory; }
}

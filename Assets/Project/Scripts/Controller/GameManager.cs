using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{
    [Header("Database")]
    [SerializeField] EnemyDatabase _enemyDatabase;
    public EnemyDatabase enemyDatabase { get => _enemyDatabase; }
    [SerializeField] AbilityDatabase _abilityDatabase;
    public AbilityDatabase abilityDatabase { get => _abilityDatabase; }
    [SerializeField] ItemDatabase _itemDatabase;
    public ItemDatabase itemDatabase { get => _itemDatabase; }

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
        playerTMp = new Player("Player Test", 100, 100, 1, 1, 10, new List<Ability>(), playerPrefab);
        playerTMp.abilities.Add(new LightningBall((LightningBall)GameManager.instance.getAbilityOfType(typeof(LightningBall))));
        playerTMp.addStat(new Stat(StatType.Life, StatBonusType.additional, 20, "Test2"));
        playerTMp.addStat(new Stat(StatType.Life, StatBonusType.Multiplied, 20, "Test2"));
        playerTMp.addStat(new Stat(StatType.NumberOfProjectile, StatBonusType.Pure, 1, "Test2", StatInfluencedBy.Level, 2));
    }

    // Start is called before the first frame update
    void Start()
    {
        CurrentMapInstanceController.instance.loadZone();
        spawnPlayer();
    }

    // Update is called once per frame
    void Update()
    {
        getMouseClick();
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
    public Ability getAbilityOfType(Type abilityType) { return _abilityDatabase.getAbilityOfType(abilityType); }
}

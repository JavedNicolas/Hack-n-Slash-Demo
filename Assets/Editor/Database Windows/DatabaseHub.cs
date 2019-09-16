using UnityEngine;
using UnityEditor;

public class DatabaseHub : EditorWindow
{
    // windows 
    protected static DatabaseHub hub;
    static EnemyDatabaseWindow enemyDatabaseWindow;
    static AbilityDatabaseWindow abilityDatabaseWindow;
    static ItemDatabaseWindow itemDatabaseWindow;
    static int tab = 0;

    [MenuItem("Database/Database Hub")]
    static void init()
    {
        hub = new DatabaseHub();
        enemyDatabaseWindow = new EnemyDatabaseWindow();
        enemyDatabaseWindow.initDB();
        abilityDatabaseWindow = new AbilityDatabaseWindow();
        abilityDatabaseWindow.initDB();
        itemDatabaseWindow = new ItemDatabaseWindow();
        itemDatabaseWindow.initDB();
        hub.minSize = new Vector2(300 * 3, 300);
        hub.Show();
    }

    #region database menu items
    [MenuItem("Database/Ability  Database")]
    static void showAbilityDB()
    {
        init();
        tab = 0;
    }

    [MenuItem("Database/Enemy Database")]
    static void showEnemyDB()
    {
        init();
        tab = 1;
    }

    [MenuItem("Database/Item  Database")]
    static void showItemDB()
    {
        init();
        tab = 2;
    }
    #endregion

    private void OnGUI()
    {
        tab = GUILayout.Toolbar(tab, new string[] { "Abilty DB", "Enemy DB", "Item DB" });
        EditorGUILayout.Space();
        switch (tab)
        {
            case 0: abilityDatabaseWindow.displayDB(); break;
            case 1: enemyDatabaseWindow.displayDB(); break;
            case 2: itemDatabaseWindow.displayDB(); break;
        }
    }

}
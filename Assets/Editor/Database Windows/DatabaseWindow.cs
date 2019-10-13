using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public abstract class DatabaseWindows<T> : EditorWindow where T : DatabaseElement
{
    // windows 
    protected static DatabaseWindows<T> windows;

    // database
    protected abstract string databasePath { get; }

    protected ResourcesList resourcesList;

    // scroll position
    Vector2 scrollPos;

    protected Database<T> database;
    protected float contentListWidth = 300;

    protected T element;
    protected int databaseIndex = -1;
    protected int databaseID = -1;

    protected GUIStyle centerTitle;
    protected GUIStyle leftTitle;

    /// <summary>
    /// Display windows
    /// </summary>
    public virtual void initDB(ResourcesList resourcesList)
    {
        this.resourcesList = resourcesList;
        database = Resources.Load<Database<T>>(databasePath);
        database.loadDB();
        databaseID = database.getFreeId();
    }

    public void displayDB()
    {
        centerTitle = new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter, fontStyle = FontStyle.Bold };
        leftTitle = new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleLeft, fontStyle = FontStyle.Bold };
        EditorGUILayout.BeginHorizontal();
        displayDatabaseContent();
        displayForm();
        EditorGUILayout.EndHorizontal();
    }

    protected abstract void displayForm();

    #region Left panel (The content list)
    void displayDatabaseContent()
    {
        EditorGUILayout.BeginVertical("Box", GUILayout.Width(contentListWidth));
        if (GUILayout.Button("Save Change made to DB"))
        {
            saveDB();
        }

        EditorGUILayout.Space();

        scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUILayout.Width(contentListWidth));
        for (int i = 0; i < database.getDatabaseSize(); i++)
        {
            T element = database.getElementAt(i);
            EditorGUILayout.BeginHorizontal("Box");
            EditorGUILayout.LabelField(getNameAtIndex(i));
            displayContentListButtons(i);

            EditorGUILayout.EndHorizontal();
        }

        EditorGUILayout.EndScrollView();
        EditorGUILayout.EndVertical();
    }

    protected virtual void displayContentListButtons(int i)
    {

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Edit"))
        {
            element = database.getElementAt(i);
            setFieldWithElementValues();
            databaseIndex = i;
        }
        if (GUILayout.Button("X"))
        {
            if (EditorUtility.DisplayDialog("Are you sure ?", "Do you want to delete " + database.elements[i].getName()+ " from the ressources list ?", "Yes", "No"))
            {
                database.removeElement(database.getElementAt(i));
                clearForm();
            }
        }
        EditorGUILayout.EndHorizontal();
    }
    #endregion

    #region rightPanel (Form)
    protected virtual void displayFormButtons()
    {
        EditorGUILayout.BeginHorizontal();
        if (databaseIndex != -1)
        {
            if (GUILayout.Button("Update"))
            {
                updateElementWithFormValues();
                database.updateElementAt(element, databaseIndex);
                clearForm();
            }
        }
        else
        {
            if (GUILayout.Button("Add"))
            {
                updateElementWithFormValues();
                database.addElement(element);
                clearForm();
            }
        }

        if (GUILayout.Button("Cancel"))
        {
            clearForm();
        }
        EditorGUILayout.EndHorizontal();
    }

    #endregion

    #region element setter
    protected abstract void setFieldWithElementValues();

    protected abstract void updateElementWithFormValues();

    protected virtual void clearForm()
    {
        EditorGUI.FocusTextInControl("");
        databaseIndex = -1;
        databaseID = database.getFreeId();
    }

    #endregion

    /// <summary>
    /// return the name of the element at index
    /// </summary>
    /// <param name="index">the index of the element</param>
    /// <returns>The name</returns>
    protected virtual string getNameAtIndex(int index)
    {
        return database.elements[index].getName();
    }

    protected void saveDB()
    {
        database.saveDB();
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
}
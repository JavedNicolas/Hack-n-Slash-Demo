using UnityEngine;
using UnityEditor;
using System;

public abstract class DatabaseWindows<DatabaseElement> : EditorWindow
{
    // windows 
    protected static DatabaseWindows<DatabaseElement> windows;

    // database
    protected abstract string databasePath { get; }

    // scroll position
    Vector2 scrollPos;

    protected Database<DatabaseElement> database;
    protected float contentListWidth = 300;

    protected DatabaseElement element;
    protected int databaseIndex = -1;

    /// <summary>
    /// Display windows
    /// </summary>
    public void showWindows()
    {
        this.Show();
        this.minSize = new Vector2(contentListWidth * 3, contentListWidth);
        database = Resources.Load<Database<DatabaseElement>>(databasePath);
    }

    void OnGUI()
    {
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
        scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUILayout.Width(contentListWidth));
        for (int i = 0; i < database.getDatabaseSize(); i++)
        {
            DatabaseElement element = database.getElementAt(i);
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
        if (GUILayout.Button("E"))
        {
            element = database.getElementAt(i);
            setFieldWithElementValues();
            databaseIndex = i;
        }
        if (GUILayout.Button("X"))
        {
            database.removeElement(database.getElementAt(i));
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
        EditorUtility.SetDirty(database);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    #endregion

    /// <summary>
    /// return the name of the element at index
    /// </summary>
    /// <param name="index">the index of the element</param>
    /// <returns>The name</returns>
    protected abstract string getNameAtIndex(int index);
}
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

public class AbilityDatabaseWindow : DatabaseWindows<Ability>
{
    // database
    protected override string databasePath { get => "Databases/AbilityDatabase"; }

    AbilityAttributs attributs;

    #region Left panel (The content list)
    protected override void displayContentListButtons(int i)
    {

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Edit"))
        {
            element = database.getElementAt(i);
            setFieldWithElementValues();
            databaseIndex = i;
        }
        EditorGUILayout.EndHorizontal();
    }
    #endregion

    #region Right panel (The form)
    protected override void displayForm()
    {
        EditorGUILayout.BeginVertical();
        if(element != null)
        {

            GUI.enabled = false;
            EditorGUILayout.IntField("Database ID : ", databaseID);
            GUI.enabled = true;
            attributs = (AbilityAttributs)EditorGUILayout.ObjectField("Attributs : ", attributs, typeof(AbilityAttributs), false);

            displayFormButtons();
        }
        EditorGUILayout.EndVertical();
    }

    protected override void displayFormButtons()
    {
        EditorGUILayout.BeginHorizontal();
        if(element != null)
        {
            if (GUILayout.Button("Update"))
            {
                updateElementWithFormValues();
                database.updateElementAt(element, databaseIndex);
                clearForm();
            }
            if (GUILayout.Button("Cancel"))
            {
                clearForm();
            }
        }
        EditorGUILayout.EndHorizontal();
    }

    protected override void setFieldWithElementValues()
    {
        databaseID = element.databaseID;
        attributs = element.abilityAttributs;
    }

    protected override void updateElementWithFormValues()
    {
        element.abilityAttributs = attributs;
    }

    #endregion

    /// <summary>
    /// return the name of the element at index
    /// </summary>
    /// <param name="index">the index of the element</param>
    /// <returns>The name</returns>
    protected override string getNameAtIndex(int index)
    {
        return database.elements[index].getName();
    }

    protected override void clearForm()
    {
        EditorGUI.FocusTextInControl("");
        databaseIndex = -1;
        databaseID = database.getFreeId();
        element = null;
    }

}
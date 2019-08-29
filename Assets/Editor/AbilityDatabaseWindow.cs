using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

public class AbilityDatabaseWindow : DatabaseWindows<Ability>
{
    // database
    protected override string databasePath { get => "Databases/AbilityDatabase"; }

    // item Field
    AbilityAttributs attributs;

    [MenuItem("Database/Ability")]
    public static void databadeMenuFunction()
    {
        windows = new AbilityDatabaseWindow();
        windows.showWindows();
    }

    #region Right panel (The form)
    protected override void displayForm()
    {
        if (databaseIndex != -1)
        {
            EditorGUILayout.BeginVertical();

            attributs = (AbilityAttributs)EditorGUILayout.ObjectField("Model : ", attributs, typeof(AbilityAttributs), false);

            displayFormButtons();
            EditorGUILayout.EndVertical();
        }
    }

    protected override void displayFormButtons()
    {
        EditorGUILayout.BeginHorizontal();

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

        EditorGUILayout.EndHorizontal();
    }

    protected override void displayContentListButtons(int i)
    {

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("E"))
        {
            element = database.getElementAt(i);
            databaseIndex = i;
            setFieldWithElementValues();
        }
        EditorGUILayout.EndHorizontal();
    }

    #endregion

    protected override void setFieldWithElementValues()
    {
        attributs = database.getElementAt(databaseIndex).abilityAttributs;
    }

    protected override void updateElementWithFormValues()
    {
        ((AbilityDatabase)database).updateAbilityAttribut(attributs, databaseIndex);
    }

    protected override void clearForm()
    {
        base.clearForm();
        attributs = null;
    }

    protected override string getNameAtIndex(int index)
    {
        Ability ability = database.getElementAt(index);
        if (ability != null )
            return ability.GetType().ToString();

        return "";
    }
}
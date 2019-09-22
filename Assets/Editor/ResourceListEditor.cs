using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(DatabaseResourcesList))]
public class ResourceListEditor : Editor
{
    DatabaseResourcesList resourcesList;
    bool canEdit;
    string searchString = "";
    SearchField searchField;

    enum SearchField
    {
        Name, ID
    }

    private void OnEnable()
    {
        resourcesList = (DatabaseResourcesList)target;
        EditorUtility.SetDirty(resourcesList);
    }

    public override void OnInspectorGUI()
    {
        EditorGUILayout.LabelField("Search : ");
        EditorGUILayout.BeginHorizontal();
        searchString =  EditorGUILayout.TextField(searchString);
        searchField = (SearchField)EditorGUILayout.EnumPopup(searchField);
        EditorGUILayout.EndHorizontal();
        if (GUILayout.Button(canEdit ? "Stop Editing" : "Enable Editing"))
            canEdit = !canEdit;


        GUIStyle centerTitle = new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter, fontStyle = FontStyle.Bold };
        for (int i =0; i < resourcesList.objects.Count; i++)
        {
            if (checkSearchField(resourcesList.objects[i]))
            {
                EditorGUILayout.BeginHorizontal("Box");
                EditorGUILayout.LabelField("Object n° " + (i + 1)+ " : ", centerTitle);
                GUI.enabled = canEdit;
                objectButton(resourcesList.objects[i]);
                EditorGUILayout.EndHorizontal();

                resourcesList.objects[i].obj = EditorGUILayout.ObjectField("Object : ", resourcesList.objects[i].obj, typeof(Object), true);
                resourcesList.objects[i].id = EditorGUILayout.TextField("id : ", resourcesList.objects[i].id);
                GUI.enabled = true;
            }
        }
    }

    bool checkSearchField(ObjectAndID objectAndID)
    {
        if (searchString != "")
        {
            
            switch (searchField)
            {
                case SearchField.ID:
                    if (objectAndID.id.ToLower().Contains(searchString.ToLower()))
                        return true;
                    else
                        return false;
                case SearchField.Name:
                    if (objectAndID.obj.name.ToLower().Contains(searchString.ToLower()))
                        return true;
                    else
                        return false;
                default: return false;
            }
            
        }
        return true;
    }

    void objectButton(ObjectAndID objectAndID)
    {
        if (GUILayout.Button("X", GUILayout.Width(30)))
        {
            if(EditorUtility.DisplayDialog("Are you sure ?", "Do you want to delete " + objectAndID.obj.name + " from the ressources list ?", "Yes", "No"))
            {
                resourcesList.objects.Remove(objectAndID);
            }
        }
            
    }
}
using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class ObjectAndID
{
    [SerializeField] public Object obj;
    [SerializeField] public string id;

    public ObjectAndID(Object obj, string id)
    {
        this.obj = obj;
        this.id = id;
    }
}

[CreateAssetMenu(menuName = "test/test", fileName ="test")]
public class DatabaseResourcesList : ScriptableObject
{
    [SerializeField] public List<ObjectAndID> objects = new List<ObjectAndID>();

    public string addObjects(Object obj)
    {
        if (!objects.Exists(x => x.obj == obj))
        { 
            objects.Add(new ObjectAndID(obj, generateID()));
        }

        return objects.Find(x => x.obj == obj).id;
    }

    public Object getObject<T>(string GUID) where T : Object
    {
        ObjectAndID objectAndId = objects.Find(x => x.id == GUID);
        if (objectAndId == null || !(objectAndId.obj is T))
            return null;

        return (T)objectAndId.obj;
    }

    string generateID()
    {
        System.Guid guid = new System.Guid();
        guid = System.Guid.NewGuid();
        return guid.ToString();
    }
}
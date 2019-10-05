using UnityEngine;
using System.Collections;

public static class TransfromExtension {
    /// <summary>
    /// Clear child to avoir duplicate
    /// </summary>
    public static void clearChild(this Transform transform)
    {
        if (Application.isPlaying)
            foreach (Transform child in transform)
            {
                GameObject.Destroy(child.gameObject);
            }

        if (!Application.isPlaying)
        {
            var tempArray = new GameObject[transform.childCount];

            for (int i = 0; i < tempArray.Length; i++)
            {
                tempArray[i] = transform.GetChild(i).gameObject;
            }

            foreach (var child in tempArray)
            {
                GameObject.DestroyImmediate(child);
            }
        }
    }

}

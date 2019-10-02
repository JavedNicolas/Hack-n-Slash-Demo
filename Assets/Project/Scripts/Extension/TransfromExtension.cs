using UnityEngine;
using System.Collections;

public static class TransfromExtension {
    /// <summary>
    /// Clear child to avoir duplicate
    /// </summary>
    public static void clearChild(this Transform transform)
    {
        foreach (Transform child in transform)
        {
            GameObject.Destroy(child.gameObject);
        }
    }

}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public static class ListExtension 
{
    public static void removeLast<T>(this List<T> list)
    {
        list.RemoveAt(list.Count - 1);
    }

    public static void addEmptyElement<T>(this List<T> list)
    {
        list.Add((T)Activator.CreateInstance(typeof(T)));
    }

    public static void updateSize<T>(this List<T> list, int size)
    {
        while (list.Count != size)
        {
            if (list.Count < size)
            {
                list.addEmptyElement();
            }
            else if (list.Count > size)
            {
                list.removeLast();
            }
        }
    }
}

using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;
using System.Linq;

public static class ExtensionMethods
{
    public static Vector2 AsVector2(this Vector3 v)
    {
        return new Vector2(v.x, v.y);
    }

    public static List<T> Clone<T>(this List<T> listToClone) where T : ICloneable
    {
        return listToClone.Select(item => (T)item.Clone()).ToList();
    }
}
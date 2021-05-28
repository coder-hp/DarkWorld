using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager
{
    public static List<GameObject> list = new List<GameObject>();

    public static void addEnemy(GameObject obj)
    {
        list.Add(obj);
    }

    public static void removeEnemy(GameObject obj)
    {
        list.Remove(obj);
    }
}

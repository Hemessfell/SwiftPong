using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameObjectExtension
{
    public static GameObject InstantiateEmptyGameObject(Vector3 position, Quaternion rotation)
    {
        GameObject temp = new GameObject();
        temp.transform.position = position;
        temp.transform.rotation = rotation;
        return temp;
    }

    public static GameObject InstantiateEmptyGameObject(Vector3 position, Quaternion rotation, Transform parent)
    {
        GameObject temp = new GameObject();
        temp.transform.position = position;
        temp.transform.rotation = rotation;
        temp.transform.SetParent(parent);
        return temp;
    }

    public static GameObject InstantiateEmptyGameObject(Vector3 position, Quaternion rotation, Transform parent, System.Type components)
    {
        GameObject temp = new GameObject("", components);
        temp.transform.position = position;
        temp.transform.rotation = rotation;
        temp.transform.SetParent(parent);
        return temp;
    }
}

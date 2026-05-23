using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class Collision
{
    private float _range;
    public float Range => _range;
    private static List<GameObject> CollisionObjects = new();
    private static List<GameObject> CollisionEnableObjects = new();
    public ReactiveProperty<bool> IsHit = new(false);
    public void AddCollisionObject(GameObject obj, float range = 0.5f)
    {
        CollisionObjects.Add(obj);
        _range = range;

        Debug.Log(CollisionObjects.Count);
    }

    public void EnableCollisionObject(GameObject obj)
    {
        CollisionEnableObjects.Add(obj);
    }

    public void DisableCollisionObject(GameObject obj)
    {
        CollisionEnableObjects.Remove(obj);
    }

    public void Check()
    {
        foreach(var obj in CollisionEnableObjects)
        {
            Transform transform = obj.transform;
            Vector3 pos = transform.position;
            for(int i = 0; i < CollisionEnableObjects.Count; i++)
            {
                if(CollisionEnableObjects[i] == obj || !CollisionEnableObjects[i]) continue;

                Vector3 enemyPos = CollisionEnableObjects[i].transform.position;
                Debug.Log(enemyPos);

            Debug.Log($"vector:{Vector2.Distance(pos, enemyPos)}");


                if(Vector2.Distance(pos, enemyPos) > _range * 2) continue;

                OnHit();
            }
        }
    }

    private void OnHit()
    {
        IsHit.Value = true;
        Debug.Log("Hit!");
    }

    public void Debugaaa()
    {
        Debug.Log("aaa");
    }
    
}

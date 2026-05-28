using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class EnemyHitCollision
{
    private static List<PooledObject> CollisionObjects = new();
    private static List<PooledObject> CollisionEnableObjects = new();
    private bool isHit = false;
    public Action<bool> action;
    public void AddCollisionObject(PooledObject obj)
    {
        CollisionObjects.Add(obj);

        Debug.Log(CollisionObjects.Count);
    }

    public void EnableCollisionObject(PooledObject obj)
    {
        CollisionEnableObjects.Add(obj);
    }

    public void DisableCollisionObject(PooledObject obj)
    {
        CollisionEnableObjects.Remove(obj);
    }

    public void Check(Player obj, float range = 1f)
    {
        Transform transform = obj.transform;
        Vector3 pos = transform.position;

        for(int i = 0; i < CollisionEnableObjects.Count; i++)
        {
            PooledObject target = CollisionEnableObjects[i];

            if(target == null || target == obj) continue;

            if(obj.RoleType == target.RoleType) continue;

            Vector3 enemyPos = target.transform.position;

            float distance = Vector2.Distance(pos, enemyPos);

// Debug.Log($"distance:{distance}");

            if(distance < range)
            {
                OnHit(target as Enemy, obj.Attack);
                return;
            }
        }
    }

    private void OnHit(Enemy enemy, int attack)
    {
        isHit = true;
        Debug.Log("Hit!");

        enemy.HP.Value -= attack;

        action.Invoke(isHit);
    }
}

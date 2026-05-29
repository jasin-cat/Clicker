using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class EnemyHitCollision
{
    private static PooledObject EnableEnemy;
    private bool isHit = false;
    public Action<bool> action;
    public void EnableCollisionObject(PooledObject obj)
    {
        if(EnableEnemy is null)
            EnableEnemy = obj;
    }

    public void DisableCollisionObject(PooledObject obj)
    {
        if(EnableEnemy is not null)
        EnableEnemy = null;
    }

    public void Check(Player obj, float range = 1f)
    {
        if(obj is null) return;

        Transform transform = obj.transform;
        Vector3 pos = transform.position;

            PooledObject target = EnableEnemy;

            if(target == null || target == obj) return;

            if(obj.RoleType == target.RoleType) return;

            Vector3 enemyPos = target.transform.position;

            float distance = Vector2.Distance(pos, enemyPos);

// Debug.Log($"distance:{distance}");

            if(distance < range)
            {
                OnHit(target as Enemy, Player.AttackStatic);
                return;
            }
    }

    private void OnHit(Enemy enemy, int attack)
    {
        action.Invoke(isHit);
        if(isHit) return;
        isHit = true;
        Debug.Log("Hit!");

        enemy.HP.Value -= attack;
        isHit = false;
    }
}

using System;
using UniRx;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class Enemy : PooledObject
{
    private float _baseHp = 12;
    private EnemyHitCollision _collision;
    private EnemyGetHp _enemyGetHp;
    private IntReactiveProperty _hp = new(10);
    public IntReactiveProperty HP {get => _hp; set => _hp = value;}
    private IDisposable _hpDispose;
    public static int EnableEnamyStatic = 0;

    private Action _isDead;
    public Action IsDead{get => _isDead; set => _isDead = value;}

    private void OnEnable()
    {
        EnableEnamyStatic++;
    }

    public void Initialize(int level)
    {
        _roletype = RoleType.enemy;
        _collision = new EnemyHitCollision();
        _enemyGetHp = new EnemyGetHp();

        _collision.EnableCollisionObject(this);

        _hp.Value = _enemyGetHp.GetHp(_baseHp, level);


        _hpDispose = _hp
            .Where(x => x < 0)
            .Subscribe(x =>
            {
                Debug.Log("倒した");
                IsDead.Invoke();
                Release();         
            });
    }

    private void OnDisable()
    {
        EnableEnamyStatic--;

        IsDead = default;
        _hpDispose?.Dispose();
        _collision?.DisableCollisionObject(this);
    }
}

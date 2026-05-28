using UniRx;
using UnityEngine;

public class Player : PooledObject
{
    [SerializeField]
    private float _speed;
    private Transform _transform;
    private EnemyHitCollision _collision;
    public int baseAttack = 1;
    public int Attack;
    private void Awake()
    {
        _roletype = RoleType.player;
        _collision = new EnemyHitCollision();
    }

    private void OnEnable()
    {
        _transform = this.transform;
        Attack = 10;

        _collision.action += IsHitEnemy;
    }

    private void OnDisable()
    {
        _collision.action -= IsHitEnemy;
    }

    private void Update()
    {       
        _collision.Check(this);

        _transform.position = _transform.position + Vector3.right * _speed * Time.deltaTime;

        if(_transform.position.x > 8)
            Release();
    }

    private void IsHitEnemy(bool isHit)
    {
        Release();
    }
}

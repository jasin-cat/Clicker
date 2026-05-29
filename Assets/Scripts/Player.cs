using UniRx;
using UnityEngine;
using LitMotion;
using LitMotion.Extensions;
using System.Threading;
using TMPro;

public class Player : PooledObject
{
    private CancellationTokenSource _source = new();
    [SerializeField]
    private float _speed;
    private Transform _transform;
    private EnemyHitCollision _collision;
    public int baseAttack = 1;
    private Player _player;
    public static int AttackStatic = 10;
    public static int MaxEnablePlayer = 1;
    public static int EnablePlayerStatic = 0;

    private bool _isReleasing;
    private void Awake()
    {
        _roletype = RoleType.player;
        _collision = new EnemyHitCollision();
    }

    private void OnEnable()
    {
        EnablePlayerStatic++;
        _player = this;
        _transform = this.transform;

        _collision.action += IsHitEnemy;

        this.GetComponent<SpriteRenderer>().color = new Color(1,1,1,1);

        _isReleasing = false;

        Debug.Log($"Spawn {this.GetInstanceID()}");
    }

    private void OnDisable()
    {
        EnablePlayerStatic--;
        _player = null;
        _collision.action -= IsHitEnemy;
    }

    private void Update()
    {
        if(_isReleasing) return;

        _collision.Check(_player);

        _transform.position = _transform.position + Vector3.right * _speed * Time.deltaTime;

        if(_transform.position.x > 8)
            Release();
    }

    private void IsHitEnemy(bool isHit)
    {
        Release();
    }

    protected override void Release()
    {
        if(_isReleasing) return;

        _isReleasing = true;

        _player = null;
        
        LMotion.Create(1f, 0f, 0.2f)
            .WithOnComplete(() =>
            {
                base.Release();
            })
            .Bind(x =>
            {
                // var sprite = this.GetComponent<SpriteRenderer>();
                // Color color = sprite.color;

                // color.a = x;

                // sprite.color = color;
            })
            .ToUniTask(cancellationToken: _source.Token);
        
    }


}

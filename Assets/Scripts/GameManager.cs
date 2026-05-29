using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UniRx;
using System;
using Unity.VisualScripting;
using UnityEngine.Video;
using System.Collections;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private View _view;
    CancellationTokenSource source = new CancellationTokenSource();
    private SOGold _gold;
    private PoolManager _playerPoolPrefab;
    private PoolManager _playerPool;
    private PoolManager _enemyPoolPrefab;
    private PoolManager _enemyPool;
    private Click _click;
    private int _clickCount = 0;
    public int ClickCount => _clickCount;

    private int _level = 1;
    public int Level => _level;
    public static int UpGradeLevel = 1;
    private Coroutine coroutine;
    private bool _isDead = false;
    private async void Awake()
    {
        var poolHandle = await GetPoolMangers(source.Token);
        _enemyPoolPrefab = poolHandle.enemy;
        _playerPoolPrefab = poolHandle.player;

        InstacePoolManagers();

        _gold = await RegisterSOGold(source.Token);

        _click = new Click(this, _gold.AddGold, _view.Gold);

        EnemyEnable();

        _gold.Init();

        _view.Gold(gold:_gold.Gold);

        coroutine = StartCoroutine(AutoCreatePlayer());
    }

    // private void Start()
    // {
    //     StartCoroutine(AutoCreatePlayer());
    // }

    private IEnumerator AutoCreatePlayer()
    {
        while (true)
        {
            if(!_isDead) PlayerEnable();
            yield return new WaitForSeconds(1.0f);
        }
        
    }

/// <summary>
/// PoolMangerをAddressableで持ってくる
/// </summary>
/// <param name="enemy"></param>
/// <param name="token"></param>
/// <returns></returns>
    private async UniTask<(PoolManager enemy, PoolManager player)> GetPoolMangers(CancellationToken token)
    {
        var enemyPoolHandle = Addressables
                                .LoadAssetAsync<GameObject>("EnemyPool")
                                .ToUniTask(
                                    cancellationToken : token,
                                    autoReleaseWhenCanceled : true);

        var playerPoolHandle = Addressables
                                .LoadAssetAsync<GameObject>("PlayerPool")
                                .ToUniTask(
                                    cancellationToken : token,
                                    autoReleaseWhenCanceled : true);

        GameObject enemyPool = await enemyPoolHandle;
        GameObject playerPool = await playerPoolHandle;

        return (enemy : enemyPool.GetComponent<PoolManager>(), player : playerPool.GetComponent<PoolManager>());
    }

/// <summary>
/// 持ってきたpoolManagerをInstanceする
/// </summary>
    private void InstacePoolManagers()
    {
        if(_playerPool is null) _playerPool = Instantiate(_playerPoolPrefab);
        if(_enemyPool is null) _enemyPool = Instantiate(_enemyPoolPrefab);
    }

/// <summary>
/// SOGoldをAddressableで持ってくる
/// </summary>
    private async UniTask<SOGold> RegisterSOGold(CancellationToken token)
    {
        var handle = Addressables
                            .LoadAssetAsync<SOGold>("Gold")
                            .ToUniTask(
                                cancellationToken : token,
                                autoReleaseWhenCanceled : true
                            );

        SOGold gold = await handle;

        return gold;
    }


    private void PlayerEnable()
    {
        if(Player.EnablePlayerStatic < Player.MaxEnablePlayer)
        {
            PooledObject obj = _playerPool.GetPooledObject();
            obj.transform.position = Vector2.zero;
        }        
    }

    private void EnemyEnable()
    {
        _click.IsRevive();

        Enemy obj = _enemyPool.GetPooledObject() as Enemy;
        obj.gameObject.SetActive(true);
        obj.transform.position = new Vector2(5f, 0f);
        obj.Initialize(_level);
        obj.IsDead += GetGold;
        obj.IsDead += _click.IsDead;
        obj.IsDead += EnemyDead;
        obj.IsDead += ReviveEnemy;

        _view.Level(_level);
    }

    private void EnemyDead()
    {
        _level++;
        _isDead = true;
    }

    private void ReviveEnemy()
    {
        ReviveEnemyAsync().Forget();
    }

    private async UniTaskVoid ReviveEnemyAsync()
    {
        await PlayerEnemyAllDisable();

        await UniTask.Delay(TimeSpan.FromSeconds(2.0f));

        _isDead = false;

        EnemyEnable();
    }

    private async UniTask PlayerEnemyAllDisable()
    {
        await UniTask.WaitUntil(() => Enemy.EnableEnamyStatic == 0);
        await UniTask.WaitUntil(() => Player.EnablePlayerStatic == 0);
    }

    private void GetGold()
    {
        int cost = Mathf.FloorToInt(10f * Mathf.Pow(1.07f, _level));
        _gold.AddGold(i:cost);
        _view.Gold(gold:_gold.Gold);
    }

    void Update()
    {
        if(_gold is not null)
        _view.Gold(gold:_gold.Gold);
    }
}

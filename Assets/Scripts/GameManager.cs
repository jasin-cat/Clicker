using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UniRx;
using System;
using Unity.VisualScripting;

public class GameManager : MonoBehaviour
{
    CancellationTokenSource source = new CancellationTokenSource();
    private SOGold _gold;
    private PoolManager _playerPoolPrefab;
    private PoolManager _playerPool;
    private PoolManager _enemyPoolPrefab;
    private PoolManager _enemyPool;
    private Click _click;
    private int _clickCount = 0;
    public int ClickCount => _clickCount;

    private int _level = 2;
    private async void OnEnable()
    {
        var poolHandle = await GetPoolMangers(source.Token);
        _enemyPoolPrefab = poolHandle.enemy;
        _playerPoolPrefab = poolHandle.player;

        InstacePoolManagers();

        _gold = await RegisterSOGold(source.Token);

        _click = new Click(this, PlayerEnable);

        EnemyEnable();
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
        PooledObject obj = _playerPool.GetPooledObject();
        obj.transform.position = Vector2.zero;

        Debug.Log("生成！");
    }

    private Enemy EnemyEnable()
    {
        Enemy obj = _enemyPool.GetPooledObject() as Enemy;
        obj.gameObject.SetActive(true);
        obj.transform.position = new Vector2(5f, 0f);
        obj.Initialize(_level);
        obj.IsDead += GetGold;

        return obj;
    }

    private void GetGold()
    {
        int cost = Mathf.FloorToInt(10f * Mathf.Pow(1.07f, _level));
        _gold.AddGold(cost);
    }
}

using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class GameManager : MonoBehaviour
{
    private PoolManager _playerPoolPrefab;
    private PoolManager _playerPool;
    private PoolManager _enemyPoolPrefab;
    private PoolManager _enemyPool;
    private int _clickCount = 0;
    public int ClickCount => _clickCount;

    private async void OnEnable()
    {
        CancellationTokenSource source = new CancellationTokenSource();
        CancellationToken token = source.Token;

        var poolHandle = await GetPoolMangers(token);
        _enemyPoolPrefab = poolHandle.enemy;
        _playerPoolPrefab = poolHandle.player;

        InstacePoolManagers();

        PooledObject obj = _playerPool.GetPooledObject();
        obj.transform.position = Vector2.zero;
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
}

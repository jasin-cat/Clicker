using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.InputSystem;

public class Click
{
    GameManager _gameManager;
    private Shopping _shopping;
    SOGold _gold;
    private InputActions _inputActions;
    private Action<int> GetGold;
    private Action<int> ViewGold;
    private bool _clickwait = true;
    private bool _enemyIsDead = false;
    CancellationTokenSource source = new();
    public Click(GameManager manager, Action<int> getAction, Action<int> viewAction)
    {
        _gameManager = manager;
        _inputActions = new();
        _inputActions.Mouse.Click.performed += OnLeftClick;
        _inputActions.Mouse.AttackUp.performed += OnFKeyClick;

        GetGold = getAction;
        ViewGold = viewAction;

        Init(source.Token).Forget();

        _shopping = new(_gold);
    }

    public void Dispose()
    {
        _inputActions.Mouse.Click.performed -= OnLeftClick;
        _inputActions.Mouse.AttackUp.performed -= OnFKeyClick;

        GetGold = default;

        _inputActions.Mouse.Disable();
    }

    private async UniTaskVoid Init(CancellationToken token)
    {
        _gold = await RegisterSOGold(token);
        _inputActions.Mouse.Enable();
    }

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

    private void OnLeftClick(InputAction.CallbackContext context)
    {
        if(_enemyIsDead) return;
        Debug.Log("クリックした");

        if (_clickwait || !_enemyIsDead)
        {
            _clickwait = false;

            GetGold?.Invoke(1);
            ViewGold?.Invoke(_gold.Gold);

            ClickWait().Forget();
        }

    }

    private async UniTaskVoid ClickWait()
    {
        await UniTask.Delay(TimeSpan.FromSeconds(2f));

        _clickwait = true;
    }

    public void IsDead()
    {
        _enemyIsDead = true;
    }

    public void IsRevive()
    {
        _enemyIsDead = false;
    }

    private void OnFKeyClick(InputAction.CallbackContext context)
    {
        _shopping.Attack();
    }
}

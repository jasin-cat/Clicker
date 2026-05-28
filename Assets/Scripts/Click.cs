using System;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UniRx;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.InputSystem;

public class Click
{
    [SerializeField]
    GameManager _gameManager;
    SOGold _gold;
    private InputActions _inputActions;
    public Action IsClick;
    private bool _clickwait = true;

    CancellationTokenSource source = new();
    public Click(GameManager manager, Action action)
    {
        _gameManager = manager;

        
        
        _inputActions = new();
        _inputActions.Mouse.Click.performed += OnLeftClick;

        IsClick = action;

        Init(source.Token).Forget();
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
        Debug.Log("クリックした");

        if (_clickwait)
        {
            _clickwait = false;

            IsClick?.Invoke();

            ClickWait().Forget();
        }

    }

    private async UniTaskVoid ClickWait()
    {
        await UniTask.Delay(TimeSpan.FromSeconds(0.2f));

        _clickwait = true;
    }
}

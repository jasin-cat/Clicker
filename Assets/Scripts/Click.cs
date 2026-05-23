using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.InputSystem;

public class Click
{
    [SerializeField]
    GameManager _gameManager;
    SOGold _gold;
    private InputActions _inputActions;
    public Click(GameManager manager)
    {
        _gameManager = manager;

        CancellationTokenSource source = new();
        CancellationToken token = source.Token;
        
        _inputActions = new();
        _inputActions.Mouse.Click.performed += OnLeftClick;

        Init(token).Forget();
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
        if(!context.started) return;
    }
}

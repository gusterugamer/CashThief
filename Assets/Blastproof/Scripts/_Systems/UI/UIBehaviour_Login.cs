using Blastproof.Systems.Core.Variables;
using Blastproof.Systems.UI;
using UnityEngine;

public class UIBehaviour_Login : UIBehaviour
{
    [SerializeField] UIState _gameplayState;
    [SerializeField] BoolVariable _loginBool;

    protected override void OnOpened()
    {
        base.OnOpened();
        _loginBool.onValueChanged += OnLoggedStateChanged;
    }

    protected override void OnClosed()
    {
        base.OnClosed();
        _loginBool.onValueChanged -= OnLoggedStateChanged;
    }

    private void OnLoggedStateChanged(bool loginState)
    {
        if (loginState) _gameplayState.Activate();
    }
}

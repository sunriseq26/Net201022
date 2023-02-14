using UnityEngine;
using UnityEngine.UI;
using PlayFab;
using PlayFab.ClientModels;
using TMPro;

public class SignInWindow : AccountDataWindowBase
{
    [SerializeField] private Button _signInButton;
    
    protected override void SubscriptionsElementsUi()
    {
        base.SubscriptionsElementsUi();
        
        _signInButton.onClick.AddListener(SignIn);
    }

    private void SignIn()
    {
        ConnectionInfo(true, _connecting, Color.green);
        PlayFabClientAPI.LoginWithPlayFab(new LoginWithPlayFabRequest
        {
            Username = _username,
            Password = _password
        }, result =>
        {
            Debug.Log($"Success: {_username}");
            ConnectionInfo(false, string.Empty, Color.white);
            EnterInGameScene(result.PlayFabId);
        }, error =>
        {
            Debug.LogError($"Fail: {error.ErrorMessage}");
            ConnectionInfo(false, error.ErrorMessage, Color.red);
        });
    }
}

using UnityEngine;
using UnityEngine.UI;
using PlayFab;
using PlayFab.ClientModels;

public class CreateAccountWindow : AccountDataWindowBase
{
    [SerializeField] private InputField _mailField;
    [SerializeField] private Button _createAccountButton;

    private string _mail;

    protected override void SubscriptionsElementsUi()
    {
        base.SubscriptionsElementsUi();
        
        _mailField.onValueChanged.AddListener(UpdateMail);
        _createAccountButton.onClick.AddListener(CreateAccount);
    }

    private void CreateAccount()
    {
        ConnectionInfo(true, _connecting, Color.green);
        PlayFabClientAPI.RegisterPlayFabUser(new RegisterPlayFabUserRequest
        {
            Username = _username,
            Email = _mail,
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

    private void UpdateMail(string mail)
    {
        _mail = mail;
    }
}

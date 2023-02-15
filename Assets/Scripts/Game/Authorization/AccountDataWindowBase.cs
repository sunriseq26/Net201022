using Photon.Pun;
using PlayFab;
using PlayFab.ClientModels;
using PlayFab.EconomyModels;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Image = UnityEngine.UI.Image;

public class AccountDataWindowBase : MonoBehaviourPunCallbacks
{
    [SerializeField] private InputField _usernameField;
    [SerializeField] private InputField _passwordField;
    [SerializeField] private Image _bgConnection;
    [SerializeField] private TMP_Text _textProcess;

    protected string _username;
    protected string _password;
    protected string _connecting = "Connecting...";

    private void Start()
    {
        SubscriptionsElementsUi();
    }

    protected virtual void SubscriptionsElementsUi()
    {
        _usernameField.onValueChanged.AddListener(UpdateUsername);
        _passwordField.onValueChanged.AddListener(UpdatePassword);
    }

    private void UpdateUsername(string username)
    {
        _username = username;
    }

    private void UpdatePassword(string password)
    {
        _password = password;
    }

    protected void EnterInGameScene(string playFabId)
    {
        SceneManager.LoadScene(1);
        //SetUserData(playFabId);
        //MakePurchase();
        //GetInventory();
    }

    protected void ConnectionInfo(bool enable, string message, Color color)
    {
        _bgConnection.enabled = enable;
        _textProcess.text = message;
        _textProcess.color = color;
    }
}

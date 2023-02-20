using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Image = UnityEngine.UI.Image;

public class AccountDataWindowBase : MonoBehaviourPunCallbacks
{
    [SerializeField] private ServerSettings _serverSettings;
    [SerializeField] private InputField _usernameField;
    [SerializeField] private InputField _passwordField;
    [SerializeField] private Image _bgConnection;
    [SerializeField] private TMP_Text _textProcess;

    protected string _username;
    protected string _password;
    protected string _connecting = "Connecting...";
    protected string gameVersion = "1";
    
    private TypedLobby _sqlLobby = new TypedLobby("CustomSqlLobby", LobbyType.SqlLobby);

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
    
    public void Connect()
    {
        _textProcess.text = "";
        
        if (PhotonNetwork.IsConnected)
        {
            LogFeedback("Joining Room...");
            PhotonNetwork.JoinLobby();
        }else{
            LogFeedback("Connecting...");
            PhotonNetwork.ConnectUsingSettings(_serverSettings.AppSettings);
            PhotonNetwork.GameVersion = this.gameVersion;
        }
    }
    
    void LogFeedback(string message)
    {
        if (_textProcess == null) {
            return;
        }
        _textProcess.text += System.Environment.NewLine+message;
    }
    
    public override void OnConnectedToMaster()
    {
        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.JoinLobby(_sqlLobby);
            Debug.LogWarning("OnConnectedToMaster: Next -> try to Join Lobby");
        }
    }

    public override void OnJoinedLobby()
    {
        LogFeedback("OnJoinedLobby: Next -> try to LoadScene(1)");
        Debug.LogWarning("OnJoinedLobby");
        SceneManager.LoadScene(1);
    }
}

using System;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;

public class ConnectAndJoinRandomLb : MonoBehaviour, IConnectionCallbacks, IMatchmakingCallbacks, ILobbyCallbacks
{
    [SerializeField] private ServerSettings _serverSettings;
    [SerializeField] private TMP_Text _stateUiText;
    
    [SerializeField] private Transform _content;
    [SerializeField] private GameObject _prefabItem;
    
    private readonly Dictionary<string, RoomInfo> _catalog = new Dictionary<string, RoomInfo>();
    
    private LoadBalancingClient _lbc;

    private const string GAME_MODE_KEY = "gm";
    private const string AI_MODE_KEY = "ai";
    
    private const string MAP_PROP_KEY = "C0";
    private const string GOLD_PROP_KEY = "C1";

    private TypedLobby _sqlLobby;
    private TypedLobby _defaultLobby = new TypedLobby("DefaultLobby", LobbyType.Default);

    private void Awake()
    {
        _sqlLobby = new TypedLobby("CustomSqlLobby", LobbyType.SqlLobby);
    }

    private void Start()
    {
        _lbc = new LoadBalancingClient();
        _lbc.AddCallbackTarget(this);

        _lbc.ConnectUsingSettings(_serverSettings.AppSettings);
    }

    private void OnDestroy()
    {
        _lbc.RemoveCallbackTarget(this);
    }

    private void Update()
    {
        if (_lbc == null)
            return;
        
        _lbc.Service();

        var state = _lbc.State.ToString();
        _stateUiText.text = state;
    }

    public void OnConnected()
    {
    }

    public void OnConnectedToMaster()
    {
        Debug.Log("OnConnectedToMaster");
        _lbc.OpJoinLobby(_sqlLobby);

        // var roomOptions = new RoomOptions
        // {
        //     MaxPlayers = 12,
        //     PublishUserId = true,
        //     CustomRoomPropertiesForLobby = new[] { MAP_PROP_KEY, GOLD_PROP_KEY },
        //     CustomRoomProperties = new Hashtable{{GOLD_PROP_KEY, 400}, {MAP_PROP_KEY, "Map3"}}
        // };
        //
        // var enterRoomParams = new EnterRoomParams
        // {
        //     RoomName = "NewRoom", 
        //     RoomOptions = roomOptions,
        //     ExpectedUsers = new []{"r23r23r"},
        //     Lobby = _sqlLobby
        // };
        // _lbc.OpCreateRoom(enterRoomParams);
    }

    public void OnDisconnected(DisconnectCause cause)
    {
    }

    public void OnRegionListReceived(RegionHandler regionHandler)
    {
    }

    public void OnCustomAuthenticationResponse(Dictionary<string, object> data)
    {
    }

    public void OnCustomAuthenticationFailed(string debugMessage)
    {
        
    }

    public void OnFriendListUpdate(List<FriendInfo> friendList)
    {
    }

    public void OnConnectedToServer()
    {
        //_lbc.OpJoinLobby(_sqlLobby);
    }

    public void OnCreatedRoom()
    {
        Debug.Log("OnCreatedRoom");
    }

    public void OnCreateRoomFailed(short returnCode, string message)
    {
    }

    public void OnJoinedRoom()
    {
        Debug.Log("OnJoinedRoom");
        //_lbc.CurrentRoom.Players.Values.First().UserId;
    }

    public void OnJoinRoomFailed(short returnCode, string message)
    {
    }

    public void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("OnJoinRandomFailed");
        _lbc.OpCreateRoom(new EnterRoomParams());
    }

    public void OnLeftRoom()
    {
    }

    public void OnJoinedLobby()
    {
        Debug.Log("OnJoinedLobby");
        var sqlLobbyFilter = $"{MAP_PROP_KEY} = Map3 AND {GOLD_PROP_KEY} BETWEEN 300 AND 500";
        
        var opJoinRandomRoomParams = new OpJoinRandomRoomParams
        {
            SqlLobbyFilter = sqlLobbyFilter
        };

        if (_lbc.OpGetGameList(_sqlLobby, sqlLobbyFilter))
        {
            _lbc.OpJoinRandomRoom(opJoinRandomRoomParams);
        }
    }

    public void OnLeftLobby()
    {
    }

    public void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        Debug.LogWarning($"OnRoomListUpdate successfully!");
        foreach (var item in roomList)
        {
            _catalog.Add(item.Name, item);
            Debug.LogWarning($"Catalog item {item.Name} was added successfully!");
            var itemTMP_Text = Instantiate(_prefabItem, _content);
            itemTMP_Text.GetComponent<TMP_Text>().text = $"Name Item: {item.Name}";
        }
    }

    public void OnLobbyStatisticsUpdate(List<TypedLobbyInfo> lobbyStatistics)
    {
        Debug.LogWarning(lobbyStatistics.Count);
    }
}

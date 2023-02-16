using System.Collections.Generic;
using Photon.Pun;
using PlayFab;
using PlayFab.ClientModels;
using TMPro;
using UnityEngine;


public class AccountInfo : AccountManagerBase
{
    [SerializeField] private TMP_Text _titleLabel;
    [SerializeField] private Transform _content;
    [SerializeField] private GameObject _prefabItem;
    
    private readonly Dictionary<string, CatalogItem> _catalog = new Dictionary<string, CatalogItem>();


    private void Start()
    {
        PlayFabClientAPI.GetAccountInfo(new GetAccountInfoRequest(), OnGetAccount, OnError);
    }

    protected override void OnGetAccount(GetAccountInfoResult result)
    {
        base.OnGetAccount(result);
        PhotonNetwork.NickName = _accountResult.AccountInfo.Username;
        _titleLabel.text = $"Username: {result.AccountInfo.Username}";
    }
}
using System.Collections.Generic;
using PlayFab;
using PlayFab.ClientModels;
using TMPro;
using UnityEngine;

public class PlayFabAccountManager : MonoBehaviour
{
    [SerializeField] private TMP_Text _titleLabel;
    [SerializeField] private Transform _content;
    [SerializeField] private GameObject _prefabItem;
    
    private readonly Dictionary<string, CatalogItem> _catalog = new Dictionary<string, CatalogItem>();


    private void Start()
    {
        PlayFabClientAPI.GetAccountInfo(new GetAccountInfoRequest(), OnGetAccount, OnError);
        PlayFabClientAPI.GetCatalogItems(new GetCatalogItemsRequest(), OnGetCatalogSuccess,OnError);
    }

    private void OnGetAccount(GetAccountInfoResult result)
    {
        _titleLabel.text = $"Playfab id: {result.AccountInfo.PlayFabId}" + "\n"
                           + $"Username: {result.AccountInfo.Username}" + "\n"
                           + $"Email: {result.AccountInfo.PrivateInfo.Email}";
    }

    private void OnError(PlayFabError error)
    {
        var errorMessage = error.GenerateErrorReport();
        Debug.LogError($"Something went wrong: {errorMessage}");
    }

    private void OnGetCatalogSuccess(GetCatalogItemsResult result)
    {
        HandleCatalog(result.Catalog);
        Debug.Log($"Catalog was loaded successfully!");
    }
    
    private void HandleCatalog(List<CatalogItem> catalog)
    {
        foreach (var item in catalog)
        {
            _catalog.Add(item.ItemId, item);
            Debug.Log($"Catalog item {item.ItemId} was added successfully!");
            var itemTMP_Text = Instantiate(_prefabItem, _content);
            itemTMP_Text.GetComponent<TMP_Text>().text = $"Name Item: {item.DisplayName}";
        }
    }
}

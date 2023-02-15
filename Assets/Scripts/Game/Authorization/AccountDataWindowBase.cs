using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using PlayFab;
using PlayFab.ClientModels;
using PlayFab.EconomyModels;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Image = UnityEngine.UI.Image;

public class AccountDataWindowBase : MonoBehaviour
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
        SetUserData(playFabId);
        //MakePurchase();
        GetInventory();
    }

    private void GetInventory()
    {
        PlayFabClientAPI.GetUserInventory(new GetUserInventoryRequest(),
            result => ShowInvetory(result.Inventory), OnLoginFailure);
    }

    private void ShowInvetory(List<ItemInstance> inventory)
    {
        var firstItem = inventory.First();
        Debug.Log($"{firstItem.ItemId}");
        ConsumePotion(firstItem.ItemInstanceId);
    }

    private void ConsumePotion(string itemInstanceId)
    {
        PlayFabClientAPI.ConsumeItem(new ConsumeItemRequest
        {
            ConsumeCount = 1,
            ItemInstanceId = itemInstanceId
        },result =>
        {
            Debug.Log("Complete ConsumePotion");
        }, OnLoginFailure);
    }

    private void MakePurchase()
    {
        PlayFabClientAPI.PurchaseItem(new PurchaseItemRequest
        {
            CatalogVersion = "main",
            ItemId = "health_potion",
            Price = 3,
            VirtualCurrency = "SC"
        },result =>
        {
            Debug.Log("Complete PurchaseItem");
        }, OnLoginFailure);
    }

    protected void ConnectionInfo(bool enable, string message, Color color)
    {
        _bgConnection.enabled = enable;
        _textProcess.text = message;
        _textProcess.color = color;
    }
    
    private void SetUserData(string playFabId)
    {
        PlayFabClientAPI.UpdateUserData(new UpdateUserDataRequest
        {
            Data = new Dictionary<string, string>
            {
                { "time_recieve_daily_reward", DateTime.UtcNow.ToString() }
            }
        }, result =>
        {
            Debug.Log("SetUserData");
            GetUserData(playFabId, "time_recieve_daily_reward");
        }, OnLoginFailure);
    }

    private void GetUserData(string playFabId, string keyData)
    {
        PlayFabClientAPI.GetUserData(new GetUserDataRequest
        {
            PlayFabId =  playFabId
        }, result =>
        {
            if (result.Data.ContainsKey(keyData))
                Debug.Log($"{keyData}: {result.Data[keyData].Value}");
        }, OnLoginFailure);
    }

    private void OnLoginFailure(PlayFabError error)
    {
        var errorMessage = error.GenerateErrorReport();
        Debug.LogError($"Something went wrong: {errorMessage}");
    }
}

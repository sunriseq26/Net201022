using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;

public class UserPlayFabMethods : MonoBehaviour
{
    

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

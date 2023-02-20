using System;
using System.Collections.Generic;
using PlayFab;
using PlayFab.ClientModels;
using TMPro;
using UnityEngine;

public class PlayfabLogin : MonoBehaviour
{
    private TMP_Text _tmpText;
    private const string AuthGuidKey = "auth_guid_key";

    private void Awake()
    {
        _tmpText = GetComponentInChildren<TMP_Text>();
        _tmpText.color = Color.black;
    }

    public void LogIn()
    {
        if (string.IsNullOrEmpty(PlayFabSettings.staticSettings.TitleId))
            PlayFabSettings.staticSettings.TitleId = "2885B";

        var needCreation = PlayerPrefs.HasKey(AuthGuidKey);
        var id = PlayerPrefs.GetString(AuthGuidKey, Guid.NewGuid().ToString());
        
        var request = new LoginWithCustomIDRequest 
        { 
            CustomId = id,
            CreateAccount = !needCreation 
        };
        
        PlayFabClientAPI.LoginWithCustomID(request, 
            result =>
            {
                PlayerPrefs.SetString(AuthGuidKey, id);
                OnLoginSuccess(result);
            }, OnLoginFailure);
    }
    
    private void OnLoginSuccess(LoginResult result)
    {
        Debug.Log("Complete Login!!!");
        _tmpText.color = Color.green;
        SetUserData(result.PlayFabId);
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
        _tmpText.color = Color.red;
        Debug.LogError($"Something went wrong: {errorMessage}");
    }
}

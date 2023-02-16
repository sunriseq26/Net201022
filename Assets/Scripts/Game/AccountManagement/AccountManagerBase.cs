using Photon.Pun;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;


public class AccountManagerBase : MonoBehaviourPunCallbacks
{
    protected GetAccountInfoResult _accountResult;
    

    protected virtual void OnGetAccount(GetAccountInfoResult result)
    {
        _accountResult = result;
    }

    protected void OnError(PlayFabError error)
    {
        var errorMessage = error.GenerateErrorReport();
        Debug.LogError($"Something went wrong: {errorMessage}");
    }
}
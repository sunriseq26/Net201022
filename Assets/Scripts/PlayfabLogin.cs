using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;

public class PlayfabLogin : MonoBehaviour
{
    private void Start()
    {
        if (string.IsNullOrEmpty(PlayFabSettings.staticSettings.TitleId))
            PlayFabSettings.staticSettings.TitleId = "2885B";
        
        var request = new LoginWithCustomIDRequest { CustomId = "Player 1",
            CreateAccount = true };
        
        PlayFabClientAPI.LoginWithCustomID(request, OnLoginSuccess, OnLoginFailure);
        
        void OnLoginSuccess(LoginResult result)
        {
            Debug.Log("Congratulations, you made successful API call!");
        }
        void OnLoginFailure(PlayFabError error)
        {
            var errorMessage = error.GenerateErrorReport();
            Debug.LogError($"Something went wrong: {errorMessage}");
        }

    }
}

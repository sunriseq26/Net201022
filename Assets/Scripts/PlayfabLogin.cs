using PlayFab;
using PlayFab.ClientModels;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayfabLogin : MonoBehaviour
{
    private TMP_Text _tmpText;
    
    private void Awake()
    {
        _tmpText = GetComponentInChildren<TMP_Text>();
        _tmpText.color = Color.black;
    }

    public void LogIn()
    {
        if (string.IsNullOrEmpty(PlayFabSettings.staticSettings.TitleId))
            PlayFabSettings.staticSettings.TitleId = "2885B";
        
        var request = new LoginWithCustomIDRequest { CustomId = "Player 1",
            CreateAccount = true };
        
        PlayFabClientAPI.LoginWithCustomID(request, OnLoginSuccess, OnLoginFailure);
        
        void OnLoginSuccess(LoginResult result)
        {
            _tmpText.color = Color.green;
        }
        void OnLoginFailure(PlayFabError error)
        {
            var errorMessage = error.GenerateErrorReport();
            _tmpText.color = Color.red;
            Debug.LogError($"Something went wrong: {errorMessage}");
        }
    }
}

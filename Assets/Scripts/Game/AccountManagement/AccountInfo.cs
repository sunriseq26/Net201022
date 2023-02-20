using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using PlayFab;
using PlayFab.ClientModels;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;


public class AccountInfo : AccountManagerBase
{
    [SerializeField] private TMP_Text _titleLabel;
    [SerializeField] private GameObject _newCharacterCreatePanel;
    [SerializeField] private Button _createCharacterButton;
    [SerializeField] private TMP_InputField _inputField;
    [FormerlySerializedAs("_slot")] [SerializeField] private List<SlotCharacterWidget> _slots;

    [SerializeField] private Transform _content;
    [SerializeField] private GameObject _prefabItem;
    
    private readonly Dictionary<string, CatalogItem> _catalog = new Dictionary<string, CatalogItem>();

    private string _characterName;
    

    private void Start()
    {
        PlayFabClientAPI.GetAccountInfo(new GetAccountInfoRequest(), OnGetAccount, OnError);

        GetCharacters();

        foreach (var slot in _slots)
            slot.SlotButton.onClick.AddListener(OpenCreateNewCharacter);
        
        _inputField.onValueChanged.AddListener(OnNameChanged);
        _createCharacterButton.onClick.AddListener(CreateCharacter);
    }

    private void CreateCharacter()
    {
        PlayFabClientAPI.GrantCharacterToUser(new GrantCharacterToUserRequest
        {
            CharacterName = _characterName,
            ItemId = "character_token"
        }, result =>
        {
            UpdateCharacterStatistics(result.CharacterId);
        }, OnError);
    }

    private void UpdateCharacterStatistics(string characterId)
    {
        PlayFabClientAPI.UpdateCharacterStatistics(new UpdateCharacterStatisticsRequest
        {
            CharacterId = characterId,
            CharacterStatistics = new Dictionary<string, int>
            {
                {"Level", 1},
                {"Gold", 0}
            }
        }, result =>
        {
            Debug.LogError($"Complete!!!");
            CloseCreateNewCharacter();
            GetCharacters();
        }, OnError);
    }

    private void OnNameChanged(string changedName)
    {
        _characterName = changedName;
    }

    private void GetCharacters()
    {
        PlayFabClientAPI.GetAllUsersCharacters(new ListUsersCharactersRequest(), 
            result =>
        {
            Debug.LogError($"Character count: {result.Characters.Count}");
            ShowCharactersInSlot(result.Characters);
        }, OnError);
    }

    private void ShowCharactersInSlot(List<CharacterResult> characters)
    {
        if (characters.Count == 0)
        {
            foreach (var slot in _slots)
                slot.ShowEmptySlot();
        }
        else if (characters.Count > 0 && characters.Count < _slots.Count)
        {
            PlayFabClientAPI.GetCharacterStatistics(new GetCharacterStatisticsRequest
            {
                CharacterId = characters.First().CharacterId
            }, 
                result =>
                {
                    var level = result.CharacterStatistics["Level"].ToString();
                    var gold = result.CharacterStatistics["Gold"].ToString();
                    
                    _slots.First().ShowInfoCharacterSlot(characters.First().CharacterName, level, gold);
                }, OnError);
        }
        else
        {
            Debug.LogError($"Add slots for character");
        }
    }

    private void OpenCreateNewCharacter()
    {
        _newCharacterCreatePanel.SetActive(true);
    }
    
    private void CloseCreateNewCharacter()
    {
        _newCharacterCreatePanel.SetActive(false);
    }

    protected override void OnGetAccount(GetAccountInfoResult result)
    {
        base.OnGetAccount(result);
        PhotonNetwork.NickName = _accountResult.AccountInfo.Username;
        _titleLabel.text = $"Username: {result.AccountInfo.Username}";
    }
}
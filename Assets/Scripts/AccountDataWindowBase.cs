using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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

    protected void EnterInGameScene()
    {
        SceneManager.LoadScene(1);
    }
    
    protected void ConnectionInfo(bool enable, string message, Color color)
    {
        _bgConnection.enabled = enable;
        _textProcess.text = message;
        _textProcess.color = color;
    }
}

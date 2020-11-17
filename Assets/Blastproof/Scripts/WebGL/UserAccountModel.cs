using UnityEngine;
using System.Runtime.InteropServices;
using TMPro;
using UnityEngine.UI;
using System;
using Blastproof.Systems.UI;
using Blastproof.Systems.Core.Variables;
using Sirenix.OdinInspector;
using System.Collections.Generic;

[Serializable]
public class AuthenticationModel
{
    public string uid;
}

public class InputModel
{
    public string text;
    public int id;
}

public class UserAccountModel : SerializedMonoBehaviour
{

    #region External Functions
    [DllImport("__Internal")]
    private static extern void LoginExternal(string Email, string Password);

    [DllImport("__Internal")]
    private static extern void RegisterExternal(string FullName, string Email, string Password, string PhoneNumber, string Address, string BirthDate);

    [DllImport("__Internal")]
    private static extern void AddClickListenerForFileDialog();

    [DllImport("__Internal")]
    private static extern void FocusFileUploader();

    [DllImport("__Internal")]
    private static extern void LogoutExternal();

    [DllImport("__Internal")]
    private static extern void FocusInputExternal(string Text, int Id);

    //currently removed parameters
    [DllImport("__Internal")]
    private static extern void BlurInputExternal();
    #endregion

    //todo cleanup or move to separate scripts
    [BoxGroup("Login"), SerializeField] private TMP_InputField EmailInputField;
    [BoxGroup("Login"), SerializeField] private TMP_InputField PasswordInputField;

    [BoxGroup("Register"), SerializeField] private TMP_InputField RegisterEmailInputField;
    [BoxGroup("Register"), SerializeField] private TMP_InputField RegisterFullNameInputField;
    [BoxGroup("Register"), SerializeField] private TMP_InputField RegisterPasswordInputField;
    [BoxGroup("Register"), SerializeField] private TMP_InputField RegisterPhoneNumberInputField;
    [BoxGroup("Register"), SerializeField] private TMP_InputField RegisterAddressInputField;
    [BoxGroup("Register"), SerializeField] private TMP_InputField RegisterBirthdateInputField;

    [BoxGroup("UI"), SerializeField] private UISystem UISystemController;
    [BoxGroup("UI"), SerializeField] private BoolVariable IsAuthenticatedSubState;

    [BoxGroup("Data"), SerializeField] private AuthenticationModel _data;

    [BoxGroup("Profile"), SerializeField] private PlayerProfile _playerProfile;

    [BoxGroup("Helpers"), SerializeField] private List<TextInputHelper> _inputFields = new List<TextInputHelper>();

    private void Start()
    {
        //Buttons requiring upload input use a "hack" as there needs to be a flag set on a html element BEFORE the clicking action
#if UNITY_WEBGL && !UNITY_EDITOR
        //AddClickListenerForFileDialog();
#endif
        //
        //UploadFileButton.onDown.AddListener(TestUpload);
        _inputFields.AddRange(GameObject.FindObjectsOfType<TextInputHelper>());
        _inputFields.ForEach(inputField =>
        {
            inputField.OnSelected += InputSelected;
            inputField.OnDeselected += InputDeselected;
        });
    }

    void InputSelected(string InputValue, TextInputHelper TextInput)
    {
        Debug.Log($"Unity > Selecting {InputValue} + position {_inputFields.IndexOf(TextInput)}");
#if UNITY_WEBGL && !UNITY_EDITOR
        FocusInputExternal(InputValue, _inputFields.IndexOf(TextInput));
#endif
    }

    void InputDeselected(string InputValue, TextInputHelper TextInput)
    {
        Debug.Log($"Unity > Deselecting {InputValue} + position {_inputFields.IndexOf(TextInput)}");
#if UNITY_WEBGL && !UNITY_EDITOR
        BlurInputExternal();
#endif
    }

    public void InputTextChanged(string InputModel)
    {
        var data = JsonUtility.FromJson<InputModel>(InputModel);
        Debug.Log("Unity > Input got to unity " + data.text);
        _inputFields[data.id].InputField.text = data.text;
    }

    void UpdateProfileData(string ProfileData)
    {
        Debug.Log($"Update Profile Data >> From Javascript {ProfileData}");
        _playerProfile.UpdateData(ProfileData);
    }

    public void TestUpload()
    {
        Debug.Log("Pressed Test External");
#if UNITY_WEBGL
        FocusFileUploader();
#endif
    }

    public void Login()
    {
        if (!EmailInputField || !PasswordInputField)
            return;

        Debug.Log($"{EmailInputField.text} {PasswordInputField.text}");
#if UNITY_WEBGL && !UNITY_EDITOR
        LoginExternal(EmailInputField.text, PasswordInputField.text);
#else
        IsAuthenticatedSubState.Value = true;
#endif
    }

    public void Logout()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        LogoutExternal();
#endif
    }

    public void RegisterAction()
    {
        //todo add checks
        RegisterExternal(
            RegisterFullNameInputField.text, 
            RegisterEmailInputField.text, 
            RegisterPasswordInputField.text, 
            RegisterPhoneNumberInputField.text, 
            RegisterAddressInputField.text, 
            RegisterBirthdateInputField.text);
    }

    public void ReceiveAuthenticationDetails(string userId)
    {
        IsAuthenticatedSubState.Value = !String.IsNullOrEmpty(userId);
        Debug.Log(IsAuthenticatedSubState.Value);
    }

    [Button]
    void TestChangeUIState(UIState State)
    {
        UISystemController.ChangeState(State);
    }
}

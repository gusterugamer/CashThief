using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class TextInputHelper : MonoBehaviour
{
    TMP_InputField _inputField;
    public TMP_InputField InputField => _inputField ? _inputField : GetComponent<TMP_InputField>();

    public Action<string, TextInputHelper> OnSelected;
    public Action<string, TextInputHelper> OnDeselected;

    void Start()
    {
        InputField.onSelect.AddListener(InputSelected);
        InputField.onDeselect.AddListener(InputDeselected);
    }

    void InputSelected(string input)
    {
        //Debug.Log(input);
        OnSelected?.Invoke(input, this);
    }

    void InputDeselected(string input)
    {
        //Debug.Log(input);
        OnDeselected?.Invoke(input, this);
    }
}

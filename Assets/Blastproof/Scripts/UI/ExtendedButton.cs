using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ExtendedButton : Button
{
    [SerializeField]
    ButtonDownEvent _onDown = new ButtonDownEvent();

    public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);

        _onDown.Invoke();
    }

    public ButtonDownEvent onDown
    {
        get { return _onDown; }
        set { _onDown = value; }
    }

    [Serializable]
    public class ButtonDownEvent : UnityEvent { }
}

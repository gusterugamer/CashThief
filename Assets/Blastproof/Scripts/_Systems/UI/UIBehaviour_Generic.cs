using Blastproof.Systems.Core;
using Blastproof.Systems.UI;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIBehaviour_Generic : UIBehaviour
{
    public class GenericButton
    {
        public string text;
        public Action action;

        public GenericButton(string _text, Action _action)
        {
            text = _text;
            action = _action;
        }
    }

    [SerializeField] private PopupEvent _popupEvent;
    [SerializeField] private SimpleEvent _popupCloseEvent;
    [SerializeField] private UISystem _system;
    [SerializeField] private UIState _state;

    [SerializeField] private TextMeshProUGUI _title;
    [SerializeField] private TextMeshProUGUI _description;
    [SerializeField] private Button _button;

    [ShowInInspector, ReadOnly] private UIState _cachedState;
    //[ShowInInspector, ReadOnly] private UISubState _cachedSubState;

    private List<Button> _buttons = new List<Button>();

    protected override void OnEnable()
    {
        base.OnEnable();
        _popupEvent.Subscribe(Open);
        _popupCloseEvent.Subscribe(Close);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        _popupEvent.Unsubscribe(Open);
        _popupCloseEvent.Unsubscribe(Close);
    }

    private void Open(string title, string description, List<GenericButton> buttons)
    {
        _title.text = title;
        _description.text = description;

        foreach(var button in buttons)
        {
            var obj = Instantiate(_button, _button.transform.parent);
            obj.gameObject.SetActive(true);
            obj.GetComponentInChildren<TextMeshProUGUI>().text = button.text;
            obj.gameObject.SetActive(true);
            obj.onClick.AddListener(new UnityAction(button.action));
            _buttons.Add(obj);
        }

        _cachedState = _system.CurrentState;
        //_cachedSubState = _system.CurrentSubState;
        _system.ChangeState(_state);
    }

    private void Close()
    {
        _system.ChangeState(_cachedState);
        //_system.ChangeSubstate(_cachedSubState);

        _buttons.ForEach(x => x.gameObject.Destroy());
        _buttons.Clear();
    }

    [ContextMenu("TEST Short")]
    private void TestShort()
    {
        Open("Title srt",
            "Description bla bla bla bla bla bla bla bla bla bla bla",
            new List<GenericButton> { new GenericButton("Close", () => { Close(); }) }
        );
    }

    [ContextMenu("TEST Long")]
    private void TestLong()
    {
        Open(
            "Title 1 long",
            "Description bla bla bla bla bla bla bla bla bla bla bla long",
            new List<GenericButton> { new GenericButton("Close", () => { Close(); }) }
        );
    }
}

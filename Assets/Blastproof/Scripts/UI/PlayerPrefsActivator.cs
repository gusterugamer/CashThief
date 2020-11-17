using Blastproof.Tools.Elements;

using UnityEngine;

public class PlayerPrefsActivator : ButtonElement
{
    [SerializeField] PlayerPrefsInt _variable;
    [SerializeField] GameObject _if_one;
    [SerializeField] GameObject _if_zero;

    protected override void OnEnable()
    {
        base.OnEnable();
        _variable.onValueChanged += UpdateObjects;
        UpdateObjects();
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        _variable.onValueChanged -= UpdateObjects;
    }

    protected override void OnButtonClick()
    {
        base.OnButtonClick();
        _variable.Value = _variable.Value == 1 ? 0 : 1;
    }


    private void UpdateObjects()
    {
        _if_zero.SetActive(_variable.Value == 0);
        _if_one.SetActive(_variable.Value == 1);
    }
}

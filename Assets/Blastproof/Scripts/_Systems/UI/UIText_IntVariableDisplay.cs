using Blastproof.Systems.Core;
using Blastproof.Systems.Core.Variables;
using Blastproof.Tools.Elements;
using UnityEngine;

public class UIText_IntVariableDisplay : TextElement
{
    [SerializeField] private IntVariable _variable;
    [SerializeField] private string _context;

    // ---- Subscribe/ Unsubscribe to changes
    private void OnEnable()
    {
        _variable.onValueChanged += UpdateText;
    }

    private void OnDisable()
    {
        _variable.onValueChanged -= UpdateText;
    }

    private void Start()
    {
        UpdateText();
    }

    // ---- Update interace on every change
    private void UpdateText()
    {
        if (_context.EmptyOrNull())
            ThisText.text = _variable.Value.ToString();
        else
            ThisText.text = string.Format(_context, _variable.Value);
    }
}

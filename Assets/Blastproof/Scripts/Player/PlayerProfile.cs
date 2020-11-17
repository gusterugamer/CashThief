using Blastproof.Systems.Core.Variables;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(menuName = "Blastproof/Player/PlayerProfile")]
public class PlayerProfile : SerializedScriptableObject
{
    public string Fullname;
    public string Email;
    public int Balance;

    [BoxGroup("Variables"), SerializeField] private StringVariable _fullNameVariable;
    [BoxGroup("Variables"), SerializeField] private StringVariable _emailVariable;
    [BoxGroup("Variables"), SerializeField] private IntVariable _balanceVariable;

    private void OnEnable()
    {
#if UNITY_EDITOR
        UpdateData("{\"FullName\":\"Tester\", \"Email\":\"tester@test.com\", \"Balance\":20000}");
#endif
    }

    [Button]
    public void UpdateData(string data)
    {
        JsonUtility.FromJsonOverwrite(data, this);

        //json utility can't deserialize directly in a property with get set
        _fullNameVariable.Value = Fullname;
        _emailVariable.Value = Email;
        _balanceVariable.Value = Balance;
    }
}

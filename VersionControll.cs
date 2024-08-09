using TMPro;
using UnityEngine;

public class VersionControll : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _textMeshPro;

    private void Start()
    {
        _textMeshPro.text = Application.version + " Work In Progress";
    }
}

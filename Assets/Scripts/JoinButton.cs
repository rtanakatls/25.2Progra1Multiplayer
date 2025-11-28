using TMPro;
using UnityEngine;

public class JoinButton : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI joinText;

    public void SetText(string text)
    {
        joinText.text = text;
    }
}

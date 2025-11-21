using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NameUI : MonoBehaviour
{
    [SerializeField] private TMP_InputField nameInputField;
    [SerializeField] private Button startButton;

    private void Start()
    {
        startButton.onClick.AddListener(SetName);
    }

    private void SetName()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach(GameObject player in players)
        {
            player.GetComponent<Player>().SetName(nameInputField.text);
        }

        Destroy(gameObject);
    }


}

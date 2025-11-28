using System;
using System.Collections.Generic;
using Unity.Services.Multiplayer;
using UnityEngine;
using UnityEngine.UI;

public class SessionUIManager : MonoBehaviour
{
    [SerializeField] private GameObject sessionCanvas;

    [SerializeField] private GameObject joinButtonPrefab;
    [SerializeField] private GameObject joinButtonContainer;

    [SerializeField] private Button hostButton;
    [SerializeField] private Button refreshButton;

    private void Awake()
    {
        hostButton.onClick.AddListener(Host);
        refreshButton.onClick.AddListener(Refresh);
    }

    private async void Start()
    {
        await MultiplayerServiceManager.Instance.InitializeServicesAsync();
    }


    private async void Host()
    {
        try
        {
            LoadingScreen.Instance.Show();
            await MultiplayerServiceManager.Instance.CreateSessionAsync();
            Destroy(sessionCanvas);
            LoadingScreen.Instance.Hide();
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
            LoadingScreen.Instance.Hide();
        }
    }

    private async void Refresh()
    {
        try
        {
            LoadingScreen.Instance.Show();
            foreach (Transform t in joinButtonContainer.transform)
            {
                if (t != joinButtonContainer.transform)
                {
                    Destroy(t.gameObject);
                }
            }

            IList<ISessionInfo> sessions = await MultiplayerServiceManager.Instance.GetSessionsAsync();
            if (sessions.Count > 0)
            {
                foreach (ISessionInfo session in sessions)
                {
                    GameObject button = Instantiate(joinButtonPrefab, joinButtonContainer.transform);
                    button.GetComponent<JoinButton>().SetText($"Join {session.Id} - {session.AvailableSlots}/{session.MaxPlayers}");
                    button.GetComponent<Button>().onClick.AddListener(() => JoinSession(session.Id));
                }
            }
            else
            {
                Debug.Log("No hay sesiones");
            }
            LoadingScreen.Instance.Hide();
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
            LoadingScreen.Instance.Hide();
        }

    }

    public async void JoinSession(string sessionId)
    {
        try
        {
            LoadingScreen.Instance.Show();
            await MultiplayerServiceManager.Instance.JoinSessionByIdAsync(sessionId);
            Destroy(sessionCanvas);

            LoadingScreen.Instance.Hide();
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
            LoadingScreen.Instance.Hide();
        }
    }

}

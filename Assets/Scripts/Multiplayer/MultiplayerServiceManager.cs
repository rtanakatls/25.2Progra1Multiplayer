using System.Threading.Tasks;
using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Authentication;
using Unity.Services.Multiplayer;
using System.Collections.Generic;


public class MultiplayerServiceManager : MonoBehaviour
{
    private static MultiplayerServiceManager instance;

    public static MultiplayerServiceManager Instance { get { return instance; } }

    private void Awake()
    {
        instance = this;
    }


    public async Task InitializeServicesAsync()
    {
        await UnityServices.InitializeAsync();
        await AuthenticationService.Instance.SignInAnonymouslyAsync();
    }

    public async Task<IHostSession> CreateSessionAsync()
    {
        SessionOptions options = new SessionOptions
        {
            MaxPlayers = 4
        }.WithDistributedAuthorityNetwork();
        IHostSession session = await MultiplayerService.Instance.CreateSessionAsync(options);
        return session;
    }

    public async Task<IList<ISessionInfo>> GetSessionsAsync()
    {
        QuerySessionsOptions options = new QuerySessionsOptions { };
        QuerySessionsResults results=await MultiplayerService.Instance.QuerySessionsAsync(options);
        return results.Sessions;
    }

    public async Task JoinSessionByIdAsync(string sessionId)
    {
        await MultiplayerService.Instance.JoinSessionByIdAsync(sessionId);
    }

}

//using ParrelSync;
using ParrelSync;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Security;
using System.Threading.Tasks;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Matchmaking : MonoBehaviour
{
	[SerializeField] private GameObject _playButton;

	private Lobby _connectedLobby;
	private QueryResponse _lobbies;
	private UnityTransport _transport;
	private const string _joinCodeKey = "J";
	private string _playerID;

	private void Awake() => _transport = FindObjectOfType<UnityTransport>();

	public async void CreateOrJoinLobby()
	{
		await Authenticate();

		_connectedLobby = await QuickJoinLobby() ?? await CreateLobby();

		if (_connectedLobby != null) _playButton.SetActive(false);
	}

	private async Task Authenticate()
	{
		var options = new InitializationOptions();

#if UNITY_EDITOR
		options.SetProfile(ClonesManager.IsClone() ? ClonesManager.GetArgument() : "Primary");
#endif

		await UnityServices.InitializeAsync(options);

		await AuthenticationService.Instance.SignInAnonymouslyAsync();
		_playerID = AuthenticationService.Instance.PlayerId;
	}
	private async Task<Lobby> QuickJoinLobby()
	{
		try
		{
			var lobby = await Lobbies.Instance.QuickJoinLobbyAsync();

			var a = await RelayService.Instance.JoinAllocationAsync(lobby.Data[_joinCodeKey].Value);

			SetTransformAsClient(a);

			NetworkManager.Singleton.StartClient();
			return lobby;
		}
		catch(Exception)
		{
			Debug.Log("No lobbies available for quickjoin");
			return null;
		}
	}
	private async Task<Lobby> CreateLobby()
	{
		try
		{
			const int maxPlayers = 2;

			var a = await RelayService.Instance.CreateAllocationAsync(maxPlayers);
			var joinCode = await RelayService.Instance.GetJoinCodeAsync(a.AllocationId);

			var options = new CreateLobbyOptions()
			{
				Data = new Dictionary<string, DataObject> { { _joinCodeKey, new DataObject(DataObject.VisibilityOptions.Public, joinCode) } }
			};
			var lobby = await Lobbies.Instance.CreateLobbyAsync("Useless Lobby Name", maxPlayers, options);

			StartCoroutine(HeartbeatLobbyCoroutine(lobby.Id, 15));

			_transport.SetHostRelayData(a.RelayServer.IpV4, (ushort)a.RelayServer.Port, a.AllocationIdBytes, a.Key, a.ConnectionData);

			NetworkManager.Singleton.StartHost();
			return lobby;
		}
		catch(Exception)
		{
			Debug.Log("Failed creating a lobby");
			return null;
		}
	}
	private void SetTransformAsClient(JoinAllocation a)
	{
		_transport.SetClientRelayData(a.RelayServer.IpV4, (ushort)a.RelayServer.Port, a.AllocationIdBytes, a.Key, a.ConnectionData, a.HostConnectionData);
	}
	private static IEnumerator HeartbeatLobbyCoroutine(string lobbyId, float waitTimeSeconds)
	{
		var delay = new WaitForSecondsRealtime(waitTimeSeconds);
		while(true)
		{
			Lobbies.Instance.SendHeartbeatPingAsync(lobbyId);
			yield return delay;
		}
	}
	private void OnDestroy()
	{
		StopAllCoroutines();
		if(_connectedLobby != null)
		{
			if(_connectedLobby.HostId == _playerID)
			{
				Lobbies.Instance.DeleteLobbyAsync(_connectedLobby.Id);
			}
			else
			{
				Lobbies.Instance.RemovePlayerAsync(_connectedLobby.Id, _playerID);
			}
		}
	}
}
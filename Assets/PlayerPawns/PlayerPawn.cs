using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Wizard), typeof(CharacterController))]
[RequireComponent(typeof(PlayerNetwork), typeof(NetworkObject))]
public class PlayerPawn : BasePawn
{
	private Wizard Wizard = null;

	private PlayerInput _playerInput = null;
	private bool _isMoving = false;
	private bool _isCasting = false;

	public override void OnNetworkSpawn()
	{
		base.OnNetworkSpawn();

		if (IsOwner)
		{
			Wizard = GetComponent<Wizard>();
			_playerInput = GetComponent<PlayerInput>();
			if (_playerInput)
				SetupInputActions();
			else
				Debug.LogError("PLAYERPAWN - ONNETWORKSPAWN PLAYERINPUT COMPONENT MISSING");
		}
	}
	private void SetupInputActions()
	{
		_playerInput.actions.FindAction("Move").performed += context => _isMoving = true;
		_playerInput.actions.FindAction("Move").canceled += context => _isMoving = false;
		if (Wizard)
		{
			_playerInput.actions.FindAction("PrimaryCast").performed += context => _isCasting = true;
			_playerInput.actions.FindAction("PrimaryCast").performed += context => Wizard.CastSpellByIndex(0);
			_playerInput.actions.FindAction("PrimaryCast").canceled += context => _isCasting = false;
			_playerInput.actions.FindAction("SecondaryCast").performed += context => _isCasting = true;
			_playerInput.actions.FindAction("SecondaryCast").performed += context => Wizard.CastSpellByIndex(1);
			_playerInput.actions.FindAction("SecondaryCast").canceled += context => _isCasting = false;
		}
	}
	public override bool HasReceivedActionInput()
	{
		return _isCasting;
	}
	public override bool HasReceivedMovementInput()
	{
		return _isMoving;
	}
	public override Vector2 GetMovementInput()
	{
		return _playerInput.actions.FindAction("Move").ReadValue<Vector2>();
	}
}

using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Wizard), typeof(CharacterController))]
public class PlayerPawn : BasePawn
{
	private CharacterController _controller;
	public CharacterController Controller { get { return _controller; } }

	public Wizard Wizard;

	private PlayerInput _playerInput;
	private bool _isMoving = false;
	private bool _isCasting = false;

	private void Start()
	{
		_controller = GetComponent<CharacterController>();
		Wizard = GetComponent<Wizard>();

		SetupInputActions();
	}
	private void SetupInputActions()
	{
		_playerInput = GetComponent<PlayerInput>();
		_playerInput.actions.FindAction("Move").performed += context => _isMoving = true;
		_playerInput.actions.FindAction("Move").canceled += context => _isMoving = false;
		_playerInput.actions.FindAction("PrimaryCast").performed += context => _isCasting = true;
		_playerInput.actions.FindAction("PrimaryCast").performed += context => Wizard.SetSpell(0);
		_playerInput.actions.FindAction("PrimaryCast").canceled += context => _isCasting = false;
		_playerInput.actions.FindAction("SecondaryCast").performed += context => _isCasting = true;
		_playerInput.actions.FindAction("SecondaryCast").performed += context => Wizard.SetSpell(1);
		_playerInput.actions.FindAction("SecondaryCast").canceled += context => _isCasting = false;
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

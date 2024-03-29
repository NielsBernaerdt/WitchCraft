using UnityEngine;
using Unity.Netcode;

public abstract class BasePawn : NetworkBehaviour
{
	private BaseState _state;
	[SerializeField] private string _stateName;

	private void Awake()
	{
		_state = new Idle(this);
	}
	private void Update()
	{
		BaseState state = _state.Update();
		if (state != null)
		{
			_state.OnExit();
			_state = state;
			_stateName = _state.ToString();
			_state.OnEnter();
		}
	}
	public abstract bool HasReceivedActionInput();
	public abstract bool HasReceivedMovementInput();
	public abstract Vector2 GetMovementInput();
	public int GetCurrentAnimationState()
	{
		return _state.GetAnimation();
	}
}
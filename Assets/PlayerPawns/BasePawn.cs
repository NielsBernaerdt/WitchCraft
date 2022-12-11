using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BasePawn : MonoBehaviour
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
	public virtual bool HasReceivedActionInput()
	{
		Debug.LogError("BasePawn - HasReceivedActionInput not using override");
		return false; 
	}
	public virtual bool HasReceivedMovementInput()
	{
		Debug.LogError("BasePawn - HasReceivedMovementInput not using override");
		return false;
	}
	public virtual Vector2 GetMovementInput()
	{
		Debug.LogError("BasePawn - GetMovementInput not using override");
		return new Vector2();
	}
}
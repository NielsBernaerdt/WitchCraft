using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CastSpell : BaseState
{
	private Wizard _wizardComponent = null;
	public CastSpell(BasePawn pawn) { _pawn = pawn; }
	public override BaseState Update()
	{
		if (_wizardComponent == null)
			return new Idle(_pawn);

		if (_wizardComponent.IsCasting == false)
			return new Idle(_pawn);

		return null;
	}
	public override void OnEnter()
	{
		_wizardComponent = _pawn.GetComponent<Wizard>();
		if (_wizardComponent)
		{
			Vector2 mousePosWorld = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
			_wizardComponent.StartCasting(mousePosWorld);
		}
	}
	public override void OnExit() { }
}
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
	public override void OnEnter() { }
	public override void OnExit() { }
}
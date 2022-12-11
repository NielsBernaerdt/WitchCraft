using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Idle : BaseState
{
	private float _accTime;
	private float _idleDuration = 2f;
	public Idle(BasePawn pawn) { _pawn = pawn; }
	public override BaseState Update()
	{
		if(_pawn is EnemyPawn)
		{
			return NPCIdle();
		}
		if(_pawn is PlayerPawn)
		{
			return PlayerIdle();
		}

		return null;
	}
	public override void OnEnter()
	{
		_accTime = 0;
	}
	public override void OnExit() { }
	private BaseState PlayerIdle()
	{
		if (_pawn.HasReceivedActionInput() == true)
			return new CastSpell(_pawn);

		if (_pawn.HasReceivedMovementInput() == true)
			return new Move(_pawn);
		return null;
	}
	private BaseState NPCIdle()
	{
		return null;
	}
}
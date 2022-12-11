using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseState
{
	protected BasePawn _pawn;
	public abstract BaseState Update();
	public abstract void OnEnter();
	public abstract void OnExit();
}
using UnityEngine;

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
	}
	public override void OnExit() { }

	public override int GetAnimation()
	{
		return Animator.StringToHash("casting");
    }
}
using UnityEngine;

public class Move : BaseState
{
	CharacterController _controller;
	float _movSpeed = 10f;
	public Move(BasePawn pawn) { _pawn = pawn; }
	public override BaseState Update()
	{
		if (_pawn.HasReceivedActionInput() == true)
			return new CastSpell(_pawn);

		Vector2 movementInput = _pawn.GetMovementInput();
		if (movementInput.magnitude < float.Epsilon)
			return new Idle(_pawn);
		_controller.Move(movementInput * Time.deltaTime * _movSpeed);
		return null;
	}
	public override void OnEnter()
	{
		_controller = _pawn.GetComponent<CharacterController>();
	}
	public override void OnExit() { }
}
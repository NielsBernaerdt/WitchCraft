using UnityEngine;

public class Move : BaseState
{
	Rigidbody2D _rigidBody;
	float _movSpeed = 15f;
	public Move(BasePawn pawn) { _pawn = pawn; }
	public override BaseState Update()
	{
		if (_pawn.HasReceivedActionInput() == true)
			return new CastSpell(_pawn);

		Vector2 movementInput = _pawn.GetMovementInput();
		if (movementInput.magnitude < float.Epsilon)
			return new Idle(_pawn);
		_rigidBody.MovePosition(_rigidBody.position + movementInput * Time.deltaTime * _movSpeed);
		return null;
	}
	public override void OnEnter()
	{
		_rigidBody = _pawn.GetComponent<Rigidbody2D>();
	}
	public override void OnExit() { }

	public override int GetAnimation()
	{
		return Animator.StringToHash("walk");
    }
}
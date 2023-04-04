using UnityEngine;

public class Wander : BaseState
{
	private float _wanderRadius = 5f;
	private Vector2 _target;
	private float _movSpeed = 10f;
	public Wander(BasePawn pawn) { _pawn = pawn; }
	public override BaseState Update()
	{
		_pawn.transform.position = Vector2.MoveTowards(_pawn.transform.position, _target, _movSpeed * Time.deltaTime);
		if (Vector2.Distance(_pawn.transform.position, _target) <= Mathf.Epsilon)
			return new Idle(_pawn);

		return null;
	}
	public override void OnEnter()
	{
		float theta = Random.Range(0, 360) * Mathf.PI / 180;
		_target.x = _pawn.transform.position.x + Random.Range(0, _wanderRadius) * Mathf.Cos(theta);
		_target.y = _pawn.transform.position.y + Random.Range(0, _wanderRadius) * Mathf.Sin(theta);
	}
	public override void OnExit() { }

	public override int GetAnimation()
	{
		return Animator.StringToHash("walk");
    }
}
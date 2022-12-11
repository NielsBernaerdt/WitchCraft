using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : BaseSpell
{
	private Vector2 _velocity;
	private float _movSpeed = 10f;
	private void Awake()
	{
		_castDuration = 0.5f;
	}
	private void FixedUpdate()
	{
		Vector2 currentPosition = transform.position;
		Vector2 movement = currentPosition + (_velocity * Time.deltaTime * _movSpeed);
		transform.SetPositionAndRotation(movement, Quaternion.identity);
	}
	public override void Execute(Vector2 targetPosition)
	{
		Vector2 currentPosition = transform.position;
		_velocity = targetPosition - currentPosition;
		_velocity.Normalize();
	}
}
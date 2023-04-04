using System.Collections.Generic;
using UnityEngine;

public class FireBall : BaseSpell
{
	private Vector2 _velocity;
	private float _movSpeed = 10f;

	private float _accTime = 0f;
	private float _lifeTime = 2f;

	private const float _hitCollisionDelay = 0.125f;

	private void FixedUpdate()
	{
		_accTime += Time.deltaTime;
		if(_accTime >= _lifeTime)
			Destroy(gameObject);

		Vector2 currentPosition = transform.position;
		Vector2 movement = currentPosition + (_velocity * Time.deltaTime * _movSpeed);
		transform.SetPositionAndRotation(movement, Quaternion.identity);
		Vector3 rotation = new Vector3(0, 0, Mathf.Atan2(movement.y - currentPosition.y, movement.x - currentPosition.x) * Mathf.Rad2Deg);
		transform.Rotate(rotation);
	}
	public override void Execute(Vector2 targetPosition)
	{
		Vector2 currentPosition = transform.position;
		_velocity = targetPosition - currentPosition;
		_velocity.Normalize();
	}
	public override List<SpawnInfo> GetSpawnInfoNextSpell()
	{
		List<SpawnInfo> list = new List<SpawnInfo>
		{
			new SpawnInfo(transform.position, _velocity)
		};
		return list;
	}
    public override void OnTriggerEnter2D(Collider2D collision)
    {
		if (_accTime <= _hitCollisionDelay)
			return;

        Destroy(collision.gameObject);
        Destroy(gameObject);
        Debug.Log("this SHOULD run");
    }
}
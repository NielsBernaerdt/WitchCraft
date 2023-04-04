using System.Collections.Generic;
using UnityEngine;

public class Vortex : BaseSpell
{
	private float _scaleSpeed = 3f;
	private float _maxScale = 4f;
	private float _rotationSpeed = -120f;

	private void FixedUpdate()
	{
		if (transform.localScale.x >= _maxScale)
			Destroy(gameObject);
		float currentScaling = _scaleSpeed * Time.deltaTime;
		transform.localScale += new Vector3(currentScaling, currentScaling, 0);
		transform.Rotate(new Vector3(0, 0, 1), transform.rotation.z + _rotationSpeed * Time.deltaTime);
	}
	public override void Execute(Vector2 targetPosition)
	{
		transform.SetPositionAndRotation(targetPosition, Quaternion.identity);
	}
	public override List<SpawnInfo> GetSpawnInfoNextSpell()
	{
		List<SpawnInfo> list = new List<SpawnInfo>
		{
			new SpawnInfo(transform.position, new Vector2( 1,  0)),
			new SpawnInfo(transform.position, new Vector2(-1,  0)),
			new SpawnInfo(transform.position, new Vector2( 0,  1)),
			new SpawnInfo(transform.position, new Vector2( 0, -1))
		};
		return list;
	}
}
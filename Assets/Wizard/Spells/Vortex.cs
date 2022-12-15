using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vortex : BaseSpell
{
	private float _scaleSpeed = 0.3f;
	private float _maxScale = 4f;
	private void FixedUpdate()
	{
		if (transform.localScale.x >= _maxScale)
			Destroy(gameObject);
		float currentScaling = _scaleSpeed * Time.deltaTime;
		transform.localScale += new Vector3(_scaleSpeed, _scaleSpeed, 0);
	}
	public override void Execute(Vector2 targetPosition)
	{
		transform.SetPositionAndRotation(targetPosition, Quaternion.identity);
	}
}
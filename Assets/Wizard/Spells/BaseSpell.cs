using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseSpell : MonoBehaviour
{
	public virtual void Execute(Vector2 targetPosition) { }

	private void OnTriggerEnter2D(Collider2D collision)
	{
		Destroy(collision.gameObject);
		Destroy(gameObject);
	}
}

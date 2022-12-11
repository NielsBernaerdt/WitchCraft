using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseSpell : MonoBehaviour
{
	protected float _castDuration = 1f;
	public float CastDuration { get { return _castDuration; } }
	public virtual void Execute(Vector2 targetPosition) { }
}

using System.Collections.Generic;
using UnityEngine;

public class BaseSpell : MonoBehaviour
{
	[SerializeField] protected float _spellCost = 0f;
	public float SpellCost { get => _spellCost; }
	public struct SpawnInfo
	{
		public Vector2 SpawnLocation;
		public Vector2 SpawnDirection;
		public SpawnInfo(Vector2 spawnLocation, Vector2 spawnDirection)
		{
			SpawnLocation = spawnLocation;
			SpawnDirection= spawnDirection;
		}
	}
	public virtual void Execute(Vector2 targetPosition) { }
	public virtual List<SpawnInfo> GetSpawnInfoNextSpell() { return null; }

	public virtual void OnTriggerEnter2D(Collider2D collision)
	{
		Destroy(collision.gameObject);
		Destroy(gameObject);
		Debug.Log("this should NOT run");
	}
}

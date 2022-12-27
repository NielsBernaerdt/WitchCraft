using System.Collections.Generic;
using UnityEngine;
using static BaseSpell;

public class CombinedSpell : MonoBehaviour
{
	private Queue<BaseSpell> _spellQueue = new ();
	private BaseSpell _currentSpell;
	private List<SpawnInfo> _spawnInfoList = new ();
	private bool _isExecuted = false;
	public void AddSpell(BaseSpell spell)
	{
		_spellQueue.Enqueue(spell);
	}
	public void SetSpells(Queue<BaseSpell> list)
	{
		_spellQueue = list;
	}
	public void ExecuteSpell(Vector2 targetSpellLocation)
	{
		_currentSpell = Instantiate(_spellQueue.Dequeue(), transform.position, Quaternion.identity);
		_currentSpell.Execute(targetSpellLocation);
		_isExecuted = true;
	}
	private void FixedUpdate()
	{
		if (_isExecuted == false)
			return;

		if (_currentSpell != null)
		{
			transform.position = _currentSpell.transform.position;
			_spawnInfoList = _currentSpell.GetSpawnInfoNextSpell();
			return;
		}

		if (_currentSpell == null
		&& _spellQueue != null
		&& _spellQueue.Count > 0)
		{
			SpawnNextSpell();
			Destroy(gameObject);
		}
		else
		{
			Destroy(gameObject);
		}
	}
	private void SpawnNextSpell()
	// Spawn the next spell in the queue
	// nr of spells & positions depend on the _spawnInfoList
	{
		foreach (var spawnInfo in _spawnInfoList)
		{
			var Queue = new Queue<BaseSpell>(_spellQueue); //Otherwise, the next iteration will have an empty queue
			var newCombinedSpell = Instantiate<CombinedSpell>(this, spawnInfo.SpawnLocation, Quaternion.identity);
			newCombinedSpell.SetSpells(Queue);
			newCombinedSpell.ExecuteSpell((Vector2)transform.position + spawnInfo.SpawnDirection);
		}
	}
}
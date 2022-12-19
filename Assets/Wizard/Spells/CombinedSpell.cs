using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using static BaseSpell;

public class CombinedSpell : MonoBehaviour
{
	private Queue<BaseSpell> _spellQueue = new Queue<BaseSpell>();
	private BaseSpell _currentSpell;
	private List<SpawnInfo> _spawnInfoList = new List<SpawnInfo>();
	public void ConstructSpell(BaseSpell spell)
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
	}
	private void FixedUpdate()
	{
		if (_currentSpell != null)
		{
			transform.position = _currentSpell.transform.position;
			_spawnInfoList = _currentSpell.GetSpawnOriginAndDirection();
			return;
		}

		if (_currentSpell == null
		&& _spellQueue != null
		&& _spellQueue.Count > 0)
		{
			foreach (var spawnInfo in _spawnInfoList)
			{
				var Queue = new Queue<BaseSpell>(_spellQueue);
				var newCombinedSpell = Instantiate<CombinedSpell>(this, spawnInfo.SpawnLocation, Quaternion.identity);
				newCombinedSpell.SetSpells(Queue);
				newCombinedSpell.ExecuteSpell((Vector2)transform.position + spawnInfo.SpawnDirection);
			}
			Destroy(gameObject);
		}
		else
		{
			Destroy(gameObject);
		}
	}
}

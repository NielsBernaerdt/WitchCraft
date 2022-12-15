using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CombinedSpell : MonoBehaviour
{
	private Queue<BaseSpell> _spellQueue;
	private BaseSpell _currentSpell;
	private Vector2 _spellLocation;
	bool _startFirstCast = false;
	public void ConstructSpell(BaseSpell spell)
	{
		if(_spellQueue == null)
		{
			_spellQueue = new Queue<BaseSpell>();
		}
		_spellQueue.Enqueue(spell);
	}
	private void FixedUpdate()
	{
		if (_startFirstCast == false) return;
		if (_currentSpell != null)
		{
			_spellLocation = _currentSpell.transform.position;
			transform.position = _spellLocation;
			return;
		}

		if (_spellQueue != null
		&& _spellQueue.Count > 0)
		{
			_currentSpell = Instantiate(_spellQueue.Dequeue(), transform.position, Quaternion.identity);
			_currentSpell.Execute(_spellLocation);
		}
		else
		{
			Destroy(gameObject);
		}
	}
	public void ExecuteSpell(Vector2 targetSpellLocation)
	{
		_spellLocation = targetSpellLocation;
		_startFirstCast = true;
	}
}

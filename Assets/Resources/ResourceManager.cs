using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ResourceManager
{
	public float _currentElixir = 0f;
	private float _maxElixir = 100f;
	private const float _elixirGrowth = 3f;
	
	public ResourceManager()
	{
		_currentElixir = _maxElixir;
	}

	public void RechargeElixir()
	{
		Debug.Log("Current elixir value: " + _currentElixir);
		if(_currentElixir < _maxElixir)
		{
			_currentElixir += _elixirGrowth * Time.deltaTime;
		}
	}
	public bool CastSpell(float cost)
	{
		if(cost <= _currentElixir)
		{
			_currentElixir -= cost;
			return true;
		}
		return false;
	}
}
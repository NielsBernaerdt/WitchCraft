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
		if(_currentElixir < _maxElixir)
		{
			_currentElixir += _elixirGrowth * Time.deltaTime;
			GlobalEventHandler.Instance?.InvokeOnElixirGenerated(this, (int)_currentElixir);
		}
	}
	public bool CanPaySpell(float cost)
	{
		if(cost <= _currentElixir)
		{
			return true;
		}
		return false;
	}
	public void PaySpell(float cost)
	{
		_currentElixir -= cost;
	}
}
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Wizard : MonoBehaviour
{
	[SerializeField] private CombinedSpell _combinedSpellPrefab;
	[SerializeField] private List<BaseSpell> Spells = new List<BaseSpell>();
	[SerializeField] private ResourceManager _resourceManager = new ResourceManager();

	public bool IsCasting = false;

	private float _accTime = 0f;
	private float _castDuration = 0.5f;
	private Vector2 _targetSpellPosition;
	private CombinedSpell _combinedSpell;
	private void Update()
	{
		if (IsCasting == true
			&& _accTime >= _castDuration)
		{
			IsCasting = false;
			_combinedSpell.gameObject.SetActive(true);
			_combinedSpell.ExecuteSpell(_targetSpellPosition);
		}

		_accTime += Time.deltaTime;
		_resourceManager.RechargeElixir();
	}
	public void StartCasting(Vector2 targetPosition)
		// Gets called when the spell gets constructed
	{
		IsCasting = true;
		_targetSpellPosition = targetPosition;
		_accTime = 0f;
	}
	public void SetSpell(int index)
		// Gets called via input
		// Creates a CombinedSpell object
		// & fills its spellqueue
	{
		if (_combinedSpell == null)
		{
			_combinedSpell = Instantiate(_combinedSpellPrefab, transform.position, Quaternion.identity);
			_combinedSpell.gameObject.SetActive(false);

			Vector2 mousePosWorld = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
			StartCasting(mousePosWorld);
		}

		BaseSpell chosenSpell = Spells[index];
		if(_resourceManager.CastSpell(chosenSpell.SpellCost))
		{
			_combinedSpell.ConstructSpell(chosenSpell);
		}
	}
}
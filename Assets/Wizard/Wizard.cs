using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static WizardNetwork;

[RequireComponent(typeof(WizardNetwork))]
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
	private int[] _testIndices = new int[0];

	private WizardNetwork _wizardNetwork;

	private void Awake()
	{
		_wizardNetwork= GetComponent<WizardNetwork>();
	}
	private void Update()
	{
		if (IsCasting == true
			&& _accTime >= _castDuration)
		{
			IsCasting = false;
			_combinedSpell.ExecuteSpell(_targetSpellPosition);


			SpellNetworkData test = new()
			{
				_spellIdArray = _testIndices,
				Position = _combinedSpell.transform.position,
				TargetPosition = _targetSpellPosition
			};
			_wizardNetwork.RequestCastServerRpc(test);
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

			Vector2 mousePosWorld = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
			StartCasting(mousePosWorld);
		}

		BaseSpell chosenSpell = Spells[index];
		if(_resourceManager.CanPaySpell(chosenSpell.SpellCost))
		{
			_resourceManager.PaySpell(chosenSpell.SpellCost);
			_combinedSpell.ConstructSpell(chosenSpell);

			int[] tempArray = new int[_testIndices.Length+1];
			Array.Copy(_testIndices, tempArray, _testIndices.Length);
			tempArray[tempArray.Length - 1] = index;
			_testIndices = tempArray;
		}
	}

	//////////////////
	///
	public void SpawnSpell(SpellNetworkData test)
	{
		_combinedSpell = Instantiate(_combinedSpellPrefab, test.Position, Quaternion.identity);

		for (int i = 0; i < test._spellIdArray.Length; ++i)
		{
			_combinedSpell.ConstructSpell(Spells[test._spellIdArray[i]]);
		}

		_combinedSpell.ExecuteSpell(test.TargetPosition);
	}
}
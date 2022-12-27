using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static WizardNetwork;

[RequireComponent(typeof(WizardNetwork))]
public class Wizard : MonoBehaviour
{
	[SerializeField] private CombinedSpell _combinedSpellPrefab;
	[SerializeField] private List<BaseSpell> Spells = new ();
	[SerializeField] private ResourceManager _resourceManager = new ();

	public bool IsCasting = false;

	private float _accTime = 0f;
	private float _castDuration = 0.5f;
	private Vector2 _targetSpellPosition;
	private CombinedSpell _combinedSpell;
	private List<int> _spellIndices = new ();

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
			ExecuteSpellLocal();
			ExecuteSpellClients();
			_spellIndices.Clear();
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
	public void CastSpellByIndex(int index)
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
		if(_resourceManager.CanCastSpell(chosenSpell.SpellCost))
		{
			_resourceManager.PaySpell(chosenSpell.SpellCost);
			_spellIndices.Add(index);
		}
	}
	private Queue<BaseSpell> ConstructSpellQueue()
	{
		Queue<BaseSpell> spellQueue = new();
		foreach(int index in _spellIndices)
		{
			spellQueue.Enqueue(Spells[index]);
		}
		return spellQueue;
	}
	private void ExecuteSpellLocal()
	{
		_combinedSpell.SetSpells(ConstructSpellQueue());
		_combinedSpell.ExecuteSpell(_targetSpellPosition);
	}
	private void ExecuteSpellClients()
	{
		SpellNetworkData test = new()
		{
			_spellIdArray = _spellIndices.ToArray(),
			Position = _combinedSpell.transform.position,
			TargetPosition = _targetSpellPosition
		};
		_wizardNetwork.RequestSpellCastServerRpc(test);
	}
	public void SpawnSpellClients(SpellNetworkData test)
	{
		_combinedSpell = Instantiate(_combinedSpellPrefab, test.Position, Quaternion.identity);
		_combinedSpell.SetSpells(ConstructSpellQueue());
		for (int i = 0; i < test._spellIdArray.Length; ++i)
		{
			_combinedSpell.AddSpell(Spells[test._spellIdArray[i]]);
		}
		_combinedSpell.ExecuteSpell(test.TargetPosition);
	}
}
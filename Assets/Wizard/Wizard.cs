using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Wizard : MonoBehaviour
{
	[SerializeField] private CombinedSpell _combinedSpellPrefab;
	[SerializeField] private List<BaseSpell> Spells = new List<BaseSpell>();

	public bool IsCasting = false;

	private float _accTime = 0f;
	private float _castDuration = 0.5f;
	private Vector2 _targetSpellPosition;
	private CombinedSpell _combinedSpell;
	private BasePawn _pawn;

	private void Awake()
	{
		_pawn = GetComponent<BasePawn>();
	}
	private void Update()
	{
		if (IsCasting == true
			&& _accTime >= _castDuration)
		{
			IsCasting = false;
			_combinedSpell.ExecuteSpell(_targetSpellPosition);
		}

		_accTime += Time.deltaTime;
	}
	public void StartCasting(Vector2 targetPosition)
	{
		IsCasting = true;
		_targetSpellPosition = targetPosition;
		_accTime = 0f;
	}
	public bool ReceivedCastInput()
	{
		return _pawn.HasReceivedActionInput();
	}
	public void SetSpell(int index)
	{
		if (IsCasting == false)
		{
			_combinedSpell = Instantiate(_combinedSpellPrefab, transform.position, Quaternion.identity);
		}
		_combinedSpell.ConstructSpell(Spells[index]);
	}
}
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Wizard : MonoBehaviour
{
	public bool IsCasting = false;
	public List<BaseSpell> Spells = new List<BaseSpell>();
	private float _accTime = 0f;
	private Vector2 _targetSpellPosition;
	private BaseSpell _castedSpell;
	private BasePawn _pawn;
	private void Awake()
	{
		_pawn = GetComponent<BasePawn>();
	}
	private void Update()
	{
		if (IsCasting == true
			&& _accTime >= _castedSpell.CastDuration)
		{
			IsCasting = false;
			ExecuteSpell(Spells.Random());
		}

		_accTime += Time.deltaTime;
	}
	public void StartCasting(Vector2 targetPosition)
	{
		IsCasting = true;
		_targetSpellPosition = targetPosition;
		_accTime = 0f;
	}
	private void ExecuteSpell(BaseSpell spell)
	{
		if (spell != null)
		{
			BaseSpell executedSpell = Instantiate(_castedSpell, transform.position, Quaternion.identity);
			executedSpell.Execute(_targetSpellPosition);
		}
	}
	public bool ReceivedCastInput()
	{
		return _pawn.HasReceivedActionInput();
	}
	public void SetSpell(int index)
	{
		_castedSpell = Spells[index];
	}
}
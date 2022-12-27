using Unity.Netcode;
using UnityEngine;

public class WizardNetwork : NetworkBehaviour
{
	[ServerRpc]
	public void RequestSpellCastServerRpc(SpellNetworkData test)
	{
		SpellCastClientRpc(test);
	}

	[ClientRpc]
	private void SpellCastClientRpc(SpellNetworkData test)
	{
		ExecuteSpellCast(test);
	}

	private void ExecuteSpellCast(SpellNetworkData test)
	{
		if(!IsOwner) GetComponent<Wizard>().SpawnSpellClients(test);
	}

	public struct SpellNetworkData : INetworkSerializable
	{
		private int[] _spellIds;
		private float _posX;
		private float _posY;
		private float _targetPosX;
		private float _targetPosY;
		public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
		{
			serializer.SerializeValue(ref _spellIds);
			serializer.SerializeValue(ref _posX);
			serializer.SerializeValue(ref _posY);
			serializer.SerializeValue(ref _targetPosX);
			serializer.SerializeValue(ref _targetPosY);
		}
		public int[] _spellIdArray
		{
			get
			{
				int[] temp;
				temp = _spellIds;
				return temp;
			}
			set
			{
				_spellIds = value;
			}
		}
		public Vector2 Position
		{
			get => new Vector2(_posX, _posY);
			set
			{
				_posX = value.x;
				_posY = value.y;
			}
		}
		public Vector2 TargetPosition
		{
			get => new Vector2(_targetPosX, _targetPosY);
			set
			{
				_targetPosX = value.x;
				_targetPosY = value.y;
			}
		}
	}
}

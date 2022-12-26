using Unity.Netcode;
using UnityEngine;

public class PlayerNetwork : NetworkBehaviour
{
	private readonly NetworkVariable<PlayerNetworkData> _networkData = new NetworkVariable<PlayerNetworkData>(writePerm: NetworkVariableWritePermission.Owner);

	private void Update()
	{
		if(IsOwner)
		{
			_networkData.Value = new PlayerNetworkData
			{
				Position = new Vector2(transform.position.x, transform.position.y)
			};
		}
		else
		{
			//TODO interpolate otherwise the movement on clients is not smooth
			transform.position = new Vector3(_networkData.Value.Position.x, _networkData.Value.Position.y, 0);
		}
	}

	struct PlayerNetworkData : INetworkSerializable
	{
		private float _posX;
		private float _posY;
		public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
		{
			serializer.SerializeValue(ref _posX);
			serializer.SerializeValue(ref _posY);
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
	}
}
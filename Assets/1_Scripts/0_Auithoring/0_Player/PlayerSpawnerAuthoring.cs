using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public class PlayerSpawnerAuthoring : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _rotationSpeed;
    [SerializeField] private float3 _spawnPos;
    [SerializeField] private GameObject _playerPrefab;

    class Baker : Baker<PlayerSpawnerAuthoring> {
        public override void Bake(PlayerSpawnerAuthoring authoring) {
            Entity entity = GetEntity(TransformUsageFlags.None);
            AddComponent(entity, new PlayerSpawner {
                playerPrefb = GetEntity(authoring._playerPrefab, TransformUsageFlags.Dynamic),
                SpawnPos = authoring._spawnPos,
                Speed = authoring._speed,
                RotationSPeed = authoring._rotationSpeed,
            });

        }
    }
}

public struct PlayerSpawner: IComponentData {
    public Entity playerPrefb;
    public float3 SpawnPos;
    public float Speed;
    public float RotationSPeed;
}
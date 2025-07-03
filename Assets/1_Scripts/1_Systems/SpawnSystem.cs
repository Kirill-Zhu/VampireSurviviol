using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;
using Unity.Mathematics;


partial struct EnemySpawnSystem : ISystem
{
    public float MinMoveSpeed;
    public float MaxMoveSpeed;

    quaternion _playerRotation;
    float3 _playerPosition;


    public void OnCreate(ref SystemState state) {

      
    }
   
    public void OnUpdate(ref SystemState state)
    {
    SystemAPI.TryGetSingleton<EntitiesReferences>(out EntitiesReferences entitiesReferences);
        //Get PlayerPos

        foreach (RefRO<LocalTransform> localTransform in SystemAPI.Query<RefRO<LocalTransform>>().WithAll<PlayerInput>()) {
            _playerPosition = localTransform.ValueRO.Position;
            _playerRotation = localTransform.ValueRO.Rotation;
        }

        //Get Random Value

        //Enemy Spawn
        foreach (
            RefRO<EnemySpawner> spawner
            in
            SystemAPI.Query<
            RefRO<EnemySpawner>
            >()) {

            if (Input.GetKey(KeyCode.Space)) {

                Debug.Log("Spawn enemy");
                Entity enemy = state.EntityManager.Instantiate(entitiesReferences.EnemyPrefab);
                SystemAPI.SetComponent(enemy, LocalTransform.FromPosition(spawner.ValueRO.SpawnPos));
                SystemAPI.SetComponent(enemy, UnitMover.SetSpeed(UnityEngine.Random.Range(8, 10), 5));

            }
            if (Input.GetKey(KeyCode.LeftShift)) {
                Entity bullet = state.EntityManager.Instantiate(entitiesReferences.Bullet);

                SystemAPI.SetComponent(bullet, LocalTransform.FromPositionRotation(_playerPosition, _playerRotation));


            }
        }

    }

    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    {
        
    }
}

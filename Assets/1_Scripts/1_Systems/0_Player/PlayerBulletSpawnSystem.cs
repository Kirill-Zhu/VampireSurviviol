using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

partial struct PlayerBulletSpawnSystem : ISystem {
    quaternion _playerRotation;
    float3 _playerPosition;
    LocalTransform playerLocalTransform;

    private double _elapsedTime;
    private double _lastUpdatedTime;

    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {

        SystemAPI.TryGetSingleton<ControlledGun>(out ControlledGun PlayerControleldGun);
        //Get PlayerPos

        foreach (RefRO<LocalTransform> localTransform in SystemAPI.Query<RefRO<LocalTransform>>().WithAll<PlayerInput>()) {
            _playerPosition = localTransform.ValueRO.Position;
            _playerRotation = localTransform.ValueRO.Rotation;
            playerLocalTransform = localTransform.ValueRO;
        }
        _elapsedTime = SystemAPI.Time.ElapsedTime;

        foreach( (var playerInput, 
            var playerGun, 
            var localTransform) 
            in 
            SystemAPI.Query<
                RefRO<PlayerInput>,
                RefRO<ControlledGun>,
                RefRO<LocalTransform>
            >()) {

            if (playerGun.ValueRO.IsSootingButtonDown) {

                if (_elapsedTime - _lastUpdatedTime < PlayerControleldGun.RateOfFire) {
                    break;
                }
                _lastUpdatedTime = _elapsedTime;

                Entity bullet = state.EntityManager.Instantiate(playerGun.ValueRO.BulletPrefab);
                SystemAPI.SetComponent(bullet, LocalTransform.FromPositionRotation(localTransform.ValueRO.Position, localTransform.ValueRO.Rotation));


            }
        }

       
    }

    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    {
        
    }
}

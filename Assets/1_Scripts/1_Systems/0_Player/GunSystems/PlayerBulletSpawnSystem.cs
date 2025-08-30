using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;


partial struct PlayerBulletSpawnSystem : ISystem {
    //quaternion _playerRotation;
    //float3 _playerPosition;
    //LocalTransform playerLocalTransform;

    private double _elapsedTime;
    private double _lastUpdatedTime;
    private Random _random;
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        _random = new Random((uint)System.DateTime.Now.Ticks);
    }

    
    public void OnUpdate(ref SystemState state)
    {

        SystemAPI.TryGetSingleton<ControlledGun>(out ControlledGun PlayerControleldGun);
        //Get PlayerPos

        //foreach (RefRO<LocalTransform> localTransform in SystemAPI.Query<RefRO<LocalTransform>>().WithAll<PlayerInput>()) {
        //    _playerPosition = localTransform.ValueRO.Position;
        //    _playerRotation = localTransform.ValueRO.Rotation;
        //    playerLocalTransform = localTransform.ValueRO;
        //}
        _elapsedTime = SystemAPI.Time.ElapsedTime;

        foreach( (var playerInput, 
            var playerGun, 
            var localTransform,
            Entity entity) 
            in 
            SystemAPI.Query<
                RefRO<PlayerInput>,
                RefRO<ControlledGun>,
                RefRW<LocalTransform>
            >().WithEntityAccess()) {

            if (playerGun.ValueRO.IsSootingButtonDown) {
                switch (PlayerControleldGun.gunType) {
                    case GunType.Pistol: {
                            //RateOfFire Check
                            if (_elapsedTime - _lastUpdatedTime < PlayerControleldGun.RateOfFire) {
                                break;
                            }
                            _lastUpdatedTime = _elapsedTime;
                            // Spawn Bullets

                            Entity bullet = state.EntityManager.Instantiate(playerGun.ValueRO.BulletPrefab);
                            SystemAPI.SetComponent(bullet, LocalTransform.FromPositionRotation(localTransform.ValueRO.Position, localTransform.ValueRO.Rotation));
                            SystemAPI.SetComponent(bullet, new PlayerProjectile { Damage = playerGun.ValueRO.Damage });
                            PlayerAudioController.Instance.Shoot(PlayerControleldGun.gunType);
                            SkillsMonobehAnimations.Instance.Shoot(PlayerControleldGun.gunType);
                            break;
                    }
                    case GunType.Shootgun: {  //RateOfFire Check
                            if (_elapsedTime - _lastUpdatedTime < PlayerControleldGun.RateOfFire) {
                                break;
                            }
                            _lastUpdatedTime = _elapsedTime;
                            // Spawn Bullets
                            for (int i =0; i < 20; i++) {
                                Entity bullet = state.EntityManager.Instantiate(playerGun.ValueRO.BulletPrefab);
                                SystemAPI.SetComponent(bullet, LocalTransform.FromPositionRotation(localTransform.ValueRO.Position,
                                    new quaternion(localTransform.ValueRO.Rotation.value.x + _random.NextFloat(0, 0.0f),  
                                    localTransform.ValueRO.Rotation.value.y + _random.NextFloat(0, 0.1f), 
                                    localTransform.ValueRO.Rotation.value.z + _random.NextFloat(0, 0.0f), 
                                    localTransform.ValueRO.Rotation.value.w + _random.NextFloat(0, 0.1f))));
                                SystemAPI.SetComponent(bullet, new PlayerProjectile { Damage = playerGun.ValueRO.Damage });
                            }
                           
                            PlayerAudioController.Instance.Shoot(PlayerControleldGun.gunType);
                            SkillsMonobehAnimations.Instance.Shoot(PlayerControleldGun.gunType);
                            break;
                    }
                    case GunType.Lasergun: {
                            SetLaserStatus(playerGun.ValueRO.IsSootingButtonDown);
                            PlayerAudioController.Instance.Shoot(PlayerControleldGun.gunType);
                            SkillsMonobehAnimations.Instance.Shoot(PlayerControleldGun.gunType);
                            break;
                    }
                }


            } else {
                switch (PlayerControleldGun.gunType) {
                    case GunType.Lasergun: {
                            SetLaserStatus(playerGun.ValueRO.IsSootingButtonDown);
                            break;
                    }
                }
            }
            void SetLaserStatus(bool isShooting) {
                var entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
                if (entityManager.HasComponent<LaserGun>(entity)) {
                    var lasergunData = entityManager.GetComponentData<LaserGun>(entity);
                    lasergunData.isShooting = isShooting;
                    entityManager.SetComponentData<LaserGun>(entity, lasergunData);
                }
            }
        }

       
    }

    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    {
        
    }
}

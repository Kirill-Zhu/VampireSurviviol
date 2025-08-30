using Unity.Burst;
using Unity.Entities;
using Unity.Jobs;
using Unity.Physics;
using Unity.Physics.Systems;
using Unity.VisualScripting;

[RequireMatchingQueriesForUpdate]
[UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
[UpdateAfter(typeof(PhysicsSystemGroup))]
partial struct LaserGunTriggerEventSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state) {
        state.RequireForUpdate<SimulationSingleton>();
        state.RequireForUpdate<PhysicsWorldSingleton>();  
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        SystemAPI.TryGetSingleton<PauseComponent>(out var pause);
        if (pause!.IsPaused)
            return;

        var simulationSingleton = SystemAPI.GetSingleton<SimulationSingleton>();
        var ecbSingleton = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>();
        var job = new Job {
            LaserGunLookup = SystemAPI.GetComponentLookup<LaserGun>(),
            EnemyHealthLookup = SystemAPI.GetComponentLookup<EnemyHealth>(),
            ecb = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged).AsParallelWriter()
        };
        JobHandle jobHandle = job.Schedule(simulationSingleton, state.Dependency);
        jobHandle.Complete();
    }


    private partial struct Job : ITriggerEventsJob {

        public ComponentLookup<LaserGun> LaserGunLookup;
        public ComponentLookup<EnemyHealth> EnemyHealthLookup;
        public EntityCommandBuffer.ParallelWriter ecb;
        public void Execute(TriggerEvent triggerEvent) {
            
            Entity entityA = triggerEvent.EntityA;
            Entity entityB = triggerEvent.EntityB;

            bool AIsLaserGun = LaserGunLookup.HasComponent(entityA);
            bool BIsLaserGun = LaserGunLookup.HasComponent(entityB);

            bool AIsEnemyHealth = EnemyHealthLookup.HasComponent(entityA);
            bool BIsEnemyHealth = EnemyHealthLookup.HasComponent (entityB);

            if(AIsLaserGun && BIsEnemyHealth) {      
                var laserGun = LaserGunLookup[entityA];
                if (!laserGun.isShooting)
                    return;
                var enemyHealth = EnemyHealthLookup[entityB];
               

                if (laserGun.timer >= laserGun.damageTime) {
                    enemyHealth.Health -= laserGun.damage;

                    ecb.SetComponent<LaserGun>(triggerEvent.BodyIndexA, entityA, new LaserGun {
                        isShooting = laserGun.isShooting,
                        damage = laserGun.damage,
                        timer = 0,
                        damageTime = laserGun.damageTime,
                    });

                    EnemyHealthLookup[entityB] = enemyHealth;
                    if (enemyHealth.Health <= 0)
                        ecb.AddComponent<DestroyTag>(triggerEvent.BodyIndexB, entityB);
                }
            }
            if (BIsLaserGun && AIsEnemyHealth) {
                var laserGun = LaserGunLookup[entityB];
                if (!laserGun.isShooting)
                    return;
                var enemyHealth = EnemyHealthLookup[entityA];
        
                if (laserGun.timer >= laserGun.damageTime) {
                    enemyHealth.Health -= laserGun.damage;

                    ecb.SetComponent<LaserGun>(triggerEvent.BodyIndexB, entityB, new LaserGun {
                        isShooting = laserGun.isShooting,
                        damage = laserGun.damage,
                        timer = 0,
                        damageTime = laserGun.damageTime,
                    });

                    EnemyHealthLookup[entityA] = enemyHealth;
                    if (enemyHealth.Health <= 0)
                        ecb.AddComponent<DestroyTag>(triggerEvent.BodyIndexA, entityA);
                }
            }
            
        }
    }
}

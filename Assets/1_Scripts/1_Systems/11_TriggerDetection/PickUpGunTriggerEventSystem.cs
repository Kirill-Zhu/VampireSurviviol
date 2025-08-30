using Unity.Collections;
using Unity.Burst;
using Unity.Entities;
using Unity.Physics;
using Unity.Physics.Systems;

[RequireMatchingQueriesForUpdate]
[UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
[UpdateAfter(typeof(PhysicsSystemGroup))]
partial struct PickUpGunTriggerEventSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<SimulationSingleton>();
        state.RequireForUpdate<PhysicsWorldSingleton>();

    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state) {

        var simulationSingleton =  SystemAPI.GetSingleton<SimulationSingleton>();
        var ecbSingletone = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>();
        var triggerJob = new Job {
            PlayerGunLookup = SystemAPI.GetComponentLookup<ControlledGun>(),
            TakeUpGunLookup = SystemAPI.GetComponentLookup<TakeUpGunTrigger>(true),
            ecb = ecbSingletone.CreateCommandBuffer(state.WorldUnmanaged).AsParallelWriter(),
        };

        var jobHandle = triggerJob.Schedule(simulationSingleton, state.Dependency);
        jobHandle.Complete();

    }

    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    {
        
    }
    private partial struct Job: ITriggerEventsJob {
         public ComponentLookup<ControlledGun> PlayerGunLookup;
         [ReadOnly] public ComponentLookup<TakeUpGunTrigger> TakeUpGunLookup;
        public EntityCommandBuffer.ParallelWriter ecb;
        public void Execute(TriggerEvent triggerEvent) {
            
            Entity entityA = triggerEvent.EntityA;
            Entity entityB = triggerEvent.EntityB;

            bool AIsPlayer = PlayerGunLookup.HasComponent(entityA);
            bool BIsPlayer = PlayerGunLookup.HasComponent(entityB);

            bool AIsGun = TakeUpGunLookup.HasComponent(entityA);
            bool BIsGun = TakeUpGunLookup.HasComponent(entityB);

            if (AIsPlayer && BIsGun) { 
                var playerGun = PlayerGunLookup[entityA];
                var gun = TakeUpGunLookup[entityB];
                playerGun.gunType = gun.gunType;
                PlayerGunLookup[entityA] = playerGun;
                ecb.AddComponent<DestroyTag>(triggerEvent.BodyIndexB, entityB);
            }
            if (AIsGun && BIsPlayer) {
                var playerGun = PlayerGunLookup[entityB];
                var gun = TakeUpGunLookup[entityA];
                playerGun.gunType = gun.gunType;
                PlayerGunLookup[entityB] = playerGun;
                ecb.AddComponent<DestroyTag>(triggerEvent.BodyIndexA, entityA);
            }
        }
    }
}

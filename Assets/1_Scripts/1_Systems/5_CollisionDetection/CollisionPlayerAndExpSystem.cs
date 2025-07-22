using Unity.Burst;
using Unity.Entities;
using Unity.Physics;
using Unity.Physics.Systems;
using Unity.Collections;
using UnityEngine;
using LVLUP;
using Unity.Mathematics;


[RequireMatchingQueriesForUpdate]
[UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
[UpdateAfter(typeof(PhysicsSystemGroup))]

public partial struct CollisionPlayerAndExpSystem : ISystem {
    [BurstCompile]
    public void OnCreate(ref SystemState state) {
        state.RequireForUpdate<SimulationSingleton>();
        state.RequireForUpdate<PhysicsWorldSingleton>();

    }
    [BurstCompile]
    public void OnUpdate(ref SystemState state) {

        var ecbSingletone = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>();

        var collisionEventQueue = new NativeQueue<CollisionEventData>(Allocator.TempJob);

        var simulationSingleton = SystemAPI.GetSingleton<SimulationSingleton>();

        var playerAndExpCOllisionJob = new CollisionPlayerAndExpEventJob {
            ExpCrystal = SystemAPI.GetComponentLookup<ExpCrystal>(true),
            PlayerExp = SystemAPI.GetComponentLookup<PlayerExp>(),
            ecb = ecbSingletone.CreateCommandBuffer(state.WorldUnmanaged).AsParallelWriter(),
            CollisionEvents = collisionEventQueue
        };

        var jobHandle = playerAndExpCOllisionJob.Schedule(simulationSingleton, state.Dependency);

        jobHandle.Complete();
        while (collisionEventQueue.TryDequeue(out var collisionEvent)) {

            UnityEngine.Debug.Log($"Collision between {collisionEvent.EntityA} and {collisionEvent.EntityA}"); 
        }
        collisionEventQueue.Dispose();
    }
    private struct CollisionPlayerAndExpEventJob : ICollisionEventsJob {
        [ReadOnly] public ComponentLookup<ExpCrystal> ExpCrystal;
        public ComponentLookup<PlayerExp> PlayerExp;

        public EntityCommandBuffer.ParallelWriter ecb;
        public NativeQueue<CollisionEventData> CollisionEvents;

        public void Execute(CollisionEvent collisionEvent) {
            Entity entityA = collisionEvent.EntityA;
            Entity entityB = collisionEvent.EntityB;

            bool AIsExpCrystal = ExpCrystal.HasComponent(entityA);
            bool BIsExpCrystal = ExpCrystal.HasComponent(entityB);

            bool AIsPlayerExp = PlayerExp.HasComponent(entityA);
            bool BIsPlayerExp = PlayerExp.HasComponent(entityB);
            // Создаем событие столкновения


            //if (AIsExpCrystal && BIsPlayerExp) {
            //    var Crystal = ExpCrystal[entityA];
            //    var Player = PlayerExp[entityB];
            //    Debug.Log("Exp Contact");
            //    Player.Exp += Crystal.Exp;
            //    PlayerExp[entityB] = Player;

            //    var eventData = new CollisionEventData {
            //        EntityA = collisionEvent.EntityA,
            //        EntityB = collisionEvent.EntityB,
            //        ImpactNormal = collisionEvent.Normal
            //    };

            //    // Добавляем событие в очередь
            //    CollisionEvents.Enqueue(eventData);
            //}
            if (BIsExpCrystal && AIsPlayerExp) {
                var Crystal = ExpCrystal[entityB];
                var Player = PlayerExp[entityA];
                Debug.Log("Exp Contact");
                Player.AddExp (Crystal.Exp);
                PlayerExp[entityA] = Player;

                var eventData = new CollisionEventData {
                    EntityA = collisionEvent.EntityA,
                    EntityB = collisionEvent.EntityB,
                    ImpactNormal = collisionEvent.Normal,
                  

                };
                // Добавляем событие в очередь
                CollisionEvents.Enqueue(eventData);

                ecb.AddComponent<DestroyTag>(collisionEvent.BodyIndexB, entityB);
               
            }
        }
    }

}
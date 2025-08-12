using UnityEngine;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Physics;
using Unity.Physics.Systems;

[RequireMatchingQueriesForUpdate]
[UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
[UpdateAfter(typeof(PhysicsSimulationGroup))]
partial struct CollisionPhysicsAndEnenmies : ISystem
{
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

      
        CollisionPhysicsAndEnemyJob job = new CollisionPhysicsAndEnemyJob {
            minVelocityRequiredToDealDamage = 4f,
            physicsMassLookup = SystemAPI.GetComponentLookup<PhysicsMass>(),
            physicsVelocityLookup = SystemAPI.GetComponentLookup<PhysicsVelocity>(),

            collisionEventPhysicsTagLookup = SystemAPI.GetComponentLookup<CollisionEventPhysicsTag>(true),
            enemyHealthLookup = SystemAPI.GetComponentLookup<EnemyHealth>(),
            collisionEventQueue = collisionEventQueue,
            ecb = ecbSingletone.CreateCommandBuffer(state.WorldUnmanaged).AsParallelWriter()
        };

        var jobHandle = job.Schedule(simulationSingleton, state.Dependency);
        jobHandle.Complete();
        collisionEventQueue.Dispose();


    }

    [BurstCompile]
    public void OnDestroy(ref SystemState state) {

    }

    private struct CollisionPhysicsAndEnemyJob : ICollisionEventsJob {
        [ReadOnly] public float minVelocityRequiredToDealDamage;
        [ReadOnly] public ComponentLookup<PhysicsMass> physicsMassLookup;
        [ReadOnly] public ComponentLookup<PhysicsVelocity> physicsVelocityLookup;
        [ReadOnly] public ComponentLookup<CollisionEventPhysicsTag> collisionEventPhysicsTagLookup;
        public ComponentLookup<EnemyHealth> enemyHealthLookup;

        public NativeQueue<CollisionEventData> collisionEventQueue;
        public EntityCommandBuffer.ParallelWriter ecb;



        public void Execute(CollisionEvent collisionEvent) {

            Entity entityA = collisionEvent.EntityA;
            Entity entityB = collisionEvent.EntityB;

            bool AIsPhysics = collisionEventPhysicsTagLookup.HasComponent(entityA);
            bool BIsPhysics = collisionEventPhysicsTagLookup.HasComponent(entityB);

            bool AIsEnemy = enemyHealthLookup.HasComponent(entityA);
            bool BIsEnemy = enemyHealthLookup.HasComponent(entityB);

            if (AIsPhysics && BIsEnemy) {

                var physics = collisionEventPhysicsTagLookup[entityA];
                var enemyHealth = enemyHealthLookup[entityB];


                var eventData = new CollisionEventData {
                    EntityA = collisionEvent.EntityA,
                    EntityB = collisionEvent.EntityB,
                    ImpactNormal = collisionEvent.Normal,
                };
                if (physicsMassLookup.TryGetComponent(collisionEvent.EntityA, out PhysicsMass physicsMass)
                    &&
                    physicsVelocityLookup.TryGetComponent(collisionEvent.EntityA, out PhysicsVelocity physicsVelocity)) {

                    float velocityOverall = physicsVelocity.Linear.x + physicsVelocity.Linear.y + physicsVelocity.Linear.z;
                    float damage = velocityOverall / physicsMass.InverseMass;

                    if (velocityOverall > minVelocityRequiredToDealDamage) { 
                        enemyHealth.DoDamage((int)damage);
                        //Debug.Log("Enemy A and physics B collides Velocity is :" + velocityOverall);
                    }

                    if (enemyHealth.Health <= 0) {
                        ecb.AddComponent<DestroyTag>(collisionEvent.BodyIndexB, entityB);
                    }
                }
                collisionEventQueue.Enqueue(eventData);

            }
            if (AIsEnemy && BIsPhysics) {
                var physics = collisionEventPhysicsTagLookup[entityB];
                var enemyHealth = enemyHealthLookup[entityA];


                var eventData = new CollisionEventData {
                    EntityA = collisionEvent.EntityA,
                    EntityB = collisionEvent.EntityB,
                    ImpactNormal = collisionEvent.Normal,
                };
                //Do damage
                if (physicsMassLookup.TryGetComponent(collisionEvent.EntityA, out PhysicsMass physicsMass)
                    &&
                    physicsVelocityLookup.TryGetComponent(collisionEvent.EntityA, out PhysicsVelocity physicsVelocity)) {
                   
                    float velocityOverall = physicsVelocity.Linear.x + physicsVelocity.Linear.y + physicsVelocity.Linear.z;
                    float damage = velocityOverall / physicsMass.InverseMass;

                    if (velocityOverall > minVelocityRequiredToDealDamage) {
                        enemyHealth.DoDamage((int)damage);
                        //Debug.Log("Enemy A and physics B collides Velocity is :" + velocityOverall);
                    }

                    if (enemyHealth.Health <= 0) {
                        ecb.AddComponent<DestroyTag>(collisionEvent.BodyIndexA, entityA);
                    }

                }
                collisionEventQueue.Enqueue(eventData);
            }   
        }
    }
}

using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Physics;
using Unity.Physics.Systems;
using UnityEditor.Build;
using UnityEngine;

// This system applies an impulse to any dynamic that collides with a Repulsor.
// A Repulsor is defined by a PhysicsShapeAuthoring with the `Raise Collision Events` flag ticked and a
// CollisionEventImpulse behaviour added.
[RequireMatchingQueriesForUpdate]
[UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
[UpdateAfter(typeof(PhysicsSystemGroup))]
public partial struct CollisionEventSystem : ISystem {
    internal ComponentDataHandles m_ComponentDataHandles;

    internal struct ComponentDataHandles {
        public ComponentLookup<CollisionEventData> CollisionEventData;
        public ComponentLookup<EnemyHealth> EnemyHealth;

        public ComponentDataHandles(ref SystemState systemState) {
            CollisionEventData = systemState.GetComponentLookup<CollisionEventData>(true);
            EnemyHealth = systemState.GetComponentLookup<EnemyHealth>(false);
        }

        public void Update(ref SystemState systemState) {
            CollisionEventData.Update(ref systemState);
            EnemyHealth.Update(ref systemState);
        }
    }

    [BurstCompile]
    public void OnCreate(ref SystemState state) {
        state.RequireForUpdate(state.GetEntityQuery(ComponentType.ReadOnly<CollisionEventData>()));
        state.RequireForUpdate(state.GetEntityQuery(ComponentType.ReadOnly<EnemyHealth>())); ;
        m_ComponentDataHandles = new ComponentDataHandles(ref state);
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state) {
        
        m_ComponentDataHandles.Update(ref state);
        state.Dependency = new CollisionEventImpulseJob {
            CollisionEventData = m_ComponentDataHandles.CollisionEventData,
            EnemyHealth = m_ComponentDataHandles.EnemyHealth,
        }.Schedule(SystemAPI.GetSingleton<SimulationSingleton>(), state.Dependency);
    }

    [BurstCompile]
    struct CollisionEventImpulseJob : ICollisionEventsJob {
        [ReadOnly] public ComponentLookup<CollisionEventData> CollisionEventData;
        public ComponentLookup<EnemyHealth> EnemyHealth;
        
        public void Execute(CollisionEvent collisionEvent){
            Entity entityA = collisionEvent.EntityA;
            Entity entityB = collisionEvent.EntityB;
            Debug.Log("Hit enemy by bullet");


            //bool isBodyADynamic = PhysicsVelocityData.HasComponent(entityA);
            //bool isBodyBDynamic = PhysicsVelocityData.HasComponent(entityB);

            //bool isBodyARepulser = CollisionEventData.HasComponent(entityA);
            //bool isBodyBRepulser = CollisionEventData.HasComponent(entityB);

            //if (isBodyARepulser && isBodyBDynamic) {
            //    var impulseComponent = CollisionEventData[entityA];
            //    var velocityComponent = PhysicsVelocityData[entityB];
            //    velocityComponent.Linear = impulseComponent.Impulse;
            //    PhysicsVelocityData[entityB] = velocityComponent;
            //}

            //if (isBodyBRepulser && isBodyADynamic) {
            //    var impulseComponent = CollisionEventData[entityB];
            //    var velocityComponent = PhysicsVelocityData[entityA];
            //    velocityComponent.Linear = impulseComponent.Impulse;
            //    PhysicsVelocityData[entityA] = velocityComponent;
            //}
            bool IsBodyAHasHealth = EnemyHealth.HasComponent(entityA);
            bool IsBodyBHasHealth = EnemyHealth.HasComponent(entityB);

            bool IsBodyACollisionable = CollisionEventData.HasComponent(entityA);
            bool IsBodyBCollisionable = CollisionEventData.HasComponent(entityB);

            if (IsBodyAHasHealth && IsBodyBCollisionable) {
                
                var healthComponent = EnemyHealth[entityA];
                var damageComponent = CollisionEventData[entityB];
                healthComponent.DoDamage(damageComponent.Damage);
                EnemyHealth[entityA] = healthComponent;
            }
            if (IsBodyBHasHealth && IsBodyACollisionable) {

                var healthComponent = EnemyHealth[entityB];
                var damageComponent = CollisionEventData[entityA];
                healthComponent.DoDamage(damageComponent.Damage);
                EnemyHealth[entityB] = healthComponent;
            }
        }
    }
}

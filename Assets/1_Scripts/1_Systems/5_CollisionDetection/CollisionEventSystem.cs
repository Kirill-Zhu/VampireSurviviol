
using Unity.Burst;
using Unity.Entities;
using Unity.Physics;
using Unity.Physics.Systems;
using Unity.Collections;
using UnityEngine;

//// Компонент для отметки объектов, которые должны обрабатывать коллизии
public struct CollisionProcessor : IComponentData { }

[RequireMatchingQueriesForUpdate]
[UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
[UpdateAfter(typeof(PhysicsSystemGroup))]
public partial struct SimpleCollisionSystem : ISystem {

    [BurstCompile]
    public void OnCreate(ref SystemState state) {
        state.RequireForUpdate<SimulationSingleton>();
        state.RequireForUpdate<PhysicsWorldSingleton>();
    }

    [BurstCompile]
    public void OnDestroy(ref SystemState state) { }

    [BurstCompile]
    public void OnUpdate(ref SystemState state) {


        var ecbSingletone = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>();

        // Получаем синглтоны физики
        var simulationSingleton = SystemAPI.GetSingleton<SimulationSingleton>();
        
        

        var playerProjectileAndEnemyCollisionEventJob = new CollisionPlayerProjectileAndEnemyEventJob {
            EnemyHealthLookUp = SystemAPI.GetComponentLookup<EnemyHealth>(),
            PlayerProjectileLookup = SystemAPI.GetComponentLookup<PlayerProjectile>(true),
            AudioOnCollisonLookup = SystemAPI.GetComponentLookup<AudioOnCollision>(),

            ecb = ecbSingletone.CreateCommandBuffer(state.WorldUnmanaged).AsParallelWriter(),
        };


        // Запускаем Job и связываем его с ECB
        state.Dependency = playerProjectileAndEnemyCollisionEventJob.Schedule(simulationSingleton, state.Dependency);
    }

    [BurstCompile]
    private struct CollisionPlayerProjectileAndEnemyEventJob : ICollisionEventsJob {
        public ComponentLookup<EnemyHealth> EnemyHealthLookUp;
        [ReadOnly] public ComponentLookup<PlayerProjectile> PlayerProjectileLookup;
        public ComponentLookup<AudioOnCollision> AudioOnCollisonLookup;
        
        public EntityCommandBuffer.ParallelWriter ecb;
        public void Execute(CollisionEvent collisionEvent) {
            Entity entityA = collisionEvent.EntityA;
            Entity entityB = collisionEvent.EntityB;

            // Проверяем, есть ли у сущностей компонент CollisionProcessor
            bool AIsHealth = EnemyHealthLookUp.HasComponent(entityA);
            bool BIsHealth = EnemyHealthLookUp.HasComponent(entityB);

            bool AIsPlayerProjectile = PlayerProjectileLookup.HasComponent(entityA);
            bool BISPlyaerProjectile = PlayerProjectileLookup.HasComponent(entityB);


            if (AIsHealth && BISPlyaerProjectile) {
                // Получаем скорости (если есть)
                var projectile = PlayerProjectileLookup[entityB];
                var enemy = EnemyHealthLookUp[entityA];

                enemy.Health -= projectile.Damage;

                
                EnemyHealthLookUp[entityA] = enemy;
                //Audio
                if (AudioOnCollisonLookup.HasComponent(entityB)) {
                    var audioSource = AudioOnCollisonLookup[entityB];
                    AudioOnCollisonLookup[entityB] = audioSource;
                 
                    ecb.AddComponent<AudioOnCollision>(collisionEvent.BodyIndexB, entityB, new AudioOnCollision { ShouldPlay = true });
                }
               
                //Destroy Projectile
                ecb.AddComponent<DestroyTag>(collisionEvent.BodyIndexB, entityB);
             
              //Destroy Enemy
                if (enemy.Health <= 0) {
                    ecb.AddComponent<DestroyTag>(collisionEvent.BodyIndexA, entityA);
                }
            }
           
        }
    }



}


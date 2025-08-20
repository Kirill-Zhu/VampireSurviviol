using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;
using UnityEngine;


[UpdateInGroup(typeof(LateSimulationSystemGroup))]
[UpdateAfter(typeof(UnitMoverSystem))]
partial struct AntiGravitationSkillSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
       
        EntityCommandBuffer ecb = new EntityCommandBuffer(Allocator.Temp);
        if (Input.GetKeyDown(KeyCode.Space)) {
            //GetPlayerPos
            float3 playerPos = float3.zero;
            foreach(var (playerPosComponent, localTransform) in SystemAPI.Query< RefRO<PlayerMover>, RefRO<LocalTransform>>()) {
                playerPos = localTransform.ValueRO.Position;
            }
            
            foreach(var (physicsMass, 
                    localTransform, 
                    entity) 
                    in SystemAPI.Query<RefRO<PhysicsMass>,
                    RefRO<LocalTransform>>().
                    WithEntityAccess().WithNone<PlayerMover>()){

                if(math.distance(localTransform.ValueRO.Position, playerPos) < 10f) {
                    ecb.AddComponent(entity, new AntiGravitationComponent {TimeToFly = 5, Timer = 0});
                }
            }
        }
            foreach ((RefRW<AntiGravitationComponent> anitGravitation,    
                      RefRW<PhysicsVelocity> velocity, 
                      RefRW<PhysicsMass> mass, 
                      Entity entity) 
                      in 
                      SystemAPI.Query<RefRW<AntiGravitationComponent>, 
                      RefRW <PhysicsVelocity>, 
                      RefRW<PhysicsMass>>().
                      WithEntityAccess()) { 
                
                anitGravitation.ValueRW.Timer += SystemAPI.Time.DeltaTime;

                velocity.ValueRW.Linear.y = 2;  

                anitGravitation.ValueRW.Timer += SystemAPI.Time.DeltaTime;
                if (anitGravitation.ValueRO.Timer > anitGravitation.ValueRO.TimeToFly) {
                    ecb.RemoveComponent<AntiGravitationComponent>(entity);
                }   
            }
        ecb.Playback(state.EntityManager);
    }

    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    {
        
    }
}

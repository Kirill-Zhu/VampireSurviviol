using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
using UnityEngine;

partial struct EnemyAttackSystem : ISystem
{
    float3 _playerPos;
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        //Get Plyaer Pos
        foreach (RefRO<LocalTransform> localTransform in SystemAPI.Query<RefRO<LocalTransform>>().WithAll<PlayerInput>()) {
            _playerPos = localTransform.ValueRO.Position;
        }


        //For Damage 
        var ecbSingleton = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>();
        Entity playerEntity = SystemAPI.GetSingletonEntity<PlayerInput>();

        AttackJob job = new AttackJob() {
            DeltaTime = SystemAPI.Time.DeltaTime,
            PlayerPos = _playerPos,

            ecb = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged).AsParallelWriter(),
            entity = playerEntity
        };

        job.ScheduleParallel();

        foreach (
            (RefRW<PlayerHealth> playerHealth,
            DynamicBuffer<PlayerDamageBufferElement> damageBuffer
            ) in 
            SystemAPI.Query<
            RefRW<PlayerHealth>,
            DynamicBuffer<PlayerDamageBufferElement>
            >()) 
            {
                foreach( var damageElement in damageBuffer) {
                    playerHealth.ValueRW.Health -=damageElement.Value;
                }
                damageBuffer.Clear();
            }   


        //foreach ((RefRW<EnemyAttack> attack, RefRO<LocalTransform> localTransform) in SystemAPI.Query<RefRW<EnemyAttack>, RefRO<LocalTransform>>()) {
        //    attack.ValueRW.AttackTime += SystemAPI.Time.DeltaTime;
        //    if (attack.ValueRO.AttackTime > attack.ValueRO.AttackRate) {

        //        if (math.distance(localTransform.ValueRO.Position, _playerPos) <= attack.ValueRO.AttackRange) {
        //            attack.ValueRW.AttackTime = 0;
        //            Debug.Log("Enemy Attack");
        //            foreach (RefRW<PlayerHealth> playerHealth in SystemAPI.Query<RefRW<PlayerHealth>>().WithAll<PlayerInput>()) {
        //                playerHealth.ValueRW.Health -= attack.ValueRO.Damage;

        //                if (playerHealth.ValueRW.Health <= 0) {
        //                    //--------------------PlayerDead---------------- -
        //                }
        //            }
        //        }
        //    }
        //}
    }

    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    {
        
    }
}
public partial struct AttackJob : IJobEntity {
  
   
    public float DeltaTime;
    public float3 PlayerPos;

    public EntityCommandBuffer.ParallelWriter ecb;
    public Entity entity;
   
  
    public void Execute(ref EnemyAttack attack, in LocalTransform localTransform,[EntityIndexInQuery]int sortKey) {
        attack.AttackTime += DeltaTime;     
        if (attack.AttackTime > attack.AttackRate) {

            if (math.distance(localTransform.Position, PlayerPos) <= attack.AttackRange) {
                attack.AttackTime = 0;
                Debug.Log("Enemy Attack");
               
                PlayerDamageBufferElement playerDamageBuffer = new PlayerDamageBufferElement { Value = attack.Damage };
                ecb.AppendToBuffer(sortKey,entity,playerDamageBuffer);
                   
            }
        }
    }
}
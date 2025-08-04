using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;

partial struct EnemyJumperSystem : ISystem
{
    float3 _playerPos;

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
       
        foreach(var (palyerMover, localTransform) in SystemAPI.Query<RefRO<PlayerMover>, RefRO<LocalTransform>>()) {
            _playerPos = localTransform.ValueRO.Position;
        }

        foreach(var (jumper,
            physicsVelocity,
            localTransform) 
            in 
            SystemAPI.Query
            <RefRW<EnemyJumper>, 
            RefRW<PhysicsVelocity>, 
            RefRO<LocalTransform>>()) {
            jumper.ValueRW.JumpReloadTimer-=SystemAPI.Time.DeltaTime;
            
            if(jumper.ValueRO.JumpReloadTimer > 0) 
                continue;

            float3 jumpDirection = (_playerPos - localTransform.ValueRO.Position);
            jumpDirection = math.normalize(jumpDirection);
            physicsVelocity.ValueRW.Linear = jumpDirection * jumper.ValueRO.JumpForce;
            physicsVelocity.ValueRW.Linear += new float3(0, jumper.ValueRO.JumpUpForce,0);
            jumper.ValueRW.JumpReloadTimer = jumper.ValueRO.JumpRate;
        }
    }

  
}

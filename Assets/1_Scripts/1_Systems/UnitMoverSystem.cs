using Unity.Entities;
using Unity.Transforms;
using Unity.Physics;
using Unity.Mathematics;

using Unity.Burst;
using System.Diagnostics;
partial struct UnitMoverSystem : ISystem
{
    float3 playerPos;
    [BurstCompile]
   public void OnUpdate(ref SystemState state) {

        
        //Get Player Pos
        foreach(RefRO<LocalTransform> localTransform 
            in
            SystemAPI.Query<
            RefRO<LocalTransform>
            >().WithAll<PlayerInput>()) {
            playerPos = localTransform.ValueRO.Position;

            }

        //EnnemyMove
        UnitMoverJob job = new UnitMoverJob() {
            deltaTime = SystemAPI.Time.DeltaTime,
            targetPos = playerPos
            
        };
        job.ScheduleParallel();

        //foreach((RefRW<LocalTransform> localTransform,
        //        RefRO<UnitMover> unitMover,
        //        RefRW<PhysicsVelocity> PhysicsVelocity) 
        //    in  SystemAPI.Query<
        //        RefRW<LocalTransform>,
        //        RefRO<UnitMover>,
        //        RefRW<PhysicsVelocity>>()) {
   
        //    float3 moveDirection = unitMover.ValueRO.TargetPosition - localTransform.ValueRO.Position;
        //    moveDirection = math.normalize(moveDirection);

        //    localTransform.ValueRW.Rotation = math.slerp(localTransform.ValueRO.Rotation, quaternion.LookRotation(moveDirection, math.up()),
        //        SystemAPI.Time.DeltaTime*unitMover.ValueRO.RotationSpeed);
        //    //physicsVelocity.ValueRW.Linear = moveDirecrtion * unitMover.ValueRO.moveSpeed;
        //    //   physicsVelocity.ValueRW.Angular = float3.zero;
        //    float moveSpeed = unitMover.ValueRO.MoveSpeed;
        //     localTransform.ValueRW.Position += moveDirection *10* SystemAPI.Time.DeltaTime;
        //}

    }
}
public partial struct UnitMoverJob : IJobEntity {
    public float deltaTime;
    public float3 targetPos;
    public void Execute(ref LocalTransform localTransform, ref UnitMover unitMover, ref PhysicsVelocity physicsVelocity) {
        
        
            float3 moveDirection = targetPos - localTransform.Position;
            moveDirection.y = 0;
          
            moveDirection = math.normalize(moveDirection);

            localTransform.Rotation = math.slerp(localTransform.Rotation, quaternion.LookRotation(moveDirection, math.up()),
              deltaTime * unitMover.RotationSpeed);
     
        physicsVelocity.Linear += moveDirection * unitMover.MoveSpeed*deltaTime;

        //UnityEngine.Debug.Log("Physics Velocity is "+ physicsVelocity.Linear);
      
        physicsVelocity.Angular = float3.zero;
        
        
        
        
            //float moveSpeed = unitMover.MoveSpeed;
            //localTransform.Position += moveDirection * 10 * deltaTime;
    
    }
}
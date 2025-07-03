using System;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;
using UnityEngine;

partial struct PlayerMoveSystem : ISystem
{
    private Quaternion _prevRotation;
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        _prevRotation = Quaternion.identity;
    }

   
    public void OnUpdate(ref SystemState state)
    {
        //PlayerMoverJob job = new PlayerMoverJob() { 
        //    DeltaTime = SystemAPI.Time.DeltaTime};
        //job.ScheduleParallel();

        foreach ((

            RefRO<PlayerMover> playerMover,
            RefRW<LocalTransform> localTransform,
            RefRW<PhysicsVelocity> physicsVelocity)
            in
            SystemAPI.Query<

            RefRO<PlayerMover>,
            RefRW<LocalTransform>,
            RefRW<PhysicsVelocity>>().WithAll<PlayerInput>()) {


            float3 moveDirection = new float3();
            moveDirection = playerMover.ValueRO.InputTargetPosition;
            moveDirection = math.clamp(moveDirection, -playerMover.ValueRO.MoveSpeed, playerMover.ValueRO.MoveSpeed);

            if (moveDirection.x != 0 || moveDirection.z != 0) {


                // localTransform.ValueRW.Position += moveDirection * playerMover.ValueRO.MoveSpeed * SystemAPI.Time.DeltaTime;

                physicsVelocity.ValueRW.Linear = moveDirection * playerMover.ValueRO.MoveSpeed;
               
                physicsVelocity.ValueRW.Angular = float3.zero;
                
                _prevRotation = math.slerp(localTransform.ValueRO.Rotation, quaternion.LookRotation(moveDirection, math.up()), SystemAPI.Time.DeltaTime * playerMover.ValueRO.RotationSpeed);

            } else {
                physicsVelocity.ValueRW.Linear = float3.zero;
               
            }
                localTransform.ValueRW.Rotation = _prevRotation;
            
            //Gravity

            PhysicsWorldSingleton physicsWorldSingleton = SystemAPI.GetSingleton<PhysicsWorldSingleton>();
            ColliderCastHit hit = new ColliderCastHit();

            physicsWorldSingleton.SphereCast(localTransform.ValueRO.Position, 0.1f, math.down(), 0.9f,out hit,
                new CollisionFilter { BelongsTo = (uint)Layers.player, CollidesWith =(uint)Layers.Default}, QueryInteraction.IgnoreTriggers);

            if (hit.Entity.GetHashCode() == 0) {
                physicsVelocity.ValueRW.Linear.y = Physics.gravity.y;
            }
            
            PlayerCharacterMonobeh.instance.Position = localTransform.ValueRO.Position;
            PlayerCharacterMonobeh.instance.Rotation = localTransform.ValueRO.Rotation;

        }

        
    }

    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    {
        
    }
}
 public partial struct PlayerMoverJob: IJobEntity {

    public float DeltaTime;
    public void Execute(ref PlayerMover playerMover, ref PhysicsVelocity physicsVelocity, ref LocalTransform localTransform) {
        float3 moveDirection = new float3();
        moveDirection = playerMover.InputTargetPosition;
        moveDirection = math.clamp(moveDirection, -playerMover.MoveSpeed, playerMover.MoveSpeed);

        if (moveDirection.x != 0 || moveDirection.z != 0) {


            // localTransform.ValueRW.Position += moveDirection * playerMover.ValueRO.MoveSpeed * SystemAPI.Time.DeltaTime;

            physicsVelocity.Linear += moveDirection * playerMover.MoveSpeed * DeltaTime;
            physicsVelocity.Angular = float3.zero;
            localTransform.Rotation = math.slerp(localTransform.Rotation, quaternion.LookRotation(moveDirection, math.up()), DeltaTime * playerMover.RotationSpeed);


        }
        PlayerCharacterMonobeh.instance.Position = localTransform.Position;
        PlayerCharacterMonobeh.instance.Rotation = localTransform.Rotation;
    }
}


enum Layers {
    Default = 1<<0,
    player = 1<<6
}
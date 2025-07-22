using UnityEngine;
using Unity.Burst;
using Unity.Entities;

partial struct DestroyDelayedSystem : ISystem
{
   

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        EntityCommandBuffer ecb = new EntityCommandBuffer(Unity.Collections.Allocator.Temp);    
        foreach((var DelayedTag, var entity) in SystemAPI.Query<RefRW<DestroyDelayed>>().WithEntityAccess()) {
            DelayedTag.ValueRW.TimeToDestroy -=SystemAPI.Time.DeltaTime;
            if(DelayedTag.ValueRW.TimeToDestroy <= 0) {
                ecb.DestroyEntity(entity);
            }

        }
        ecb.Playback(state.EntityManager);
        ecb.Dispose();
    }

   
}

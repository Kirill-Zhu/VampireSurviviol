using Unity.Burst;
using Unity.Entities;
using Unity.Collections;
using Unity.Transforms;
using Unity.Mathematics;
using UnityEngine;
[UpdateInGroup(typeof(LateSimulationSystemGroup))]
partial struct DestroyTagSystem : ISystem
{
   

    [BurstCompile]
   
    public void OnUpdate(ref SystemState state)
    {
        SystemAPI.TryGetSingleton<EntitiesReferences>(out EntitiesReferences entitiesReferences);
       
        
        //var ecbDestroyEntity = new EntityCommandBuffer(Allocator.Temp);
        //foreach (
        //    (RefRO<DestroyTag> destroyTag,
        //    RefRO<LocalTransform> localTransform,
        //    Entity entity)
        //    in
        //    SystemAPI.Query<
        //        RefRO<DestroyTag>,
        //        RefRO<LocalTransform>>().WithEntityAccess()) {

        //    if (SystemAPI.HasComponent<ExpirianceTag>(entity)) {
        //        Entity exp = state.EntityManager.Instantiate(entitiesReferences.ExpCystal);
        //        SystemAPI.SetComponent(exp, LocalTransform.FromPositionRotation(localTransform.ValueRO.Position, quaternion.identity));
        //    }
        //    ecbDestroyEntity.DestroyEntity(entity);  
        //}

        //ecbDestroyEntity.Playback(state.EntityManager);
        //ecbDestroyEntity.Dispose();


        var ecbSingleton = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>();
        DestroyTagJob job = new DestroyTagJob {
            EntitiesReferences = entitiesReferences,
            ecb = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged).AsParallelWriter(),
            ExpTagComponentLookup = SystemAPI.GetComponentLookup<ExpirianceTag>(),

        };
        job.ScheduleParallel();
    }

   private partial struct DestroyTagJob: IJobEntity {
        [ReadOnly] public EntitiesReferences EntitiesReferences;
        [ReadOnly] public ComponentLookup<ExpirianceTag> ExpTagComponentLookup;
        public EntityCommandBuffer.ParallelWriter ecb;
        public void Execute(in DestroyTag tag, in LocalTransform localTransform, [EntityIndexInQuery] int sortKey,  Entity entity) {
      
            if (ExpTagComponentLookup.TryGetComponent(entity, out ExpirianceTag expTag)) {

                Entity ExpEntity =  ecb.Instantiate(sortKey, EntitiesReferences.ExpCystal);
                ecb.SetComponent(sortKey, ExpEntity, LocalTransform.FromPosition(localTransform.Position));

            }
            ecb.DestroyEntity(sortKey, entity);
        }

    } 
}

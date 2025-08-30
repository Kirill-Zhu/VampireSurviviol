using Unity.Burst;
using Unity.Entities;
using Unity.Collections;
using Unity.Transforms;


[UpdateInGroup(typeof(LateSimulationSystemGroup))]
partial struct DestroyTagSystem : ISystem
{
    private double _elapsedTime;
    private double _lastUpdatedTime;
    private float _destroyRate;
    
    
    [BurstCompile]
    public void OnCreate(ref SystemState state) {
        _destroyRate = 0.01f;
    }

    [BurstCompile]
   
    public void OnUpdate(ref SystemState state) {

        //-----------------------------------------------------Disable first

        SystemAPI.TryGetSingleton<EntitiesReferences>(out EntitiesReferences entitiesReferences);

        var ecbSingleton = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>();
        DestroyTagJob job = new DestroyTagJob {
            EntitiesReferences = entitiesReferences,
            ecb = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged).AsParallelWriter(),
            ExpTagComponentLookup = SystemAPI.GetComponentLookup<ExpirianceTag>(),
        };
        job.ScheduleParallel();


        //---------------------------------------------------Destroy then in query

        _elapsedTime = SystemAPI.Time.ElapsedTime;

        if (_elapsedTime - _lastUpdatedTime < _destroyRate)
            return;

      
        EntityCommandBuffer ecb = new EntityCommandBuffer(Allocator.Temp);
        _lastUpdatedTime = SystemAPI.Time.ElapsedTime;
        foreach (var (destroyTag, localTransform, entity) in SystemAPI.Query<RefRO<DestroyTag>, RefRO<LocalTransform>>().WithEntityAccess().WithOptions(EntityQueryOptions.IncludeDisabledEntities)) {

            if (state.EntityManager.HasComponent<ExpirianceTag>(entity)) {
                Entity expEntity = ecb.Instantiate(entitiesReferences.ExpCystal);
                ecb.SetComponent(expEntity, LocalTransform.FromPosition(localTransform.ValueRO.Position));
            }
            DestroyEntityWithChildren(entity, ecb, ref state);
            break;
        }
        ecb.Playback(state.EntityManager);
        ecb.Dispose();             
    }
    private void DestroyEntityWithChildren(Entity entity, EntityCommandBuffer ecb, ref SystemState state) {
        // Находим и уничтожаем всех детей
        if (state.EntityManager.HasComponent<Child>(entity)) {
            var children = state.EntityManager.GetBuffer<Child>(entity);
            for (int i = 0; i < children.Length; i++) {
                DestroyEntityWithChildren(children[i].Value, ecb, ref state);
            }
        }

        ecb.DestroyEntity(entity);
    }
    private partial struct DestroyTagJob: IJobEntity {
        [ReadOnly] public EntitiesReferences EntitiesReferences;
        [ReadOnly] public ComponentLookup<ExpirianceTag> ExpTagComponentLookup;
        public EntityCommandBuffer.ParallelWriter ecb;
        public void Execute(in DestroyTag tag, in LocalTransform localTransform, [EntityIndexInQuery] int sortKey,  Entity entity) {
         
            if (ExpTagComponentLookup.TryGetComponent(entity, out ExpirianceTag expTag)) {

                //Entity ExpEntity =  ecb.Instantiate(sortKey, EntitiesReferences.ExpCystal);
                //ecb.SetComponent(sortKey, ExpEntity, LocalTransform.FromPosition(localTransform.Position));
            }
            ecb.SetEnabled(sortKey, entity, false);
        }

    } 
}

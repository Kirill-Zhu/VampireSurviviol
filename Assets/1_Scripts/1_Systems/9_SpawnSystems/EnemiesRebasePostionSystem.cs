using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;


partial struct EnemiesRebasePostionSystem : ISystem
{
    private float3 _playerPos;
    private float3 _nearesPos;
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        _playerPos = float3.zero;
        _nearesPos = float3.zero;
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state) {
        foreach ((RefRO<PlayerMover> PlayerMover, RefRO<LocalTransform> localTransform)
            in
            SystemAPI.Query<RefRO<PlayerMover>, RefRO<LocalTransform>>()) {

            _playerPos = localTransform.ValueRO.Position;

        }

        EntityCommandBuffer commandBuffer = new EntityCommandBuffer(Allocator.Temp);
        foreach ((RefRO<UnitMoverRebaseTag> unitMoverRebaseTag, RefRW<LocalTransform> localTransform, Entity entity)
           in
           SystemAPI.Query<RefRO<UnitMoverRebaseTag>, RefRW<LocalTransform>>().WithEntityAccess()) {
            commandBuffer.RemoveComponent<UnitMoverRebaseTag>(entity);
            localTransform.ValueRW.Position = _nearesPos;
        }
        commandBuffer.Playback(state.EntityManager);

        var ecbSingleton = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>();

        Job job = new Job {
            playerPos = _playerPos,
            ecb = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged).AsParallelWriter()
        };
        job.ScheduleParallel();

        foreach (
             (RefRO<EnemySpawner> spawner,
             DynamicBuffer<EnemySpawnerBufferElement> bufferElement)
             in
             SystemAPI.Query<
             RefRO<EnemySpawner>,
             DynamicBuffer<EnemySpawnerBufferElement>
             >()) {
            _nearesPos = EnemySpawnSystem.NearestToPlayerSpawnPos(_playerPos, bufferElement);
        }
    }
    private partial struct Job : IJobEntity {
        public float3 playerPos;
        public EntityCommandBuffer.ParallelWriter ecb;
        public void Execute(ref UnitMover unitMover, LocalTransform localTransform, Entity entity, [EntityIndexInQuery] int sortKey) {
            if (math.distance(localTransform.Position, playerPos) > 100) {
                ecb.AddComponent(sortKey, entity, new UnitMoverRebaseTag());
            }
        }

    }
    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    {
        
    }

   
}

using System.Diagnostics;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;

partial class VFXSpawnSystem : SystemBase
{

    private VFXSpawner _vfxSpawner;
    private EntityCommandBufferSystem _ecbSystem;
    protected override void OnStartRunning() {
        _vfxSpawner = VFXSpawner.Instance;
        _ecbSystem = World.GetOrCreateSystemManaged<BeginSimulationEntityCommandBufferSystem>();   
    }
    [BurstCompile]
    protected override void OnUpdate() {
        Entities.ForEach((ref VFXPlayComponent VFXCOmponent, ref LocalTransform localTransform) => {
            _vfxSpawner.PlayParticle(VFXCOmponent.VFXType, localTransform.Position);
        }).WithoutBurst().Run();

        var ecb = _ecbSystem.CreateCommandBuffer();
        Entities.ForEach((Entity entity, ref VFXPlayComponent VFXCOmponent) => {
            ecb.RemoveComponent<VFXPlayComponent>(entity);
        }).WithoutBurst().Run();
    }
   
    private partial struct Job : IJobEntity {
       public EntityCommandBuffer.ParallelWriter ecb;

        public void Execute() {

        }
    }
}

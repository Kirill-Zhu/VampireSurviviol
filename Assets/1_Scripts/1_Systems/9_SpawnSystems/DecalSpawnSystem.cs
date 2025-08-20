using Unity.Entities;
using Unity.Transforms;
[RequireMatchingQueriesForUpdate]
partial class DecalSpawnSystem : SystemBase
{  
    private DecalsSpawner _decalSpawner;
    
    private EntityCommandBufferSystem _ecbSystem;
  
    
    protected override void OnStartRunning() { 
        _ecbSystem = World.GetOrCreateSystemManaged<BeginSimulationEntityCommandBufferSystem>();
        _decalSpawner = DecalsSpawner.Instance;
    }
    protected override void OnUpdate() {
      

        EntityCommandBuffer ecb = _ecbSystem.CreateCommandBuffer();
        Entities.ForEach((Entity entity, ref DecalComponent decaComponent, ref LocalTransform localTransform) => {

            _decalSpawner.SpawnDecal(decaComponent.DecalType, localTransform.Position);
            ecb.AddComponent(entity, new DestroyDelayed { TimeToDestroy = 1 });

        }).WithoutBurst().Run();

        _ecbSystem.AddJobHandleForProducer(Dependency);
    }
}

using Unity.Burst;
using Unity.Entities;

[UpdateInGroup(typeof(LateSimulationSystemGroup))]
partial struct PlayerSpawnerSystem : ISystem {
    [BurstCompile]
    public void OnCreate(ref SystemState state) {

       
   
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state) {
      
    }
    public void OnDestroy(ref SystemState state) {

    }
}

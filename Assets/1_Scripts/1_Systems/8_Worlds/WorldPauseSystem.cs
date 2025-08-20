using Unity.Burst;
using Unity.Entities;
using UnityEngine;        
using Unity.Physics.Systems;

partial struct WorldPauseSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state) {
      
    }


    public void OnUpdate(ref SystemState state) {
        if (Input.GetKeyDown(KeyCode.P)) {

            foreach (var pauseComponent in SystemAPI.Query<RefRW<PauseComponent>>()) { 
             
                if (pauseComponent.ValueRW.IsPaused) {
                   PauseWorld(pauseComponent, false);
                } else {
                    PauseWorld(pauseComponent ,true);
                }
            }    
        }
    }
    private void PauseWorld(RefRW<PauseComponent> pauseComponent, bool SetOnPause) {
        if (!SetOnPause) {
            pauseComponent.ValueRW.IsPaused = false;
            Debug.Log("Try UnPause");
            // Получаем мир ECS
            var world = World.DefaultGameObjectInjectionWorld;

            // Находим группу, отвечающую за симуляцию физики
            var physicsSimGroup = world.GetExistingSystemManaged<PhysicsSimulationGroup>();

            // Отключаем всю физику
            physicsSimGroup.Enabled = true;

        } else {
            // Получаем мир ECS
            var world = World.DefaultGameObjectInjectionWorld;

            // Находим группу, отвечающую за симуляцию физики
            var physicsSimGroup = world.GetExistingSystemManaged<PhysicsSimulationGroup>();

            // Отключаем всю физику
            physicsSimGroup.Enabled = false;
            pauseComponent.ValueRW.IsPaused = true;
            Debug.Log("Try pause");
        }
    }
    [BurstCompile]
    public void OnDestroy(ref SystemState state) {

    }
}

using Unity.Burst;
using Unity.Entities;

partial struct LaserGunSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        SystemAPI.TryGetSingleton<PauseComponent>(out var pause);
        if (pause!.IsPaused)
            return;

        foreach (RefRW<LaserGun> laserGun in SystemAPI.Query<RefRW<LaserGun>>()) {
            laserGun.ValueRW.timer += SystemAPI.Time.DeltaTime;
        }
    }

    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    {
        
    }
}

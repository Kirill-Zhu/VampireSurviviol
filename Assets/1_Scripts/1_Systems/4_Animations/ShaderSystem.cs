using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
partial struct ShadersSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        foreach(var(random, shader, entity) 
            in 
            SystemAPI.Query<
                RefRO<RandomData>,
                RefRW<ColorShaderProperty>
            >().WithEntityAccess()) {
            shader.ValueRW.Value = new float4(
                random.ValueRO.Value.NextFloat(0,10),
               random.ValueRO.Value.state,
                1, 0.2f);
        }
        foreach (var (random, shader, entity)
            in
            SystemAPI.Query<
                RefRO<RandomData>,
                RefRW<RandomSeedPropertyShader>
            >().WithEntityAccess()) {
            shader.ValueRW.Value = random.ValueRO.Value.NextFloat(1, 10);
        }
    }

    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    {
        
    }
}

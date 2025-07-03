using System.Diagnostics;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Random =  Unity.Mathematics.Random;
using UnityEngine;
partial struct RandomSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<RandomData>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {

        RandomJob job = new RandomJob();
        job.ScheduleParallel();
      
    }

    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    {
        
    }
}
public partial struct RandomJob: IJobEntity {
    public void Execute(ref RandomData random, Entity entity) {
        if (random.Value.state == 0) {
            float randomVal = math.pow(entity.Index, 2);
            random.Value.InitState((uint)randomVal);

        }
    }
}
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;


partial struct BulletSystem : ISystem {


    [BurstCompile]
    public void OnCreate(ref SystemState state) {

    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state) {

        SystemAPI.TryGetSingleton<PauseComponent>(out var pause);
        if (pause!.IsPaused)
            return;

        EntityCommandBuffer ecb = new EntityCommandBuffer(Allocator.TempJob);
        foreach ((
            RefRW<Bullet> bullet,
            RefRW<LocalTransform> localTransform,
            Entity entity)
            in
            SystemAPI.Query<
            RefRW<Bullet>,
            RefRW<LocalTransform>
            >().WithEntityAccess()) {

            if (bullet.ValueRO.Timer > bullet.ValueRO.LiveTime) {
                ecb.DestroyEntity(entity);
            }
            ;
        }


        BulletMoverJob bulletMoverJob = new BulletMoverJob() {
            deltaTime = SystemAPI.Time.DeltaTime,
        };
        bulletMoverJob.ScheduleParallel();

        ecb.Playback(state.EntityManager);
    }
    [BurstCompile]
    public void OnDestroy(ref SystemState state) {

    }

    public partial struct BulletMoverJob : IJobEntity {
        public float deltaTime;

        public void Execute(ref Bullet bullet, ref LocalTransform localTransform, Entity entity) {


            localTransform.Position += localTransform.Forward() * bullet.Speed * deltaTime;
            bullet.Timer += deltaTime;
        }
       
    }
}

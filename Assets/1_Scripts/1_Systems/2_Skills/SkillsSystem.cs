
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;
using UnityEngine;


partial struct SkillsSystem : ISystem
{
    float3 playerPos;
    Skills skills;
    private bool _ultWasPressed;
  
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
    
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state) {
        //if (PlayerInputs.Instance.UltiAction.WasPressedThisFrame()) {
        //Get Player Pos
        foreach (RefRO<LocalTransform> localTransform
            in
            SystemAPI.Query<
            RefRO<LocalTransform>
            >().WithAll<PlayerInput>()) {
            playerPos = localTransform.ValueRO.Position;
        }
        //// GetPlayer SKills
        foreach (RefRO<Skills> skills in SystemAPI.Query<RefRO<Skills>>().WithAll<PlayerInput>()) {
            this.skills.PowerUlti = skills.ValueRO.PowerUlti;
            this.skills.RangeUlti = skills.ValueRO.RangeUlti;
            this.skills.Damage = skills.ValueRO.Damage; 
            _ultWasPressed = skills.ValueRO.UltiWasPpressed;
           
        }
     
        if (_ultWasPressed) {
            var ecbSingletone = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>();
            EntityCommandBuffer.ParallelWriter ecb = ecbSingletone.CreateCommandBuffer(state.WorldUnmanaged).AsParallelWriter();
            _ultWasPressed = false;
            UltiJob ultiJob = new UltiJob() {
                ecb = ecb,
                playerPos = this.playerPos,
                skills = this.skills,
                deltaTime = SystemAPI.Time.DeltaTime,
                amountOfObjects = 0

            };
            ultiJob.ScheduleParallel();
            UltyPhysicsJob physicsJob = new UltyPhysicsJob() {
                playerPos = this.playerPos,
                skills = this.skills,
            };
            physicsJob.ScheduleParallel();
        }
        
    }

   
    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    {
        
    }
    partial struct UltiJob : IJobEntity {


        public float3 playerPos;
        public Skills skills;
        //Потом добавим
        public float deltaTime;
        public float maxAmount;
        public float amountOfObjects;
        public EntityCommandBuffer.ParallelWriter ecb;

        public void Execute([ChunkIndexInQuery] int sortKey, Entity entity, in LocalTransform localTransform, ref EnemyHealth enemyHealth) {

            float distance = math.distance(playerPos, localTransform.Position);
            if ( distance < skills.RangeUlti) {

                float3 direction = localTransform.Position - playerPos;
                //physicsVelocity.Linear += math.normalize(direction) * skills.PowerUlti;
                int intDistance = (int)distance;
                if (intDistance <= 1)
                    intDistance = 1;
                enemyHealth.Health -= skills.Damage/intDistance;
                Debug.Log("Enemy health is :"+ enemyHealth.Health);
                if(enemyHealth.Health <= 0) 
                    ecb.AddComponent<DestroyTag>(sortKey, entity);
                UnityEngine.Debug.Log("ULTIMATE");
            }
        }

    }

    partial struct UltyPhysicsJob : IJobEntity { 
        public float3 playerPos;
        public Skills skills;
        public void Execute(in LocalTransform localTransform, ref PhysicsVelocity physicsVelocity) {
            float3 direction = localTransform.Position - playerPos;
            physicsVelocity.Linear += math.normalize(direction) * skills.PowerUlti;
        }
    }
}

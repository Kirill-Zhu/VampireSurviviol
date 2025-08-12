
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;
using Unity.VisualScripting;
using UnityEngine;

[RequireMatchingQueriesForUpdate]
partial struct SkillsSystem : ISystem
{
    float3 playerPos;
    Skills skills;
    private bool _ultWasPressed;
    private bool _canUlti;
 

    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
    
    }

    [BurstCompile]
   
    public void OnUpdate(ref SystemState state) {
        var pause = SystemAPI.GetSingleton<PauseComponent>();
        if (pause.IsPaused)
            return;

        foreach (RefRW<Skills> skills in SystemAPI.Query<RefRW<Skills>>().WithAll<PlayerInput>()) {

            if (skills.ValueRO.UltiTimer < skills.ValueRO.UltiReloadTime) {
                _canUlti = false;
                skills.ValueRW.UltiTimer += SystemAPI.Time.DeltaTime;
            } else
                _canUlti = true;
            _ultWasPressed = skills.ValueRO.UltiWasPpressed;
        }
      

        if (_ultWasPressed&&_canUlti) {
            //if (PlayerInputs.Instance.UltiAction.WasPressedThisFrame()) {
            //Get Player Pos
            Debug.Log("Ulti");
            foreach (RefRO<LocalTransform> localTransform
                in
                SystemAPI.Query<
                RefRO<LocalTransform>
                >().WithAll<PlayerInput>()) {
                playerPos = localTransform.ValueRO.Position;
            }
            //// GetPlayer SKills
            foreach (RefRW<Skills> skills in SystemAPI.Query<RefRW<Skills>>().WithAll<PlayerInput>()) {
                this.skills.PowerUlti = skills.ValueRO.PowerUlti;
                this.skills.RangeUlti = skills.ValueRO.RangeUlti;
                this.skills.Damage = skills.ValueRO.Damage;
                skills.ValueRW.UltiTimer = 0;
            }

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
              
                if (distance <= 1)
                    distance = 1;
                float damage = skills.Damage / distance;
                enemyHealth.Health -= (int)damage;
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
              
            float distance = math.distance(playerPos, localTransform.Position);
            
            if (distance < skills.RangeUlti) {
                float3 direction = localTransform.Position - playerPos;
                physicsVelocity.Linear += (math.normalize(direction) * skills.PowerUlti /distance);
            }
        }
    }
}

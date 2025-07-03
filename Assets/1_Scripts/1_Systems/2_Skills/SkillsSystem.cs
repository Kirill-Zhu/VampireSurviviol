
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
            _ultWasPressed = skills.ValueRO.UltiWasPpressed;
           
        }

     
        if (_ultWasPressed) {
            _ultWasPressed = false;
            UltiJob ultiJob = new UltiJob() {
                playerPos = this.playerPos,
                skills = this.skills,
                deltaTime = SystemAPI.Time.DeltaTime,
                amountOfObjects = 0

            };
            ultiJob.ScheduleParallel();
         
        }
        
    }

   
    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    {
        
    }
}
public partial struct UltiJob : IJobEntity {

  
    public float3 playerPos;
    public Skills skills;
    //Потом добавим
    public float deltaTime;
    public float maxAmount;
    public float amountOfObjects;
    
    public void Execute(Entity entity, in LocalTransform localTransform, ref PhysicsVelocity physicsVelocity) {
        
        if (math.distance(playerPos, localTransform.Position) < skills.RangeUlti) {
           
            float3 direction = localTransform.Position - playerPos;
            physicsVelocity.Linear += math.normalize(direction) * skills.PowerUlti;
            
            UnityEngine.Debug.Log("ULTIMATE");
        }
    }

}
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

partial struct PlayerMeleAttackSystem : ISystem
{
    public bool _attackWasPressed;
    private float3 _playerPos;
    private Quaternion _playerRot;
    private float3 _playerForward;
    private int _damage;
    private float _attackRange;
    private float _attackAngle;
    private float _maxAngle;
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        _damage = 11;
        _attackAngle = 100;
    }

    //[BurstCompile]
    public void OnUpdate(ref SystemState state) {
      

        foreach (var playerMeleAttack in SystemAPI.Query<PlayerMeleAttack>()) {
            _attackWasPressed = playerMeleAttack.meleAttackWasPressed;
            _attackRange = playerMeleAttack.attackRange;
        }
        //Get Player Pos

        foreach (RefRO<LocalTransform> localTransform
            in
            SystemAPI.Query<
            RefRO<LocalTransform>
            >().WithAll<PlayerInput>()) {
            _playerPos = localTransform.ValueRO.Position;
            _playerRot = localTransform.ValueRO.Rotation;
        }
        EntityCommandBuffer ecb = new EntityCommandBuffer(Unity.Collections.Allocator.Temp);

        //if (_attackWasPressed) {
        //    Debug.Log("Mele Attack");
        //    _attackWasPressed = false;
        //    _playerForward = math.mul(_playerRot, math.forward()); ;
        //    foreach(var (enemyHealt, physicsVelocity, localTransform, entity) in SystemAPI.Query< RefRW<EnemyHealth>, RefRW<PhysicsVelocity>, RefRO<LocalTransform>>().WithEntityAccess()){

        //        //Distance and Angle check 
        //        if (math.distance(_playerPos, localTransform.ValueRO.Position) > _attackRange)
        //            continue;
        //        var directionToEneny = math.normalize(localTransform.ValueRO.Position - _playerPos);
        //        var dotProduct = math.dot(_playerForward, directionToEneny);
        //        _maxAngle = math.cos(math.radians(_attackAngle/2));


        //        if (dotProduct < _maxAngle)
        //            continue;

        //        //Do Physiscs
        //        physicsVelocity.ValueRW.Linear = math.normalize(localTransform.ValueRO.Position - _playerPos)*_damage;
            
        //        //Do VFX and Decals
        //        ecb.AddComponent(entity, new DecalComponent { decalType = DecalType.Blood });
        //        Debug.Log("Mele attack cycle");
        //        ecb.AddComponent(entity, new VFXPlayComponent { VFXType = VFXType.Blood });

        //        // Do Health Damage
        //        enemyHealt.ValueRW.Health -= _damage;
        //        if (enemyHealt.ValueRO.Health <= 0)
        //            ecb.AddComponent(entity, new DestroyTag());
        //    }
        //}
        //ecb.Playback(state.EntityManager);
        //ecb.Dispose();
        //////////////////////////////////

        if (_attackWasPressed) {
            Debug.Log("Mele Attack");
            _attackWasPressed = false;
            var ecbSingletone = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>();
            EntityCommandBuffer.ParallelWriter ecbParalellel = ecbSingletone.CreateCommandBuffer(state.WorldUnmanaged).AsParallelWriter();
            MeleAttackJob job = new MeleAttackJob { 
                ecb = ecbParalellel,
                playerPos = _playerPos,
                playerRot = _playerRot,
                
                damage = _damage,
                attackRange = _attackRange, 
                attackAngle = _attackAngle,
                maxAngle = _maxAngle,
            };

            job.ScheduleParallel();
        }
            

    }

    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    {
        
    }

    private partial struct MeleAttackJob: IJobEntity {

        public EntityCommandBuffer.ParallelWriter ecb;
        public float3 playerPos;
        public Quaternion playerRot;
        private float3 _playerForward;
       
        public int damage;
        public float attackRange;
        public float attackAngle;
        public float maxAngle;
        public void Execute([ChunkIndexInQuery] int index, Entity entity, ref EnemyHealth enemyHealth, ref PhysicsVelocity physicsVelocity, in LocalTransform localTransform) {
            _playerForward = math.mul(playerRot, math.forward());

            //Distance and Angle check 
            if (math.distance(playerPos, localTransform.Position) > attackRange)
                return;

            var directionToEneny = math.normalize(localTransform.Position - playerPos);
            var dotProduct = math.dot(_playerForward, directionToEneny);
            maxAngle = math.cos(math.radians(attackAngle / 2));


            if (dotProduct < maxAngle)
                return;

            //Do Physiscs
            physicsVelocity.Linear = math.normalize(localTransform.Position - playerPos) * damage;

            //Do VFX and Decals
            
            ecb.AddComponent(index, entity, new DecalComponent { decalType = DecalType.Blood });
            Debug.Log("Mele attack cycle");
            ecb.AddComponent(index,entity, new VFXPlayComponent { VFXType = VFXType.Blood });

            // Do Health Damage
            enemyHealth.Health -= damage;
            if (enemyHealth.Health <= 0)
                ecb.AddComponent(index, entity, new DestroyTag());

        }
    }
}

using Unity.Entities;
using Unity.Transforms;
using UnityEngine;
using Unity.Mathematics;



public partial struct EnemySpawnSystem : ISystem {


    private float _sapwnRate;
    private double _elapsedTime;
    private double _lastUpdatedTime;
    private int _maxUnits;
    private int _currentUnits;
    private Unity.Mathematics.Random _random;
    public void OnCreate(ref SystemState state) {

        _random = new Unity.Mathematics.Random((uint)System.DateTime.Now.Ticks);
        _sapwnRate = 0.03f;
        _maxUnits = 1024;
    }

    public void OnUpdate(ref SystemState state) {
        //Pause State
        SystemAPI.TryGetSingleton<PauseComponent>(out var pause);
        if (pause!.IsPaused)
            return;
        //Max units
        _currentUnits = 0;
        foreach (var unitMover in SystemAPI.Query<UnitMover>()) {
            _currentUnits++;
        }
        if (_currentUnits > _maxUnits)
            return;
        //SpawnRate
        _elapsedTime = SystemAPI.Time.ElapsedTime;
        if (_elapsedTime - _lastUpdatedTime < _sapwnRate)
            return;


        _lastUpdatedTime = _elapsedTime;
        

        SystemAPI.TryGetSingleton<EntitiesReferences>(out EntitiesReferences entitiesReferences);


        //Get Random 
        var entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

        //Enemy Spawn
        foreach (
            (RefRO<EnemySpawner> spawner,
            DynamicBuffer<EnemySpawnerBufferElement> bufferElement)
            in
            SystemAPI.Query<
            RefRO<EnemySpawner>,
            DynamicBuffer<EnemySpawnerBufferElement>
            >()) {

          
                int randomInt = _random.NextInt(0, 3);
                Debug.Log("Spawn enemy" + "Random is : " + randomInt);



            switch (randomInt) {
                case 0: {
                        //Zombie 1
                        Entity enemy = state.EntityManager.Instantiate(entitiesReferences.Zombie1Prefb);

                        if (entityManager.HasComponent<UnitMover>(enemy)) {
                            UnitMover unitMover = entityManager.GetComponentData<UnitMover>(enemy);
                            SystemAPI.SetComponent(enemy, UnitMover.SetSpeedComponents(UnityEngine.Random.Range(unitMover.MoveSpeed - 1, unitMover.MoveSpeed + 1), 5, unitMover.MinMoveSpeed, unitMover.MaxMoveSpeed));
                        }
                        foreach ((RefRO<PlayerMover> playerMover, RefRO<LocalTransform> localTransofrm)
                            in
                            SystemAPI.Query<
                            RefRO<PlayerMover>,
                            RefRO<LocalTransform>
                            >()) {
                            SystemAPI.SetComponent(enemy, LocalTransform.FromPosition(NearestToPlayerSpawnPos(localTransofrm.ValueRO.Position, bufferElement)));
                        }
                        // SystemAPI.SetComponent(enemy, UnitMover.SetSpeedComponents(UnityEngine.Random.Range(8, 10), 5), );
                        break;
                }
                case 1: {
                        //Zombie 2
                        Entity enemy = state.EntityManager.Instantiate(entitiesReferences.Zombie2Prefb);
                        if (entityManager.HasComponent<UnitMover>(enemy)) {
                            UnitMover unitMover = entityManager.GetComponentData<UnitMover>(enemy);
                            SystemAPI.SetComponent(enemy, UnitMover.SetSpeedComponents(UnityEngine.Random.Range(unitMover.MoveSpeed - 1, unitMover.MoveSpeed + 1), 5, unitMover.MinMoveSpeed, unitMover.MaxMoveSpeed));
                        }
                        foreach ((RefRO<PlayerMover> playerMover, RefRO<LocalTransform> localTransofrm)
                            in
                            SystemAPI.Query<
                            RefRO<PlayerMover>,
                            RefRO<LocalTransform>
                            >()) {
                            SystemAPI.SetComponent(enemy, LocalTransform.FromPosition(NearestToPlayerSpawnPos(localTransofrm.ValueRO.Position, bufferElement)));
                        }

                        break;
                }
                case 2: {
                        //Jumper
                        Entity enemy = state.EntityManager.Instantiate(entitiesReferences.Jumper);
                        if (entityManager.HasComponent<UnitMover>(enemy)) {
                            UnitMover unitMover = entityManager.GetComponentData<UnitMover>(enemy);
                            SystemAPI.SetComponent(enemy, UnitMover.SetSpeedComponents(UnityEngine.Random.Range(unitMover.MoveSpeed - 1, unitMover.MoveSpeed + 1), 5, unitMover.MinMoveSpeed, unitMover.MaxMoveSpeed));
                        }
                        foreach ((RefRO<PlayerMover> playerMover, RefRO<LocalTransform> localTransofrm)
                            in
                            SystemAPI.Query<
                            RefRO<PlayerMover>,
                            RefRO<LocalTransform>
                            >()) {
                            SystemAPI.SetComponent(enemy, LocalTransform.FromPosition(NearestToPlayerSpawnPos(localTransofrm.ValueRO.Position, bufferElement)));
                        }
                        // SystemAPI.SetComponent(enemy, UnitMover.SetSpeedComponents(UnityEngine.Random.Range(8, 10), 5), );  

                        break;
                }

            }
           }
        
    }

   public static float3 NearestToPlayerSpawnPos(float3 PlayerPos, DynamicBuffer<EnemySpawnerBufferElement> bufferElement) {
       

        float3 nearestPos = float3.zero;
        float distance = 1;

        for (int i = 0; i < bufferElement.Length; i++) {
            if (i == 0) {
                distance = math.distance(PlayerPos, bufferElement[i].Value);
                nearestPos = bufferElement[i].Value;
                continue;
            }
            if (math.distance(PlayerPos, bufferElement[i].Value) < distance) {
                distance = math.distance(PlayerPos, bufferElement[i].Value);
                nearestPos = bufferElement[i].Value;
            }     
        }
        return nearestPos;
       
   }
     
    
}


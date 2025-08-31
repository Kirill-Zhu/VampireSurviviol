using Unity.Entities;
using Unity.Transforms;
using UnityEngine;
using Unity.Mathematics;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Burst;


public partial struct EnemySpawnSystem : ISystem {
    /// <summary>
    /// Дистанция спавна внизу в методе RandomNearSpawnPos
    /// </summary>

    private float _spawnRate;
    private double _elapsedTime;
    private double _lastUpdatedTime;
    private float _TimeToUpSpawnRate;
    private float _upSpawnRateElapsedTime;
    private int _maxUnits;
    private int _currentUnits;
    private Unity.Mathematics.Random _random;
    public void OnCreate(ref SystemState state) {

        _random = new Unity.Mathematics.Random((uint)System.DateTime.Now.Ticks);
        _spawnRate = 0.5f;
        _maxUnits = 600;
        _TimeToUpSpawnRate = 15;
}

    [BurstCompile]
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
        if(_upSpawnRateElapsedTime >=_TimeToUpSpawnRate) {
            _upSpawnRateElapsedTime = 0;
            
            _spawnRate -= 0.05f;
            if(_spawnRate <= 0) {
                _spawnRate = 0.03f;
            }
        }
        _upSpawnRateElapsedTime += SystemAPI.Time.DeltaTime;        
        //Check Reload Spawn
        _elapsedTime = SystemAPI.Time.ElapsedTime;
        if (_elapsedTime - _lastUpdatedTime < _spawnRate)
            return;


        //---------------------------------------------------------------------


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
                            SystemAPI.SetComponent(enemy, LocalTransform.FromPosition(RandomNearSpawnPos(localTransofrm.ValueRO.Position, bufferElement, _random)));
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
                            SystemAPI.SetComponent(enemy, LocalTransform.FromPosition(RandomNearSpawnPos(localTransofrm.ValueRO.Position, bufferElement, _random)));
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
                            SystemAPI.SetComponent(enemy, LocalTransform.FromPosition(RandomNearSpawnPos(localTransofrm.ValueRO.Position, bufferElement, _random)));
                        }
                        // SystemAPI.SetComponent(enemy, UnitMover.SetSpeedComponents(UnityEngine.Random.Range(8, 10), 5), );  

                        break;
                }

            }
           }
        
    }

   public static float3 RandomNearSpawnPos(float3 PlayerPos, DynamicBuffer<EnemySpawnerBufferElement> bufferElement, Unity.Mathematics.Random random) {


        List<float3> nearestPos = new List<float3>();
        float distance = 50;

        for (int i = 0; i < bufferElement.Length; i++) {
            if (i == 0) {
                nearestPos.Add(bufferElement[i].Value);
                continue;
            }
            if (math.distance(PlayerPos, bufferElement[i].Value) < distance) {
                nearestPos.Add(bufferElement[i].Value);
            }
        }

        return nearestPos[random.NextInt(0, nearestPos.Count)];
       
   }
     
    
}


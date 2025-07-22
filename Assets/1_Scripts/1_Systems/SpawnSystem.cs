using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;
using Unity.Mathematics;
using Unity.VisualScripting;


partial struct EnemySpawnSystem : ISystem
{
 

    
    private double _elapsedTime;
    private double _lastUpdatedTime;

    private Unity.Mathematics.Random _random;
    public void OnCreate(ref SystemState state) {

        _random = new Unity.Mathematics.Random((uint)System.DateTime.Now.Ticks);
    }
   
    public void OnUpdate(ref SystemState state){


        SystemAPI.TryGetSingleton<EntitiesReferences>(out EntitiesReferences entitiesReferences);
      

        //Get Random Value

        _elapsedTime = SystemAPI.Time.ElapsedTime;
        var entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
        //Enemy Spawn
        foreach (
            RefRO<EnemySpawner> spawner
            in
            SystemAPI.Query<
            RefRO<EnemySpawner>
            >()) {

            if (Input.GetKey(KeyCode.Space)) {
                int randomInt = _random.NextInt(0, 2);
                Debug.Log("Spawn enemy" + "Random is : "+randomInt);
                switch (randomInt) {
                    case 0: {

                            //Zombie 1
                            Entity enemy = state.EntityManager.Instantiate(entitiesReferences.Zombie1Prefb);

                            if (entityManager.HasComponent<UnitMover>(enemy)) {
                                UnitMover unitMover = entityManager.GetComponentData<UnitMover>(enemy);
                                SystemAPI.SetComponent(enemy, UnitMover.SetSpeedComponents(UnityEngine.Random.Range(unitMover.MoveSpeed -1, unitMover.MoveSpeed+1), 5, unitMover.MinMoveSpeed, unitMover.MaxMoveSpeed));
                            }

                            SystemAPI.SetComponent(enemy, LocalTransform.FromPosition(spawner.ValueRO.SpawnPos));
                           
                           
                            break;
                        }
                    case 1: {
                            //Zombie 2
                            Entity enemy = state.EntityManager.Instantiate(entitiesReferences.Zombie2Prefb);
                            SystemAPI.SetComponent(enemy, LocalTransform.FromPosition(spawner.ValueRO.SpawnPos));
                           // SystemAPI.SetComponent(enemy, UnitMover.SetSpeedComponents(UnityEngine.Random.Range(8, 10), 5), );
                            break;
                        }
                }

            }
            
        }


    }

}

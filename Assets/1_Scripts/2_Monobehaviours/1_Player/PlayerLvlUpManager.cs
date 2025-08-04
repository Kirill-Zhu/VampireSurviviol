using System.Collections;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerLvlUpManager : MonoBehaviour
{
    public static PlayerLvlUpManager Instance;
    [SerializeField] private UnityEngine.InputSystem.PlayerInput _playerInput;
    public bool LvlUpUIIsOpened = false;//Controlled by PlayerLvlUpUi
    [SerializeField] private PlayerLvlUpUI _lvlUpUI;

    private int _currentLvl=1;
    private int _needLvl=1;
    private IEnumerator _coroutine;
    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this.gameObject);
    }

    public void LvlUp() {
        _needLvl++;
        if(_coroutine!=null)    
            StopCoroutine(_coroutine);

        _coroutine = LvlUpCoroutine();
        StartCoroutine(_coroutine);
    }
    public void LvlUp(int needLvl) {
        _needLvl = needLvl;

    }
    private IEnumerator LvlUpCoroutine() {

        while (_needLvl > _currentLvl) { 
        
            if(LvlUpUIIsOpened)
                yield return null;

            _currentLvl++;
            _lvlUpUI.gameObject.SetActive(true);
          
            _playerInput.SwitchCurrentActionMap("UI");
            
            PauseEntityWorld();
        }

    }
    public void ResumeGame() {
        RunEntityWorld();
    }
    private void PauseEntityWorld() {
        EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager; // Create Entity world manager

        EntityQuery entityQuery = new EntityQueryBuilder(Allocator.Temp).WithAll<PauseComponent>().Build(entityManager); //Grab all entities with UnitMover in array and create Query
        NativeArray<Entity> entityArray = entityQuery.ToEntityArray(Allocator.Temp);// Grab all the ehntities form entityQuery

        NativeArray<PauseComponent> unitMoverArray = entityQuery.ToComponentDataArray<PauseComponent>(Allocator.Temp); //Grab all unitMover data form entities to Native Array
        for (int i = 0; i < unitMoverArray.Length; i++) {
            PauseComponent pauseComponent = unitMoverArray[i];
            pauseComponent.IsPaused = true;
            entityManager.SetComponentData(entityArray[i], pauseComponent);

        }
    }
    private void RunEntityWorld() {
        EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager; // Create Entity world manager

        EntityQuery entityQuery = new EntityQueryBuilder(Allocator.Temp).WithAll<PauseComponent>().Build(entityManager); //Grab all entities with UnitMover in array and create Query
        NativeArray<Entity> entityArray = entityQuery.ToEntityArray(Allocator.Temp);// Grab all the ehntities form entityQuery

        NativeArray<PauseComponent> unitMoverArray = entityQuery.ToComponentDataArray<PauseComponent>(Allocator.Temp); //Grab all unitMover data form entities to Native Array
        for (int i = 0; i < unitMoverArray.Length; i++) {
            PauseComponent pauseComponent = unitMoverArray[i];
            pauseComponent.IsPaused = false;
            entityManager.SetComponentData(entityArray[i], pauseComponent);

        }
    }
    [ContextMenu("LvlUp Speed")]
    public void LvlUpMoveSpeed() {
        EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager; // Create Entity world manager

        EntityQuery entityQuery = new EntityQueryBuilder(Allocator.Temp).WithAll<PlayerMover>().Build(entityManager); //Grab all entities with UnitMover in array and create Query
        NativeArray<Entity> entityArray = entityQuery.ToEntityArray(Allocator.Temp);// Grab all the ehntities form entityQuery

        NativeArray<PlayerMover> unitMoverArray = entityQuery.ToComponentDataArray<PlayerMover>(Allocator.Temp); //Grab all unitMover data form entities to Native Array
        for (int i = 0; i < unitMoverArray.Length; i++) {
            PlayerMover unitMover = unitMoverArray[i];
            unitMover.MoveSpeed += 1;
            entityManager.SetComponentData(entityArray[i], unitMover);

        }
    }
    [ContextMenu("LvlUp Rate of fire")]
    public void lvlUpRateOfFire() {
        EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager; // Create Entity world manager
        EntityQuery entityQuery = new EntityQueryBuilder(Allocator.Temp).WithAll<ControlledGun>().Build(entityManager);
        NativeArray<Entity> entityArray = entityQuery.ToEntityArray(Allocator.Temp);

        NativeArray<ControlledGun> controlledGunArray = entityQuery.ToComponentDataArray<ControlledGun>(Allocator.Temp);
        for (int i = 0; i < controlledGunArray.Length; i++) {
            ControlledGun gun = controlledGunArray[i];
            gun.RateOfFire -= 0.02f;
            entityManager.SetComponentData(entityArray[i], gun);
           
        }
    }
    [ContextMenu("LvlUp Projectile Damage of fire")]
    public void LvlUpProjectileDamage() {
        EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager; // Create Entity world manager
        EntityQuery entityQuery = new EntityQueryBuilder(Allocator.Temp).WithAll<ControlledGun>().Build(entityManager);
        NativeArray<Entity> entityArray = entityQuery.ToEntityArray(Allocator.Temp);

        NativeArray<ControlledGun> controlledGunArray = entityQuery.ToComponentDataArray<ControlledGun>(Allocator.Temp);
        for (int i = 0; i < controlledGunArray.Length; i++) {
            ControlledGun gun = controlledGunArray[i];
            gun.Damage += 1;
            entityManager.SetComponentData(entityArray[i], gun);

        }
    }
    [ContextMenu("LvlUp Ulty Rnge of fire")]
    public void LvlUpUltiRange() {
        EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager; // Create Entity world manager
        EntityQuery entityQuery = new EntityQueryBuilder(Allocator.Temp).WithAll<Skills>().Build(entityManager);
        NativeArray<Entity> entityArray = entityQuery.ToEntityArray(Allocator.Temp);

        NativeArray<Skills> controlledGunArray = entityQuery.ToComponentDataArray<Skills>(Allocator.Temp);
        for (int i = 0; i < controlledGunArray.Length; i++) {
            Skills skills = controlledGunArray[i];
            skills.RangeUlti += 1;
            entityManager.SetComponentData(entityArray[i], skills);

        }
    }
}

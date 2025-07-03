using Unity.Collections;
using Unity.Entities;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputs : MonoBehaviour
{
    public static PlayerInputs Instance;
    [SerializeField] private Vector2 _moveInputVector;
    [SerializeField] private Vector3 _cameraVector;
    public Vector3 moveVector;
    private InputAction _moveAction;

    [Header("Camera Inputs")]
    [Space(2)]
    private Vector2 _cameraLookVector;
    public Vector3 CameraLookVector => _cameraLookVector;
    private InputAction _cameraLookAction;
    [Header("Skills Inputs")]
    [Space(2)]
    public bool UltiWasPressed = false;
    public InputAction UltiAction;

    private void Awake() {

        if (Instance == null)
            Instance = this;
        else
            Destroy(this.gameObject);

        _moveAction = InputSystem.actions.FindAction("Move");
        _cameraLookAction = InputSystem.actions.FindAction("Look");
        UltiAction = InputSystem.actions.FindAction("Ulti");

    }

    private void Update() {
        //Move Vector reads
        _moveInputVector = _moveAction.ReadValue<Vector2>();
       
        //Camera Inputs Read
        _cameraLookVector = _cameraLookAction.ReadValue<Vector2>();



        _cameraVector = Camera.main.transform.forward;
        moveVector = _cameraVector *_moveInputVector.y;
        moveVector += Camera.main.transform.right * _moveInputVector.x;
        moveVector.y = 0;

        moveVector.Normalize();

        //Ulti Inputs Rea
        if (UltiAction.WasPressedThisFrame())
            UltiWasPressed = true;
        
                                                  //Entities
                                                  //To Unit Movers with 
        
        EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager; // Create Entity world manager

        EntityQuery entityQuery = new EntityQueryBuilder(Allocator.Temp).WithAll<PlayerMover>().Build(entityManager); //Grab all entities with UnitMover in array and create Query
        NativeArray<Entity> entityArray = entityQuery.ToEntityArray(Allocator.Temp);// Grab all the ehntities form entityQuery
        
        NativeArray<PlayerMover> unitMoverArray = entityQuery.ToComponentDataArray<PlayerMover>(Allocator.Temp); //Grab all unitMover data form entities to Native Array
        for (int i = 0; i < unitMoverArray.Length; i++) { 
            PlayerMover unitMover = unitMoverArray[i];
            unitMover.InputTargetPosition = moveVector;
            entityManager.SetComponentData(entityArray[i], unitMover);
        }
        // Skills


        entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
        entityQuery = new EntityQueryBuilder(Allocator.Temp).WithAll<Skills>().Build(entityManager);
        entityArray = entityQuery.ToEntityArray(Allocator.Temp);    

        NativeArray<Skills> skillsAray = entityQuery.ToComponentDataArray<Skills>(Allocator.Temp);
        for (int i = 0; i < skillsAray.Length; i++) { 
            Skills skills = skillsAray[i];
            skills.UltiWasPpressed = UltiWasPressed;
            entityManager.SetComponentData(entityArray[i], skills);
            if(UltiWasPressed)
                UltiWasPressed = false;
        }
        
            
       
    }

}

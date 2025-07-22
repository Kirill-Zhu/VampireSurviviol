using TMPro;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAnimationMonobeh : MonoBehaviour
{
   
    private Animator _animator;
    private Vector3 _cameraForward;
    private Vector3 _cameraRight;
    private Vector3 _moveVector;
    public float _YDotRotation;
    public float _XDotRotation;
    private NativeArray<float> _XYDotRotation;
    private PlayerInputs _playerInputs;
    private Camera _camera;
    private JobHandle _jobHandle;
    private void Awake() {
        _animator = GetComponent<Animator>();
        _XYDotRotation = new NativeArray<float> (2,Allocator.Persistent);
    }
    // Update is called once per frame
    void Update()
    {
        _playerInputs = PlayerInputs.Instance;
        _camera = Camera.main;

        //Set Values
        transform.rotation = Quaternion.LookRotation(_playerInputs.RotationVector, Vector3.up);

        _moveVector = _playerInputs.moveVector;
        //_moveVector.y = 0;

        _cameraRight = _camera.transform.right;
        //_cameraRight.y = 0;
        //_XDotRotation = Vector3.Dot(transform.right, _moveVector);

        _cameraForward = _camera.transform.forward;
        //_cameraForward.y = 0;
        //_YDotRotation = Vector3.Dot(transform.forward, _moveVector);





        var MoveJob = new MoveJob {
            
            XYDotProducts = _XYDotRotation,
            CameraForward = _cameraForward,
            CameraRight = _cameraRight,
            
            TransforwForward = transform.forward,
            TransformRight = transform.right,
          
            MoveVector = _moveVector,
        };
        _jobHandle = MoveJob.Schedule();
        JobHandle.ScheduleBatchedJobs();

        
    }
    private void LateUpdate() {
        
        _jobHandle.Complete();


        _animator.SetFloat("XMoveAxis", -_XYDotRotation[0]);
        _animator.SetFloat("YMoveAxis", -_XYDotRotation[1]);
        if (_playerInputs.UltiAction.WasPressedThisFrame()) {
            _animator.CrossFade("Ulti", 0.05f, 1);
            _animator.CrossFade("Ulti", 0.05f, 2);
        }

    }
    private struct MoveJob : IJob {
        public float XDotPorduct;
        public float YDotPorduct;
        public NativeArray<float> XYDotProducts;

        public Vector3 CameraForward;
        public Vector3 CameraRight;

        public Vector3 TransforwForward;
        public Vector3 TransformRight;
        
        public Vector3 MoveVector;
        public void Execute() {
        
            MoveVector.y = 0;
            CameraRight.y = 0;
            CameraForward.y = 0;
            
            XDotPorduct = Vector3.Dot(TransformRight, MoveVector);
            YDotPorduct = Vector3.Dot(TransforwForward, MoveVector);

            XYDotProducts[0] = XDotPorduct;
            XYDotProducts[1] = YDotPorduct;   
        }
    }
}

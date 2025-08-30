using UnityEngine;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Jobs;

public class VFXSpawner : MonoBehaviour
{
    [SerializeField] private List<VFXHandler> _particlesList;
    private int _arrayLength = 200;
    private NativeArray<float> _particleTimers;


    #region Singleton
    public static VFXSpawner Instance;
    private void Awake() {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this.gameObject);
        #endregion
        //Awake
        _particleTimers = new NativeArray<float>(_arrayLength, Allocator.Persistent);
    }

    private void Start() {
    
    }

    private void Update() {
       
        Job job = new Job {
            timers = _particleTimers,
            liveTime = _particlesList[0].LiveTime,
            deltaTime = Time.deltaTime,
        };
        JobHandle jobHandle = job.Schedule(_arrayLength, 3); // Обработать _arrayLength элементов, по 3 в пакете
       

        jobHandle.Complete();

        for (int i = 0; i < _particlesList.Count; i++) {
            if (!_particlesList[i].gameObject.activeInHierarchy)
                continue;
            

            _particlesList[i].Timer = _particleTimers[i];
            if (_particlesList[i].Timer >= _particlesList[i].LiveTime) {
                _particlesList[i].gameObject.SetActive(false);
            }
        }
       
    }
    private void LateUpdate() {
       
    }
    public void PlayParticle(VFXType VfxType, Vector3 pos) {
        for (int i = 0; i <_particlesList.Count; i++) { 
            if( _particlesList[i].gameObject.activeInHierarchy) 
                continue;

            _particlesList[i].Timer = 0;
            _particleTimers[i] = 0;
            _particlesList[i].gameObject.SetActive(false);
            _particlesList[i].gameObject.SetActive(true);
            _particlesList[i].transform.position = pos;
            break;
        }
    }

    private struct Job : IJobParallelFor {
        public NativeArray<float> timers;
        public float liveTime;
        public float deltaTime;
        public void Execute(int index) {
            if (timers[index] <= liveTime)
                timers[index] += deltaTime;
            
            
            
        }
    }
}

public enum VFXType{
    Blood = 0,

}
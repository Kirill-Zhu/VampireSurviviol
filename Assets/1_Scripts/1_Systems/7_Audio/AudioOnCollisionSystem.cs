using System.Diagnostics;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEditor.SceneManagement;

public partial struct AudioOnCollisionSystem : ISystem {

    private double _lastUpdateTime;
    private float _soundRate;

    private bool _stopIterate;
    //private BeginInitializationEntityCommandBufferSystem _ecbSystem;
    private void OnCreate(ref SystemState state) {
        _soundRate = 0.01f;
    }
    
   
    [BurstCompile]
    public void OnUpdate(ref SystemState state) {

        double currentTime = SystemAPI.Time.ElapsedTime;
      
        if (currentTime - _lastUpdateTime < _soundRate) {
            return;
        }
        _lastUpdateTime = currentTime;
        _stopIterate = false;
       
        var ecb = new EntityCommandBuffer(Allocator.Temp);
        foreach (var buffer in SystemAPI.Query<DynamicBuffer<AudioReferencesBufferElement>>()) {
            if(_stopIterate) 
                break;
            
            foreach ((RefRW<AudioOnCollision> audio,
                RefRO<LocalTransform> localTransform,
                Entity entity) 
                in SystemAPI.Query<
                  RefRW<AudioOnCollision>,
                  RefRO<LocalTransform>
                >().WithEntityAccess()) {
                    if (_stopIterate) 
                        break;
                    if (audio.ValueRO.ShouldPlay) {
                        
                        foreach(var element in buffer) { 
                            if (_stopIterate)
                                break;
                            Entity newEntity = ecb.Instantiate(element.Value);
                            ecb.SetComponent(newEntity, LocalTransform.FromPosition(localTransform.ValueRO.Position));
                            _stopIterate = true;
                        }
                        //Entity newEtity2 = ecb.Instantiate(audioReferences.SoundPrefabsArray[0]);
                        audio.ValueRW.ShouldPlay = false;
                    }
            }
        }
        ecb.Playback(state.EntityManager);
        ecb.Dispose();
    }
}

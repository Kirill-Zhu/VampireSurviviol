using Unity.Entities;
using UnityEngine;
using Unity.Mathematics;
using UnityEditor;

class EnemySpawnerAuthoring : MonoBehaviour
{
    public Vector3[] SpawnPosArray;
  
   
    class SpawnerAuthoringBaker : Baker<EnemySpawnerAuthoring> {
        
        
        public override void Bake(EnemySpawnerAuthoring authoring) {

           
            Entity entity = GetEntity(TransformUsageFlags.None);
            AddComponent(entity, new EnemySpawner { });
            var buffer = AddBuffer<EnemySpawnerBufferElement>(entity);
            foreach (var authoringPos in authoring.SpawnPosArray) {
                buffer.Add(new EnemySpawnerBufferElement { Value = authoringPos});
            }
        }
    }
    private void OnDrawGizmos() {
        Gizmos.color = Color.orangeRed;

        for (int i = 0; i < SpawnPosArray.Length; i++) {
            Gizmos.DrawSphere(SpawnPosArray[i], 0.5f);
#if UNITY_EDITOR
            Handles.Label(SpawnPosArray[i] + Vector3.up, "Spawn Posion N: " + i);
#endif
        }

    }
}
public struct EnemySpawner : IComponentData {
}
public struct EnemySpawnerBufferElement: IBufferElementData {
    public float3 Value;
}

using Unity.Entities;
using UnityEngine;
using Unity.Mathematics;
using UnityEditor;

class EnemySpawnerAuthoring : MonoBehaviour
{
    public Vector3 SpawnPos;
  
   
    class SpawnerAuthoringBaker : Baker<EnemySpawnerAuthoring> {
        
        
        public override void Bake(EnemySpawnerAuthoring authoring) {

           
            Entity entity = GetEntity(TransformUsageFlags.None);
            AddComponent(entity, new EnemySpawner { SpawnPos = authoring.SpawnPos});
        }
    }

}
public struct EnemySpawner : IComponentData {
    public float3 SpawnPos;
 
}

using Chartacters;
using Unity.Entities;
using Unity.Rendering;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

class EntitiesReferencesAuthoring : MonoBehaviour
{
    public GameObject EnemyPrefab;

    [Header("Bullets")]
    [Space(2)]
    public GameObject Bullet;
    [Header("Exp")]
    public GameObject ExpCystal;
    
    class EntetiesReferencesAuthoringBaker : Baker<EntitiesReferencesAuthoring> {
        public override void Bake(EntitiesReferencesAuthoring authoring) {
            Debug.Log("Bake");
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);

            
           
            AddComponent(entity, new EntitiesReferences { EnemyPrefab = GetEntity(authoring.EnemyPrefab, TransformUsageFlags.Dynamic),
            Bullet = GetEntity(authoring.Bullet, TransformUsageFlags.Dynamic)
            });
        }
    }

}


public struct EntitiesReferences: IComponentData {
    public Entity EnemyPrefab;
    public Entity Bullet;
}
using Chartacters;
using LVLUP;
using Unity.Entities;
using Unity.Rendering;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

class EntitiesReferencesAuthoring : MonoBehaviour
{
    [Header("Enemies Prefabs")]
    public GameObject Zombie1Prefeb;
    public GameObject Zombie2Prefeb;
    [Header("Exp")]
    public GameObject ExpCystal;
    
    class EntetiesReferencesAuthoringBaker : Baker<EntitiesReferencesAuthoring> {
        public override void Bake(EntitiesReferencesAuthoring authoring) {
            Debug.Log("Bake");
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);

            
           
            AddComponent(entity, new EntitiesReferences { Zombie1Prefb = GetEntity(authoring.Zombie1Prefeb, TransformUsageFlags.Dynamic),
            Zombie2Prefb = GetEntity(authoring.Zombie2Prefeb, TransformUsageFlags.Dynamic),
            ExpCystal = GetEntity(authoring.ExpCystal, TransformUsageFlags.Dynamic)
            });
        }
    }

}


public struct EntitiesReferences: IComponentData {
    public Entity Zombie1Prefb;
    public Entity Zombie2Prefb;


    public Entity ExpCystal;
}
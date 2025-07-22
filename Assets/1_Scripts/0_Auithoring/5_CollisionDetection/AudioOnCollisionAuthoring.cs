using Unity.Entities;
using UnityEngine;

public class AudioOnCollisionAuthoring : MonoBehaviour {
    public GameObject Clip;//≈сли нужно несколько источинков звука
    public float Volume;
    public bool SholdPlay;  
    public  TypeOfCollisionClip TypeOfCollisionClip;
    class Baker : Baker<AudioOnCollisionAuthoring> {
        public override void Bake(AudioOnCollisionAuthoring authoring) {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new AudioOnCollision { 
                audioClipEntity = GetEntity(authoring.Clip, TransformUsageFlags.Dynamic),
                Volume = authoring.Volume,
                ShouldPlay = authoring.SholdPlay,
                typeOfCollisionClip = authoring.TypeOfCollisionClip
            });
        }
    }
}
public struct AudioOnCollision : IComponentData
{
    public Entity audioClipEntity;
    public float Volume;
    public bool ShouldPlay;
    public TypeOfCollisionClip typeOfCollisionClip;
}

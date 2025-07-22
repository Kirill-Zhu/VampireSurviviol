using Unity.Collections;
using Unity.Entities;
using UnityEngine;

public class AudioReferencesAuthoring : MonoBehaviour
{
    public GameObject[] SoundPreffbsArray;
    public float PlayerProjectileSoundPrefabLiveTime;
    class Baker : Baker<AudioReferencesAuthoring> {
        public override void Bake(AudioReferencesAuthoring authoring) {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);

            var buffer = AddBuffer<AudioReferencesBufferElement>(entity);
            foreach (var prefab in authoring.SoundPreffbsArray) {
                buffer.Add(new AudioReferencesBufferElement {
                    Value = GetEntity(prefab, TransformUsageFlags.Dynamic)
                });
            }
        }
    }

}

public struct AudioReferencesBufferElement : IBufferElementData {
    public Entity Value;
}
public enum TypeOfCollisionClip {
    PlayerPojectile = 1
}
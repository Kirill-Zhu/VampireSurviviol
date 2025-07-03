using Unity.Entities;
using UnityEngine;

class PlayerInputAuthoring : MonoBehaviour
{
  
    class PlayerInputAuthoringBaker : Baker<PlayerInputAuthoring> {
        public override void Bake(PlayerInputAuthoring authoring) {
            Entity entity = GetEntity(TransformUsageFlags.None);
            AddComponent(entity, new PlayerInput { });
        }
    }
}

public struct PlayerInput: IComponentData {

   public bool UltWasPressed;
}

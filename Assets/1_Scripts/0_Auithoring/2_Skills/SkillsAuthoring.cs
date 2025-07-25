using Unity.Entities;
using UnityEngine;

public class SkillsAuthoring : MonoBehaviour
{
    public float RangeUlti =10;
    public float PowerUlti = 10;
    public int Damage = 15;

    public class Baker : Baker<SkillsAuthoring> {
        public override void Bake(SkillsAuthoring authoring) {
            Entity entity = GetEntity(TransformUsageFlags.None);
            AddComponent(entity, new Skills {
                RangeUlti = authoring.RangeUlti,
                PowerUlti = authoring.PowerUlti,
                Damage = authoring.Damage
            });
        }
    }
}
public struct Skills: IComponentData {
    public float RangeUlti;
    public float PowerUlti;
    public int Damage;
    public bool UltiWasPpressed;
}
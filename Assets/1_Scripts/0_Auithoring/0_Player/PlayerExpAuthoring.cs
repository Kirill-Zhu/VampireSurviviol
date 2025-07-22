using Unity.Entities;
using UnityEngine;

public class PlayerExpAuthoring : MonoBehaviour
{
    public int Lvl = 1;
    public int Exp = 0;
    class Baker : Baker<PlayerExpAuthoring> {
        public override void Bake(PlayerExpAuthoring authoring) {
            Entity entity = GetEntity(TransformUsageFlags.None);
            AddComponent(entity, new PlayerExp { Exp = 1, Lvl = authoring.Lvl });
        }
    }
}
public struct PlayerExp : IComponentData {
    public int Lvl;
    public int Exp;

    public void AddExp(int value) {
        Exp += value;
        while (Exp > Lvl) {
            Exp -= Lvl;
            Lvl++;
        }
    }
}
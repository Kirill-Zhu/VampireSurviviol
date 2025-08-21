using Unity.Entities;
using UnityEngine;

public class PlayerMeleAttackAuthoring : MonoBehaviour {
    [SerializeField] private float _attackRate;
    [SerializeField] private float _attackRange;
    [SerializeField] private int _damage;

    class Baker : Baker<PlayerMeleAttackAuthoring> {
        public override void Bake(PlayerMeleAttackAuthoring authoring) {
            Entity entity = GetEntity(TransformUsageFlags.None);
            AddComponent(entity, new PlayerMeleAttack { 
                attackRate = authoring._attackRate, 
                attackRange = authoring._attackRange,
                damage = authoring._damage });
        }
    }
}
public struct PlayerMeleAttack : IComponentData{

    public float attackRate;
    public float attackRange;
    public int damage;
    public bool meleAttackWasPressed;
}

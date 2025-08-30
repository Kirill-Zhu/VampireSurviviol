using Unity.Entities;
using UnityEngine;

public class ControlledGunAuthoring: MonoBehaviour {
    public GameObject BulletPrefab;
    public GunType GunType;
    public BulletType TypeOfBullet;
    public float ReloadTimer;
    public float RateOfFire;
    public int Damage;

    public class Baker : Baker<ControlledGunAuthoring> {
        public override void Bake(ControlledGunAuthoring authoring) {
            Entity entity = GetEntity(TransformUsageFlags.None);
            AddComponent(entity, new ControlledGun {
                BulletPrefab = GetEntity(authoring.BulletPrefab, TransformUsageFlags.Dynamic),
                gunType = authoring.GunType,
                bulletType = authoring.TypeOfBullet, 
                ReloadTimer = authoring.ReloadTimer,
                RateOfFire = authoring.RateOfFire,
                Damage = authoring.Damage,  
            });
        }
    }

}
public struct ControlledGun : IComponentData
   {public Entity BulletPrefab;
    public GunType gunType;
    public BulletType bulletType;
    public float ReloadTimer;
    public float RateOfFire;
    public int Damage;
    public bool IsSootingButtonDown;
}
public enum BulletType {
    None = 0,   
    Pistol = 1,
    Shootgun = 2,
    Lasergun =3,
}
public enum GunType {
    None =0,
    Pistol = 1,
    Shootgun = 2,
    Lasergun = 3,
}
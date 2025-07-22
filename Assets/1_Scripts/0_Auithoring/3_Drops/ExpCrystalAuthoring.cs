using Unity.Entities;
using UnityEngine;


namespace LVLUP {

    public class ExpCrystalAuthoring : MonoBehaviour {
        public int Exp = 1;
        public TypeOfExpCystal TypeOfExpCrystal;
        class BakeExpCrystal : Baker<ExpCrystalAuthoring> {
            public override void Bake(ExpCrystalAuthoring authoring) {
                Entity entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity, new ExpCrystal { Exp = (int)authoring.TypeOfExpCrystal, TypeOfExpCrystal = authoring.TypeOfExpCrystal });
            }
        }
    }

    public struct ExpCrystal : IComponentData {
        public int Exp;
        public TypeOfExpCystal TypeOfExpCrystal;
    }
    public enum TypeOfExpCystal {
        None = 0,
        blue = 1,
        green = 10,
        Red  = 50
    }
}
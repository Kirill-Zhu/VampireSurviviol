using Unity.Entities;
using Unity.Rendering;
using UnityEngine;
using Unity.Mathematics;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.UIElements.Experimental;
public class ColorShaderPropertyAuthoring : MonoBehaviour
{
    public float4 Value;
    class Baker: Baker<ColorShaderPropertyAuthoring> {
      
public override void Bake(ColorShaderPropertyAuthoring authoring) {
            Entity entity = GetEntity(TransformUsageFlags.None);
            AddComponent(entity, new ColorShaderProperty { Value = authoring.Value});
        }
    }
}
[MaterialProperty("_BaseColor")]
public struct ColorShaderProperty: IComponentData {
    public float4 Value;
}
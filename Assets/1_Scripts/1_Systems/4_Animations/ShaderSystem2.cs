using UnityEngine;
using Unity.Entities;
using Unity.Rendering;
public  partial class ShaderSystem2: SystemBase
{
    protected override void OnUpdate() {
      Entities.ForEach((ref RenderMeshUnmanaged renderMesh)=>{
          var propertyBlock = new MaterialPropertyBlock();
          

        }).WithoutBurst().Run();
    }

    
}

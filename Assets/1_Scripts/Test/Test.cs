using UnityEngine;
using Zenject;

public class Test : MonoBehaviour
{
    [Inject]
    public DecalsSpawner playerSettings;
    private MeshFilter _meshFilter;
    public Mesh mesh;
    private void Awake() {
        _meshFilter = GetComponent<MeshFilter>();   
        
        _meshFilter.mesh = mesh;
    }
}

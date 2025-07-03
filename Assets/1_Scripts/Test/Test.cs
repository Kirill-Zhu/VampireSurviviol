using UnityEngine;
using Zenject;

public class Test : MonoBehaviour
{
    [Inject]
    public PlayerSettings playerSettings;
    private MeshFilter _meshFilter;
    public Mesh mesh;
    private void Awake() {
        _meshFilter = GetComponent<MeshFilter>();   
        mesh = playerSettings.Character.Mesh;
        _meshFilter.mesh = mesh;
    }
}

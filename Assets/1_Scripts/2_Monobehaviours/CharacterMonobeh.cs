using UnityEngine;
using Zenject;
using Chartacters;
public class CharacterMonobeh : MonoBehaviour
{
    [Inject]
    [SerializeField] private PlayerSettings _settings;
    [SerializeField] private Character _character;
    
    private MeshFilter _meshFilter;
    [SerializeField] private MeshRenderer _meshRenderer;

    private void Start() {
        _character = _settings.Character;

        _meshFilter = GetComponent<MeshFilter>();
        _meshFilter.mesh = _character.Mesh;

        _meshRenderer.material = _character.Material;
    }
   
}

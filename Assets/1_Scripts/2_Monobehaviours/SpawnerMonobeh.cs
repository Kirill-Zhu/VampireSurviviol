using Chartacters;
using UnityEngine;
using Zenject;

public class SpawnerMonobeh : MonoBehaviour
{
    public static SpawnerMonobeh instance;


    [Header("Bullet")]
    [Inject]
    [SerializeField] private PlayerSettings _playerSettings;
    private Weapon _weapon;
    public Mesh BulletMesh;
    public Material BulletMaterial;
    
    private void Awake() {
       
        if(instance != null ) {
            Destroy(this.gameObject);
        }
        else
            instance = this;

        _weapon = _playerSettings.Weapon;
        BulletMesh = _weapon.BulletMesh;
        BulletMaterial = _weapon.BulletMaterial;
    }


}

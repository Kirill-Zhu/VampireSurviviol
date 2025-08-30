using Unity.Entities;
using UnityEngine;

public class SkillsMonobehAnimations : MonoBehaviour
{
    [SerializeField] private ParticleSystem[] _ulitParitcle;
    [SerializeField] private ParticleSystem[] _dashParticle;
    [SerializeField] private ParticleSystem[] _antiGravitationParticle;
    [SerializeField] private ParticleSystem[] _meleParticle;
    [SerializeField] private ParticleSystem[] _shotParticle;
    [SerializeField] private ParticleSystem[] _laserGunParticle;
    public static SkillsMonobehAnimations Instance;
    
    private EntityManager _entityManager;
    private Entity targetEntity;
    private GunType _gunType;
    #region Singleton
    private void Awake() {
        if(Instance == null) 
            Instance = this;
        else Instance = this;
        #endregion
    }
    private void Update() {
        if (PlayerInputs.Instance.UltiAction.WasPressedThisFrame()&&PlayerUltiUI.Instance.CanUlti()) {
            foreach (var particle in _ulitParitcle) { 
            particle.Play();
            }
        }
        if (PlayerInputs.Instance.DashAction.WasPressedThisFrame()) {
            foreach (var particle in _dashParticle) {
                particle.Play();
            }
        }
        if (Input.GetKeyDown (KeyCode.Space)) {
            foreach (var particle in _antiGravitationParticle) {
                particle.Play();
            }
        }
        if (PlayerInputs.Instance.MeleAttackAction.WasPressedThisFrame()) {
            foreach (var particle in _meleParticle) {
                particle.Play();
            }
        }


    }

    public void Shoot(GunType type) {
        _entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

        FindTargetEntity();
        if (_entityManager.Exists(targetEntity) && _entityManager.HasComponent<ControlledGun>(targetEntity)) { 
            var gun = _entityManager.GetComponentData<ControlledGun>(targetEntity);
            _gunType = gun.gunType;
        }
        //Switch case gun type
        switch (_gunType) {
            case GunType.Pistol: {
                    foreach (var particle in _shotParticle) {
                        particle.Play();
                    }
                    break;
            }
            case GunType.Shootgun: {
                    foreach (var particle in _shotParticle) {
                        particle.Play();
                    }
                    break;
            }
            case GunType.Lasergun: {
                    foreach (var particle in _laserGunParticle) {
                        particle.Play();
                    }
                    break;  
            }
        }
       
    }
        void FindTargetEntity() {
            // Пример поиска entity по компоненту
            var query = _entityManager.CreateEntityQuery(typeof(ControlledGun));
            var entities = query.ToEntityArray(Unity.Collections.Allocator.Temp);

            if (entities.Length > 0) {
                targetEntity = entities[0];
            }
            entities.Dispose();
        }
}


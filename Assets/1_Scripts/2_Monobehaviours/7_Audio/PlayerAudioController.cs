using System.Collections.Generic;
using Tayx.Graphy.Advanced;
using UnityEngine;

public class PlayerAudioController : MonoBehaviour
{
    [SerializeField] private AudioSource _shootAudioSource;
    [SerializeField] private AudioSource _meleAudioSource;
    [SerializeField] private AudioSource _skillsAudioSource;
    [SerializeField] private List<AudioClip> _shootClips;
    [SerializeField] private AudioClip _meleAttackSlashClip;
    [SerializeField] private AudioClip _ultiClip;
    [SerializeField] private AudioClip _laserGunClip;

    private PlayerInputs _playerInputs;

    public static PlayerAudioController Instance;
    private GunType _gunType;
    #region Singleton
    private void Awake() {

        if (Instance == null)
            Instance = this;
        else
            Destroy(this.gameObject);
        #endregion
        _playerInputs = PlayerInputs.Instance;
    }

    private void Update() {
        if(_playerInputs == null)
            _playerInputs = PlayerInputs.Instance;

        if (_playerInputs.MeleAttackAction.WasPressedThisFrame()) {
            _meleAudioSource.clip = _meleAttackSlashClip;   
            _meleAudioSource.Play();
        }
        if (_playerInputs.UltiAction.WasPressedThisFrame()) {
            
            _skillsAudioSource.PlayOneShot(_ultiClip);
        }
        if (_shootAudioSource.clip == _laserGunClip &&!_playerInputs.ShootingAction.IsPressed()&&_gunType == GunType.Lasergun) {
            _shootAudioSource.Stop();
        }
    }

    public void Shoot(GunType gunType) {
        switch (gunType) {
            case GunType.Pistol: {
                    _gunType = gunType;
                    _shootAudioSource.Stop();
                    _shootAudioSource.loop = false;
                    _shootAudioSource.PlayOneShot(_shootClips[0]);
               break;
            }
            case GunType.Shootgun: {
                    _gunType = gunType;
                    _shootAudioSource.Stop();
                    _shootAudioSource.loop = false;
                    _shootAudioSource.PlayOneShot(_shootClips[0]);
                break;
            }
            case GunType.Lasergun: {
                    _gunType = gunType;
                    if (_shootAudioSource.clip == _laserGunClip && _shootAudioSource.isPlaying)
                        break;
                   _shootAudioSource.loop = true;
                   _shootAudioSource.clip = _laserGunClip;
                   _shootAudioSource.Play();
               break;
            }

        }
    }
}

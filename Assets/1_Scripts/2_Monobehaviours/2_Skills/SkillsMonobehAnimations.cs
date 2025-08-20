using TMPro;
using UnityEngine;

public class SkillsMonobehAnimations : MonoBehaviour
{
    [SerializeField] private ParticleSystem[] _ulitParitcle;
    [SerializeField] private ParticleSystem[] _dashParticle;
    [SerializeField] private ParticleSystem[] _antiGravitationParticle;
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
    }
}

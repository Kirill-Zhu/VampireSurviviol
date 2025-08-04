using UnityEngine;

public class SkillsMonobehAnimations : MonoBehaviour
{
    [SerializeField] private ParticleSystem[] _ulitParitcle;
    [SerializeField] private ParticleSystem[] _dashParticle;
    private void Update() {
        if (PlayerInputs.Instance.UltiAction.WasPressedThisFrame()) {
            foreach (var particle in _ulitParitcle) { 
            particle.Play();
            }
        }
        if (PlayerInputs.Instance.DashAction.WasPressedThisFrame()) {
            foreach (var particle in _dashParticle) {
                particle.Play();
            }
        }
    }
}

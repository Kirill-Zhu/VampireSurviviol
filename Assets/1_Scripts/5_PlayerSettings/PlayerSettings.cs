using UnityEngine;
using Zenject;
using Chartacters;
using System.Diagnostics.Contracts;

[CreateAssetMenu(fileName = "Player Settings", menuName = "Custom/PlayerSettings")]
public class PlayerSettings: ScriptableObject {
   
    public Character Character;
    public Weapon Weapon;
}
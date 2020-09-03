using UnityEngine;

namespace RPG.Combat{

    [CreateAssetMenu(fileName = "Weapon", menuName = "create new weapon", order = 0)]
    public class Weapon : ScriptableObject
    {
        [SerializeField] GameObject weaponPrefab = null;
        [SerializeField] AnimatorOverrideController weaponOverride = null;

       public void Spawn(Transform handTransform, Animator animator) {
           Instantiate(weaponPrefab, handTransform);
           animator.runtimeAnimatorController = weaponOverride;
       }
    }
}
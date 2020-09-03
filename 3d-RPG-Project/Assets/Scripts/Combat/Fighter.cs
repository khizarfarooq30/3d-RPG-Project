using UnityEngine;
using RPG.Movement;
using RPG.Core;
using System;

namespace RPG.Combat{
public class Fighter : MonoBehaviour, IAction
    {
        Health target;
        [SerializeField] float weaponRange = 2f;
        [SerializeField] float timeBetweenAttack = 1f;
        [SerializeField] float damageAmout = 5f;
        [SerializeField] Transform handTransform = null;
        [SerializeField] Weapon weapon = null;

        float timeSinceLastAttack = Mathf.Infinity;
        

        Animator animator;

        private void Start() {
            animator = GetComponent<Animator>();
            SpawnWeapon();
        }

        private void SpawnWeapon()
        {
            if(weapon == null) { return; }

            Animator animator = GetComponent<Animator>();
            weapon.Spawn(handTransform, animator);
            // if(weaponOverride == null) return;

          
            // animator.runtimeAnimatorController = weaponOverride;
        }

        private void Update()
        {
            timeSinceLastAttack += Time.deltaTime;

            if(target == null) { return; }

            if(target.IsDead()) { return; }
            
            if (!GetIsInRange())
            {
                GetComponent<Mover>().MoveTo(target.transform.position, 1f);
            }
            else
            {
                GetComponent<Mover>().Cancel();
                AttackBehaviour();
            }

        }

        private void AttackBehaviour()
        {
            transform.LookAt(target.transform);

            if (timeSinceLastAttack > timeBetweenAttack)
            {
                TriggerAttack();
                timeSinceLastAttack = 0;

            }
        }

        private void TriggerAttack()
        {
            animator.ResetTrigger("stopAttack");
            animator.SetTrigger("attack");
        }

        //Animation Event
        void Hit()
        {   if(target == null) { return; }
            target.TakeDamage(damageAmout);
        }

        private bool GetIsInRange()
        {
            return Vector3.Distance(transform.position, target.transform.position) < weaponRange;
        }

        public bool CanAttack(GameObject combatTarget)
        {
            if(combatTarget == null) { return false; }
            Health targetToTest = combatTarget.GetComponent<Health>();
            return targetToTest != null && !targetToTest.IsDead();
        }



        public void Attack(GameObject combatTarget){
            GetComponent<ActionScheduler>().StartAction(this);
            target = combatTarget.GetComponent<Health>();
        }

        public void Cancel()
        {
            StopAttack();
            target = null;
            GetComponent<Mover>().Cancel();
        }

        private void StopAttack()
        {
            animator.ResetTrigger("attack");
            animator.SetTrigger("stopAttack");
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Combat;
using RPG.Core;
using RPG.Movement;
using System;

namespace RPG.Control{

    public class AIController : MonoBehaviour
    {   
      
        [SerializeField] float chaseDistance = 5f;
        [SerializeField] float suspicionTime = 3f;
        [SerializeField] float dewellTime = 3f;
        [SerializeField] PatrolPath patrolPath;
        [SerializeField] float wayPointTolerance = 1f;
        [Range(0,1)]
        [SerializeField] float patrolSpeedFraction = 0.2f;
        

        GameObject player;
        Fighter fighter;
        Health health;
        Mover mover;

        Vector3 guardLocation;

        float timeSinceLastSawPlayer = Mathf.Infinity;
        float timeSinceReachedWaypoint = Mathf.Infinity;

        int currentWaypointIndex = 0;

        

        private void Start() {
            player = GameObject.FindWithTag("Player");
            fighter = GetComponent<Fighter>();
            health = GetComponent<Health>();
            mover = GetComponent<Mover>();

            guardLocation = transform.position;
        }

        // Update is called once per frame
        void Update()
        {
            if(health.IsDead()) { return; }

            if (InAttackRange() && fighter.CanAttack(player))
            {
                timeSinceLastSawPlayer = 0;
                AttackBehaviour();
            }

            else if(timeSinceLastSawPlayer < suspicionTime)
            {
                SuspicionBehaviour();
            }

            else
            {
            
                PatrolBehaviour();
            }

            timeSinceLastSawPlayer += Time.deltaTime;
            timeSinceReachedWaypoint += Time.deltaTime;
        }

        private void AttackBehaviour()
        {
            fighter.Attack(player);
        }

        private void SuspicionBehaviour()
        {
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }


        private void PatrolBehaviour()
        {
            Vector3 nextPosition = guardLocation;
           
            if(patrolPath != null)
            {
            
                if(AtWayPoint()) 
                {   
                    timeSinceReachedWaypoint = 0;
                    CycleWayPoint();
                }
                
                nextPosition = GetCurrentWaypoint();
            }
            if (timeSinceReachedWaypoint > dewellTime)
            {
                mover.StartMoveAction(nextPosition, patrolSpeedFraction);
            }
        }

        private bool AtWayPoint()
        {
            float distanceToWayPoint = Vector3.Distance(transform.position, GetCurrentWaypoint());
            return distanceToWayPoint < wayPointTolerance;
        }

        private void CycleWayPoint()
        {
            currentWaypointIndex = patrolPath.GetNextIndex(currentWaypointIndex);
        }

        private Vector3 GetCurrentWaypoint()
        {
           return patrolPath.GetWaypoint(currentWaypointIndex);
        }

        private bool InAttackRange()
        {
            float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
            return distanceToPlayer < chaseDistance;
        }
        
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, chaseDistance);
        }
    }
}
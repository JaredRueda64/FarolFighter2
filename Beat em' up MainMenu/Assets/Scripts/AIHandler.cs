using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;


namespace SA
{
    public class AIHandler : MonoBehaviour
    {
        public UnitController unitController;

        public UnitController enemy;

        public float minDeadTime;
        public float maxDeadTime;
        float getDeadTimeRate
        {
            get
            {
                float v =Random.Range(minDeadTime, maxDeadTime);
                return v;
            }
        }
        float deadTime;


      float attackTime = 1;
      public float attackRate = 1.5f;
      public float attackDistance = 2;
      public float rotateDistance = 2;
      public float verticalThreshold = .1f;
      public float rotationThreshold = .5f;
      public float forceStopDistance = .3f;
      public bool forceStop;
      float forceStopCooldown = 0f;
      public float forceStopCooldownTime = 0.5f;
      
      
      public bool isInteracting { 
          get
          {
      
              return unitController.isInteracting;
          }
      }

      private void Start()
      {
          unitController.isAI = true;
      }
      

      private void Update()
      {
         if (enemy == null)
             return;
      
         float delta = Time.deltaTime;
         Vector3 myPosition = transform.position;
         Vector3 enemyPosition = enemy.position;
      
         if (isInteracting || unitController.isDead)
         {
             unitController.UseRootMotion();
             return;
         }
      
         
      
      
         if(deadTime > 0)
         {
             deadTime -= delta;
             return;
         }
      
         Vector3 directionToTarget = enemyPosition - myPosition;
         directionToTarget.Normalize();
         directionToTarget.z = 0;
         Vector3 targetPosition = enemyPosition + (directionToTarget * -1) * attackDistance;
        
      
         bool isCloseToTargetPosition = IsCloseToTargetPosition(myPosition, targetPosition);
         bool CloseToEnemy_NoVertical = isCloseToEnemy_NoVertical(myPosition, enemyPosition);
         bool CloseToEnemy_General = isCloseToEnemy_General(myPosition, enemyPosition);
      
         Collider[] colliders = Physics.OverlapSphere(transform.position, forceStopDistance);
         bool someoneBlocking = false;
      
         foreach (var item in colliders)
         {
             AIHandler a =item.transform.GetComponentInParent<AIHandler>();
             if(a != null && a != this && !a.forceStop)
             {
                 someoneBlocking = true;
                 break;
             }
         }
      
         if (someoneBlocking)
         {
             forceStop = true;
             forceStopCooldown = forceStopCooldownTime;
         }
         else
         {
             if (forceStopCooldown > 0)
             {
                 forceStopCooldown -= delta;
                 forceStop = true;
             }
             else
             {
                 forceStop = false;
             }
         }

            //   if (!forceStop && !CloseToEnemy_NoVertical
            //       && !isCloseToTargetPosition)
            //   {
            //          targetPosition
            //
            //
            //       //NavMeshHit navHit;
            //       //if (NavMesh.Raycast(transform.position,
            //       //    unitController.agent.desiredVelocity, out navHit, NavMesh.AllAreas))
            //       //{
            //       //    unitController.agent.isStopped = true;
            //       //}
            //
            //        if (!CloseToEnemy_General)
            //        {
            //            unitController.HandleRotation(unitController.agent.velocity.x < 0);
            //        }
            //        else
            //        {
            //            unitController.HandleRotation(directionToTarget.x < 0);
            //        
            //        }
            //
            //    }
            //    else
            //    {
            //          
            //       unitController.agent.isStopped = true;
            //       unitController.HandleRotation(directionToTarget.x < 0);
            //      
            //       
            //      
            //      
            //       if (attackTime > 0)
            //       {
            //           if (!forceStop)
            //               attackTime -= delta;
            //       }
            //       else
            //       {
            //         //unitController.PlayAction(unitController.defaultActions[0]);
            //           attackTime = attackRate;
            //           deadTime = getDeadTimeRate;
            //       }
            //      }

            Vector3 targetDirection = targetPosition - transform.position;
            targetDirection.Normalize();
           unitController.TickPlayer(delta, targetDirection);
       }
      
       public bool IsCloseToTargetPosition(Vector3 p1, Vector3 p2)
       {
           float distance = Vector3.Distance(p1, p2);
           return distance < attackDistance;
       }
      
       public bool isCloseToEnemy_NoVertical(Vector3 p1, Vector3 p2)
       {
           float dif = p1.z - p2.z;
           if(Mathf.Abs(dif) < verticalThreshold)
           {
               return Vector3.Distance(p1,p2) < attackDistance;
           }
           else
           {
               return false;
           }
       }
       public bool isCloseToEnemy_General(Vector3 p1, Vector3 p2)
       {
           return Vector3.Distance(p1, p2) < rotateDistance;
      
       }



    }
}


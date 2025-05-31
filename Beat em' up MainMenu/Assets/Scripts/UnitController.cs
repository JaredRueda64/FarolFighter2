using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.AI;

namespace SA
{
    public class UnitController : MonoBehaviour
    {
        public int health = 100;
        public int team;
        AnimatorHook animatorHook;
        public Transform holder;

        public float horizontalSpeed = .8f;
        public float verticalSpeed = .65f;
        public bool isLookingLeft;
        public bool isAI;
        public bool hasBackHit;
        public bool isDead;
        public LayerMask walkLayer;

        public Vector3 position {
            get {
                return transform.position;
            }
        }

        public bool canDoCombo
        {
            get
            {
                return animatorHook.canEnableCombo;
            }
        }

        public int actionsIndex;
        public ActionDataHolder actionDataHolder;

        ActionData[] currentActionData
        {
            get
            {
                return actionDataHolder.GetActions(actionsIndex);
            }
        }


        public bool isInteracting
        {
            get
            {
                return animatorHook.isInteracting;
            }
        }

        private void Start()
        {
            
            animatorHook = GetComponentInChildren<AnimatorHook>();

            
        }
        public void TickPlayer(float delta, Vector3 direction)
        {
            direction.x *= horizontalSpeed * delta;
            direction.y *= verticalSpeed * delta;
            bool isMoving = direction.sqrMagnitude > 0;

            
            animatorHook.Tick(isMoving);

            Vector3 targetPosition = transform.position + direction;
            MoveOnPosition(targetPosition);

        }

        public void UseRootMotion()
        {
            //agent.velocity = animatorHook.deltaPosition;
            Vector3 targetPosition = transform.position + animatorHook.deltaPosition;
        }

        void MoveOnPosition(Vector3 targetPostion)
        {
            

            
            Collider2D[] colliders = Physics2D.OverlapPointAll(targetPostion,walkLayer);
            bool isValid = false;
            
            foreach (var item in colliders)
            {
                if (!isAI)
                {
                    TBlockMovement block = item.GetComponent<TBlockMovement>();
                    if (block != null)
                    {
                        isValid = false;
                        break;
                    }
                }

                TWalkable w = item.GetComponent<TWalkable>();
                if (w != null)
                {
                    if (isAI) 
                    {
                        isValid = true; 
                    }
                    else
                    {
                        if (w.isPlayer)
                        {
                            isValid = true;
                        }
                    }
                }
            }

            if (isValid)
            {
                transform.position = targetPostion;
            }
        }

        public void HandleRotation(bool looksLeft)
        {
            Vector3 eulers = Vector3.zero;
            isLookingLeft = false;
            if (looksLeft)
            {
                eulers.y = 180;
                isLookingLeft = true;
            }

            holder.localEulerAngles = eulers;
        }

        ActionData storedAction;
        public ActionData getLastAction
        {
            get
            {
                return storedAction;
            }
        }

        public void DetectAction(InputHandler.InputFrame f)
        {
               if (f.attack == false && f.jump == false)
                   return;

            foreach (var a in currentActionData)
            {
                if (a.isDeterministic)
                {
                    if (a.inputs.attack == f.attack && a.inputs.down == f.down && a.inputs.left == f.left
                        && a.inputs.right == f.right && a.inputs.up == f.up && a.inputs.jump == f.jump)
                    {
                        PlayAction(a);
                        break;
                    }
                }
                else
                {
                    if (a.inputs.attack == f.attack || a.inputs.jump == f.jump)
                    {
                        PlayAction(a);
                        break;
                    }
                }
            }
        }

        public void PlayAction(ActionData actionData)
        {
            PlayAnimation(actionData.actionAnim);
            storedAction = actionData;
        }

        public void PlayAnimation(string animName)
        {
            animatorHook.PlayAnimation(animName);
        }

        public void SetIsDead()
        {
            animatorHook.SetIsDead();
            isDead = true;
        }

        public void OnHit(ActionData actionData, bool hitterLooksLeft)
        {
            bool isFromBehind = false;
      
            if(isLookingLeft && hitterLooksLeft 
                || !hitterLooksLeft && !isLookingLeft)
            {
                isFromBehind = true;
            }

            DamageType damageType = actionData.damageType;
            health -= actionData.damage;
            if(health <= 0)
            {
                damageType = DamageType.heavy;
                SetIsDead();
            }

            if (!hasBackHit) 
            {

                if (isFromBehind) 
                {
                    HandleRotation(!hitterLooksLeft);
                }
                isFromBehind = false;
            }

            switch (damageType)
            {
                case DamageType.light:
                    if (isFromBehind)
                    {
                        PlayAnimation("hurt_light_back");
                    }
                    else
                    {
                        PlayAnimation("hurt_light_front");
                    }

                    break;
                case DamageType.mid:
                    if (isFromBehind)
                    {
                        PlayAnimation("hurt_light_back");
                    }
                    else
                    {
                        PlayAnimation("hurt_light_front");
                    }
                    break;
                case DamageType.heavy:
                    if (isFromBehind)
                    {
                        PlayAnimation("knockdown_back");
                    }
                    else
                    {
                        PlayAnimation("knockdown_front");
                    }
                    break;
                default:
                    break;
            }
        }

        public void isCombo()
        {
            animatorHook.SetIsCombo();
        }
        
        public void LoadActionData(int index)
        {
            actionsIndex = index;
        }

        public void ResetActionData()
        {
            actionsIndex = 0;
        }

    }
}

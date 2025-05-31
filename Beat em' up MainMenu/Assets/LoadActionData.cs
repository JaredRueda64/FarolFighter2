using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class LoadActionData : StateMachineBehaviour
    {
        public int actionIndex;
        UnitController owner;
       override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (owner == null)
            {
                owner = animator.GetComponentInParent<UnitController>();
            }

            owner.LoadActionData(actionIndex);
        }
    }
}


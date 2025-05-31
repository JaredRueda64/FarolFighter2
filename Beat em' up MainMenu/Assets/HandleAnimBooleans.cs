using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SA.HandleAnimBooleans;

namespace SA
{
    public class HandleAnimBooleans : StateMachineBehaviour
    {
        public BoolHolder[] boolHolders;
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            for (int i = 0; i < boolHolders.Length; i++)
            {
                animator.SetBool(boolHolders[i].boolName, boolHolders[i].status);
            }
        }

    
        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            for (int i = 0; i < boolHolders.Length; i++)
            {
                if (boolHolders[i].resetOnExit)
                {
                    animator.SetBool(boolHolders[i].boolName, !boolHolders[i].status);
                }
            }
        }

        [System.Serializable]
        public class BoolHolder
        {
            public string boolName;
            public bool status;
            public bool resetOnExit;
        }


    }
}

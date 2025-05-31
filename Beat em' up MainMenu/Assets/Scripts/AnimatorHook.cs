using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA { 
    public class AnimatorHook : MonoBehaviour
    {
        Animator anim;

        public Vector3 deltaPosition;
        UnitController owner;

        public bool canEnableCombo {
            get{
                return anim.GetBool("canEnableCombo");
            }
        }

        public bool isInteracting
        {
            get
            {
                return anim.GetBool("isInteracting");
            }
        }

        private void Start()
        {
            anim = GetComponent<Animator>();
            owner = GetComponentInParent<UnitController>();
        }

        public void Tick(bool isMoving)
        {
            float v = (isMoving) ? 1 : 0;
            anim.SetFloat("move", v);
        }
        public void PlayAnimation(string animName)
        {
            anim.Play(animName);
            anim.SetBool("isInteracting", true);
        }

        private void OnAnimatorMove()
        {
            deltaPosition = anim.deltaPosition / Time.deltaTime;
        }

        public void SetIsDead()
        {
            anim.SetBool("isDead", true);
        }

        public void SetIsCombo()
        {
            anim.SetBool("isCombo", true);

        }

        public void CloseAgent()
        {
           // owner.agent.enabled = false;
        }

        public void OpenAgent()
        {
           // owner.agent.enabled = true;
        }

        public void SetIsInteracting(int status)
        {
            anim.SetBool("isInteracting", status == 1);
        }

        public void LoadActionData(int actionIndex)
        {
            owner.LoadActionData(actionIndex);
        }


    }
}

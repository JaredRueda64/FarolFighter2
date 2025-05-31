using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA { 
    public class TWalkable : MonoBehaviour
    {

        public bool isPlayer;
        private void Start()
        {
            gameObject.layer = 7;
        }


    }
}
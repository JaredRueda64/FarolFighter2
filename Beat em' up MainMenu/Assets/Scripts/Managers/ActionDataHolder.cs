using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA {
    [CreateAssetMenu]
    public class ActionDataHolder : ScriptableObject
    {
        public ActionsContainer[] actions;

        public ActionData[] GetActions(int index)
        {
            return actions[index].actions;
        }
    }

    [System.Serializable]
    public class ActionsContainer
    {
        public string actionsId;
        public ActionData[] actions;
    }


}

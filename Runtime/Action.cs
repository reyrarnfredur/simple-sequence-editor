using System.Collections;
using UnityEngine;
using System;

namespace Theogramme.SequenceScript 
{

    [System.Serializable]
    public abstract class Action {

        [HideInInspector] public string visibleName;
        [HideInInspector] public string visibleParameters;

        [HideInInspector] public bool enabled = true;

        public virtual void OnAdd() {
            OnValidate();
        }
        
        public virtual void OnRemove() {}
        public virtual void OnValidate() {}

        public abstract IEnumerator GetCoroutine( ObjectContainer objectContainer );
        
        public void SetDisplayContent(string name, params string[] parameters) {

            this.visibleName = name + ": ";
            this.visibleParameters = string.Join( ", ", parameters );

        }

    }

}

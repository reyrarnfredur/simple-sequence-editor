using System.Collections;
using NaughtyAttributes;
using UnityEngine;

using Theogramme.SequenceScript;

public class Wait : Action {

    public float seconds;
    public bool realtime;

    public override void OnValidate() {

        SetDisplayContent( "Wait", seconds.ToString() + " seconds");

    }

    public override IEnumerator GetCoroutine( ObjectContainer objectContainer ) {
        
        if(realtime) {

            yield return new WaitForSecondsRealtime( seconds );

        } else {

            yield return new WaitForSeconds( seconds );

        }

    }

}

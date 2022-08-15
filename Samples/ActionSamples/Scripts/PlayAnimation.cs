using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Theogramme.SequenceScript;
using Theogramme.ObjectContainerSystem;

public class PlayAnimation : Action
{

    [Key] public string animationKey = "AnimationKey";
    [Key] public string animationName = "AnimationName";

    public ExposedReference<Transform> transform;

    public ComponentSearchMode searchMode = ComponentSearchMode.Object;

    public float startDelay = 0;
    public bool queued = false;
    public bool waitUntilFinished = false;
    
    public override void OnValidate() {

        SetDisplayContent( "Play Animation", animationKey, animationName );

    }

    public override IEnumerator GetCoroutine( ObjectContainer objectContainer ) {

        Animation _animation = objectContainer.GetComponent<Animation>(animationKey );

        if(startDelay > 0) yield return new WaitForSeconds( startDelay );

        if(queued) _animation.PlayQueued( animationName );
        else _animation.Play(animationName);

        if(waitUntilFinished) yield return new WaitForSeconds(_animation.GetClip(animationName).length);

    }

}

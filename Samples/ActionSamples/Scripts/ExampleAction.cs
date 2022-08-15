using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Theogramme.SequenceScript;
using Theogramme.ObjectContainerSystem;

public class ExampleAction : Action
{

    [Key] public string objectKey;

    public float time = 1;

    // Is called right after adding the action to the sequence
    public override void OnAdd() {

        base.OnAdd();


    }

    // Is called when the action is removed from the list
    public override void OnRemove() {



    }

    // Is called with Sequence OnValidate()
    public override void OnValidate() {

        // Set how and what is displayed as the action name, 
        // format is: "ActionName: Argument1, Argument2, Argument3"
        SetDisplayContent( "Example Action", objectKey, time.ToString() );

    }

    // Returns the coroutine to be run as part of the sequence
    public override IEnumerator GetCoroutine( ObjectContainer objectContainer ) {

        // To get a component in a GameObject, use GetComponent<Component>(string key)
        Animator _animator = objectContainer.GetComponent<Animator>( objectKey );

        // You can also get a component the normal way
        //_animator = objectContainer.GetObject<GameObject>( objectKey ).GetComponent<Animator>();

        // Example
        _animator.SetTrigger( "trigger" );

        yield return new WaitForSeconds(time);

    }

}

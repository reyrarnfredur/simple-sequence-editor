using System.Collections;
using UnityEngine;

using Theogramme.SequenceScript;
using Theogramme.ObjectContainerSystem;

[System.Serializable]
public class ChangePosition : Theogramme.SequenceScript.Action {

    [Key] public string objectKey = "Object Key";

    public ValueMode mode = ValueMode.Set;
    public Space space = Space.World;

    public Vector3 position = Vector3.zero;

    public override void OnValidate() {

        SetDisplayContent( mode.ToString() + " Position", objectKey, space.ToString() );

    }

    public override IEnumerator GetCoroutine( ObjectContainer objectContainer ) {

        // Retrieve object with objectKey key as Transform
        Transform _transform = objectContainer.GetComponent<Transform>( objectKey );

        if(_transform != null) {

            if(space == Space.World) {

                if(mode == ValueMode.Set) _transform.position = position;
                else if(mode == ValueMode.Add) _transform.position += position;
                else if(mode == ValueMode.Subtract) _transform.position -= position;

            } else {

                if (mode == ValueMode.Set) _transform.position = position;
                else if (mode == ValueMode.Add) _transform.position += position;
                else if (mode == ValueMode.Subtract) _transform.position -= position;
                
            }

        }

        yield return null;

    }

}

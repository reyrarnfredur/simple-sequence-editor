using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Theogramme.SequenceScript;
using Theogramme.ObjectContainerSystem;

public class ChangeRotation : Action
{

    [Key] public string objectKey = "Object Key";

    public ValueMode mode = ValueMode.Set;
    public Space space = Space.World;

    public Vector3Int rotation = Vector3Int.zero;

    public bool wrapAround360 = true;

    private Transform transform;


    public override void OnValidate() {

        SetDisplayContent( mode.ToString() + " Rotation", objectKey, space.ToString() );

        if(wrapAround360) {

            rotation.x %= 360;
            rotation.y %= 360;
            rotation.z %= 360;

            rotation.x = rotation.x < 0 ? 360 : rotation.x;
            rotation.y = rotation.y < 0 ? 360 : rotation.y;
            rotation.z = rotation.z < 0 ? 360 : rotation.z;

        }

    }

    public override IEnumerator GetCoroutine( ObjectContainer objectContainer ) {

        Transform _transform = objectContainer.GetComponent<Transform>( objectKey );

        if (_transform != null) {

            if (space == Space.World) {

                if (mode == ValueMode.Set) _transform.eulerAngles = rotation;
                else if (mode == ValueMode.Add) _transform.eulerAngles += rotation;
                else if (mode == ValueMode.Subtract) _transform.eulerAngles -= rotation;

            } else {

                if (mode == ValueMode.Set) _transform.localEulerAngles = rotation;
                else if (mode == ValueMode.Add) _transform.localEulerAngles += rotation;
                else if (mode == ValueMode.Subtract) _transform.localEulerAngles -= rotation;

            }

        }

        yield return null;

    }

}

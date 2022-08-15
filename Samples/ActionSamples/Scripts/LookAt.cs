using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Theogramme.SequenceScript;
using Theogramme.ObjectContainerSystem;

public class LookAt : Action {

    [Key] public string objectKey = "ObjectName";
    [Key] public string targetKey = "TargetName";

    public float duration = 0;

    public Vector3 upVector = Vector3.up;

    public bool useXAxis = true;
    public bool useYAxis = true;
    public bool useZAxis = true;

    public override void OnValidate() {

        SetDisplayContent( "Look At", objectKey, targetKey, duration + " seconds");

        duration = Mathf.Max( 0, duration );

    }

    public override IEnumerator GetCoroutine( ObjectContainer objectContainer ) {

        Transform _transform = objectContainer.GetComponent<Transform>( objectKey );
        Transform _target = objectContainer.GetComponent<Transform>( targetKey );

        Debug.Log( _transform );

        Vector3 _initialEulers = _transform.eulerAngles;

        float _timer = 0;

        while(_timer < duration) {

            _timer += Time.deltaTime;

            Vector3 _direction = (_target.position - _transform.position).normalized;
            Quaternion _rotation = Quaternion.LookRotation( _direction, upVector );

            Vector3 _eulers = _rotation.eulerAngles;

            _transform.eulerAngles = new Vector3(

                useXAxis ? Mathf.LerpAngle( _initialEulers.x, _eulers.x, _timer / duration) : _transform.eulerAngles.x,
                useYAxis ? Mathf.LerpAngle( _initialEulers.y, _eulers.y, _timer / duration) : _transform.eulerAngles.y,
                useZAxis ? Mathf.LerpAngle( _initialEulers.z, _eulers.z, _timer / duration) : _transform.eulerAngles.z

            );

            yield return null;

        }

    }

}

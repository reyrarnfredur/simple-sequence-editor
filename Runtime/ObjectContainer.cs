using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ObjectContainer : MonoBehaviour
{

    [System.Serializable]
    public struct ObjectReference {

        public string key;
        public Object value;

        [HideInInspector]
        public int keyIndex;

        public ObjectReference(string key, Object value) {

            this.key = key;
            this.value = value;

            this.keyIndex = 0;

        }

    }

    [SerializeField]
    private List<ObjectReference> objects = new List<ObjectReference>();

    public T GetObject<T>(string key) where T : Object {

        int _index = objects.FindIndex( x => x.key == key );

        if(_index != -1) {

            T _value = objects[_index].value as T;

            Debug.Log( _value );

            return _value;

        }

        Debug.LogWarning( "Object with key: " + key + " has not been found in container.", gameObject );

        return null;

    }

    public T[] GetObjects<T>(string key) where T : Object {

        return objects.Where( x => x.key == key ).Select(x => x.value).Cast<T>().ToArray();

    }

    public T GetComponent<T>( string key ) where T : Component {

        GameObject _gameObject = GetObject<GameObject>(key);

        if(_gameObject != null) {

            T _component = _gameObject.GetComponent<T>();

            if(_component != null) return _gameObject.GetComponent<T>() as T;

            Debug.LogWarning( "Couldn't get component: " + typeof( T ).Name + " from " + _gameObject.name, gameObject );

        }  

        return null;

    }

    public void AddObject(string key, Object value) {

        objects.Add( new ObjectReference( key, value ) );

    }

    public void RemoveObject(string key) {

        if(!objects.Any(x => x.key == key)) {

            objects.RemoveAt(objects.FindIndex( x => x.key == key ));

        }

    }

}

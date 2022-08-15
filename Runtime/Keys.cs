using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

[FilePath("ProjectSettings/ExposedKeys.asset", FilePathAttribute.Location.ProjectFolder)]
public class Keys : ScriptableSingleton<Keys>, ISerializationCallbackReceiver
{

    public HashSet<string> hashSet = new HashSet<string>();

    [SerializeField]
    private string[] _serializedArray;

    public void Add(string key) {

        hashSet.Add( key );

    }

    public void Remove(string key) {

        hashSet.Remove( key );

    }

    public bool IsDefined(string key) {

        return hashSet.Contains( key );

    }

    public new string ToString() {

        string _str = "[ " + string.Join( ", ", hashSet ) + " ]";
        return _str;

    }

    public string GetPath() {

        return Keys.GetFilePath();

    }

    public void SaveChanges() {

        Debug.LogWarning( "Key list modified. Saving to: \"" + GetPath() + "\"");

        Save(true);

    }

    public void OnBeforeSerialize() {

        _serializedArray = hashSet.ToArray();

    }

    public void OnAfterDeserialize() {

        hashSet = new HashSet<string>( _serializedArray );

    }
}

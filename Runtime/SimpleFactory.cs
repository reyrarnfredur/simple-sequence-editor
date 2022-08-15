using System.Collections.Generic;
using System.Linq;
using System;

[System.Serializable]
public class SimpleFactory<T> where T : class
{

    private Dictionary<string, Type> _types = new Dictionary<string, Type>();

    public SimpleFactory(Type[] types) {

        foreach (Type t in types) {

            if(!t.IsAbstract && t.BaseType == typeof(T)) {

                _types.Add(t.Name, t);

            } else {

                throw(new Exception("Type " + t.Name + " is not valid because it is abstract or it does not inherit from " + typeof(T).Name));

            }

        }

    }

    public T CreateInstance(string name, params object[] args) {

        Type _t = _types[name];
        object _instance = Activator.CreateInstance(_t, args);

        return (T)_instance;

    }

    public T CreateInstance(Type type, params object[] args ) {

        Type _t = _types[type.Name];
        object _instance = Activator.CreateInstance(_t, args);

        return (T)_instance;

    }

    public string[] GetNames() {

        return _types.Select(x => x.Key).ToArray();

    }

    public Type[] GetTypes() {

        return _types.Values.ToArray();
        
    }
    
}

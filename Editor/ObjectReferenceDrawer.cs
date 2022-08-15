using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Theogramme.ObjectContainerSystem
{
    
    [CustomPropertyDrawer( typeof( ObjectContainer.ObjectReference ) )]
    public class ObjectReferenceDrawer : PropertyDrawer
    {

        public override void OnGUI( Rect position, SerializedProperty property, GUIContent label ) {

            EditorGUI.BeginProperty( position, label, property );

            var _key = property.FindPropertyRelative( "key" );
            var _value = property.FindPropertyRelative( "value" );
            var _index = property.FindPropertyRelative( "keyIndex" );

            position.height = GetPropertyHeight( property, label ) - 2;
            position.y ++;

            Rect _keyRect = new Rect( position.x, position.y, position.width / 3 - 2, position.height );
            Rect _valueRect = new Rect( position.x + _keyRect.width + 2, position.y, (position.width / 3) * 2, position.height );

            string[] _validKeys = null;

            if(Keys.instance.hashSet.Count > 0) {

                if(_index.intValue > Keys.instance.hashSet.Count - 1 || !Keys.instance.IsDefined( _key.stringValue )) _index.intValue = 0;

                _validKeys = Keys.instance.hashSet.ToArray();

                _index.intValue = EditorGUI.Popup( _keyRect, _index.intValue, _validKeys );
                _key.stringValue = _validKeys[_index.intValue];

            } else {

                //_index.intValue = 0;
                EditorGUI.LabelField( _keyRect, "None" );

            }

            _value.objectReferenceValue = EditorGUI.ObjectField( _valueRect, _value.objectReferenceValue, typeof(Object), true );

            EditorGUI.EndProperty();

        }

        public override float GetPropertyHeight( SerializedProperty property, GUIContent label ) {

            float _height = base.GetPropertyHeight( property, label );

            return _height + 2;

        }

    }

}


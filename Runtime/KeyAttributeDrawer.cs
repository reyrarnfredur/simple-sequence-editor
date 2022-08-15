using UnityEditor;
using UnityEngine;

namespace Theogramme.ObjectContainerSystem
{
    
    [CustomPropertyDrawer( typeof( KeyAttribute ) )]
    public class KeyAttributeDrawer : PropertyDrawer {

        //[SerializeField] bool _exposed;
        //[SerializeField] string _key;
        //[SerializeField] string _oldKey;

        public override void OnGUI( Rect position, SerializedProperty property, GUIContent label ) {

            string _key = property.stringValue;
            bool _exposed = Keys.instance.IsDefined( _key );

            EditorGUI.BeginProperty( position, label, property );

            // Calculate rects
            Rect _labelRect = new Rect( position.x, position.y, position.width / 2, position.height);
            Rect _textFieldRect = new Rect( position.x + _labelRect.width, position.y, position.width / 2, position.height );

            // Change color if exposed
            if(_exposed) GUI.color += Color.green;
            else GUI.color += Color.red;

            // Draw label
            EditorGUI.LabelField( _labelRect, label);

            // Start monitoring changes
            EditorGUI.BeginChangeCheck();

            // Set property value with text field
            property.stringValue = _key = EditorGUI.TextField( _textFieldRect, property.stringValue);

            // Reset color
            GUI.color = Color.white;

            // Stop monitoring changes
            if (EditorGUI.EndChangeCheck()) {

                // If property changed and is exposed
                if (_exposed) {

                    // Add as a new key
                    Keys.instance.Add( _key );

                    Keys.instance.SaveChanges();
                    
                }
                
            }

            if (Event.current.type == EventType.ContextClick) {

                Vector2 mousePos = Event.current.mousePosition;

                if (position.Contains( mousePos )) {

                    GenericMenu menu = new GenericMenu();

                    string _localKey = _key;

                    if(_exposed) {

                        menu.AddItem( new GUIContent( "Hide" ), false, () => { Keys.instance.Remove( _localKey ); Keys.instance.SaveChanges(); } );
                        menu.AddDisabledItem( new GUIContent( "Expose" ));

                    } else {

                        menu.AddDisabledItem( new GUIContent( "Hide" ));
                        menu.AddItem( new GUIContent( "Expose" ), false, () => { Keys.instance.Add( _localKey ); Keys.instance.SaveChanges(); } );

                    }

                    menu.DropDown( new Rect( mousePos.x, mousePos.y, 0, 0 ) );

                    Event.current.Use();

                }

            }

            EditorGUI.EndProperty();

        }

    }

}


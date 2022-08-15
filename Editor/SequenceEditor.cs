using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System.Reflection;
using System.Linq;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace Theogramme.SequenceScript {

    [CustomEditor( typeof( Sequence ) )]
    public class SequenceEditor : Editor {
        

        private ReorderableList _reorderableList;
        private SerializedProperty _currentList;
        
        private Sequence _sequence => serializedObject.targetObject as Sequence;

        private int _selectedTypeIndex;
        private int _selectedPageIndex = 0;

        private int _lastSelectedPageIndex = -1;
        private int _numberOfPages;

        private bool _displaySettings = false;
        private bool _displayPages = true;
        private bool _displayAddRemoveIcons = false;

        private bool _unappliedDisplayPages;
        private bool _unappliedDisplayAddRemoveIcons;

        private string[] _pageOptions;

        private void OnEnable() {

            Reset();

            _sequence.onReset += Reset;
            Undo.undoRedoPerformed += Reset;

        }

        private void OnDisable() {

            _sequence.onReset -= Reset;
            Undo.undoRedoPerformed -= Reset;

        }

        private void Reset() {

            _numberOfPages = _sequence.GetNumberOfPages();
            //_sequence.SetNumberOfPages(_numberOfPages );

            UpdateCurrentList();
            BuildPageOptions();

            _reorderableList.ClearSelection();

        }

        public override void OnInspectorGUI() {

            EditorGUI.BeginChangeCheck();

            // Update serialized object
            serializedObject.Update();

            DisplaySequenceEditor();

            EditorGUILayout.Space();

            DisplaySettings();

            EditorGUILayout.Space();

            _reorderableList.DoLayoutList();

            if (EditorGUI.EndChangeCheck()) {
                serializedObject.ApplyModifiedProperties();
            }

        }

        private void AddAction(int typeIndex) {

            // Record Object's current state
            Undo.RecordObject( _sequence, "Add Action" );

            // Get Action list by page index
            var _actionList = _sequence.GetListByPage( _selectedPageIndex );

            if(_reorderableList.selectedIndices.Count > 0 && _reorderableList.selectedIndices[0] != _actionList.Count - 1) {

                //_actionList.Insert(_reorderableList.selectedIndices[0] + 1, _action);

                _sequence.AddAction( _selectedPageIndex, typeIndex, _reorderableList.selectedIndices[0] );

            } else {

                //_actionList.Add( _action );

                _sequence.AddAction( _selectedPageIndex, typeIndex );

                

            }

            EditorUtility.SetDirty( _sequence );

        }

        private void RemoveAction() {
            
            // Get Action list by page index
            var _actionList = _sequence.GetListByPage( _selectedPageIndex );

            int _lastSelectedIndex = 0;

            if (_reorderableList.selectedIndices.Count > 0) {

                _lastSelectedIndex = _reorderableList.selectedIndices[0];

                // Record Object's current state
                Undo.RecordObject(  _sequence, "Remove Action" );

                _actionList[_reorderableList.selectedIndices[0]].OnRemove();

                _actionList.RemoveAt( _reorderableList.selectedIndices[0] );

                _reorderableList.ClearSelection();

                if(_actionList.Count > 0) {

                    if(_lastSelectedIndex < _actionList.Count) {

                        // Select element that replaced the removed element's place
                        _reorderableList.Select( _lastSelectedIndex);

                    } else {

                        _reorderableList.Select( _lastSelectedIndex - 1 );

                    }

                    EditorUtility.SetDirty( _sequence );

                }

            }

        }

        private void DisplaySettings() {

            if(_displaySettings) {

                GUILayout.BeginVertical(GUI.skin.box);

                EditorGUILayout.LabelField( "Settings for this Sequence" );

                EditorGUILayout.Space();

                _numberOfPages = EditorGUILayout.IntField( "Number of pages ", Mathf.Clamp(_numberOfPages, 1, 32) );

                _unappliedDisplayPages = EditorGUILayout.Toggle( "Page selection ", _unappliedDisplayPages);
                _unappliedDisplayAddRemoveIcons = EditorGUILayout.Toggle( "Add/Remove icons ", _unappliedDisplayAddRemoveIcons );

                EditorGUILayout.Space();

                GUILayout.BeginHorizontal();

                if(GUILayout.Button( "Apply" )) {

                    Undo.RecordObject( this, "Apply Sequence Settings" );

                    _sequence.SetNumberOfPages( _numberOfPages );

                    _displayPages = _unappliedDisplayPages;
                    _displayAddRemoveIcons = _unappliedDisplayAddRemoveIcons;

                    BuildPageOptions();
                    UpdateCurrentList();
                    Repaint();

                    EditorUtility.SetDirty( _sequence );

                }

                if (GUILayout.Button( "Close" )) {

                    _displaySettings = false;

                }

                GUILayout.EndHorizontal();
                
                GUILayout.EndVertical();

            } else {

                _unappliedDisplayPages = _displayPages;
                _numberOfPages = _sequence.GetNumberOfPages();

            }

        }

        private void BuildPageOptions() {

            // Reset selection to the first page
            _selectedPageIndex = 0;

            _pageOptions = new string[_numberOfPages];

            for (int i = 0; i < _numberOfPages; i++) {

                _pageOptions[i] = "Page " + (i + 1);

            }

        }

        private void DisplaySequenceEditor() {

            // Begin horizontal group
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField( "Action: ", GUILayout.Width(50));
            
            // Create dropdown
            _selectedTypeIndex = EditorGUILayout.Popup( _selectedTypeIndex, _sequence.GetFactory().GetNames() );

            // Create buttons 
            if (GUILayout.Button( "Insert" )) AddAction(_selectedTypeIndex);

            if(_reorderableList.selectedIndices.Count == 0) {

                GUI.enabled = false;

            }

            if (GUILayout.Button( "Remove" )) RemoveAction();

            GUI.enabled = true;

            GUIStyle iconButtonStyle = GUI.skin.FindStyle( "IconButton" ) ?? EditorGUIUtility.GetBuiltinSkin( EditorSkin.Inspector ).FindStyle( "IconButton" );
            GUIContent content = new GUIContent( EditorGUIUtility.Load( "icons/d__Popup.png" ) as Texture2D );

            // Toggle display settings on/off with button icon
            if (EditorGUILayout.DropdownButton( content, FocusType.Passive, iconButtonStyle )) {

                _displaySettings = !_displaySettings;

            }

            // End horizontal group
            EditorGUILayout.EndHorizontal();

            //EditorGUILayout.HelpBox( "Use add and remove buttons to edit the action list. ", MessageType.None );

        }

        private void UpdateCurrentList() {

            // Find list property matching current selected page
            _currentList = serializedObject
                .FindProperty( $"actionSets.Array.data[{_selectedPageIndex}].actions" );

            // Create reorderable list
            _reorderableList = new ReorderableList( serializedObject, _currentList, true, true, _displayAddRemoveIcons, _displayAddRemoveIcons ) {

                // Decide how to draw the list header
                drawHeaderCallback = rect => {
                    
                    // Draw list title
                    EditorGUI.LabelField( rect, "Actions" );

                    // Draw page selection dropdown
                    if(_displayPages) {

                        float _initialWidth = rect.width;

                        rect.x = _initialWidth - 46;
                        rect.width = 75;

                        _selectedPageIndex = EditorGUI.Popup( rect, _selectedPageIndex, _pageOptions );

                        // Update current list if page selection changes
                        if (_selectedPageIndex != _lastSelectedPageIndex) {

                            _lastSelectedPageIndex = _selectedPageIndex;

                            UpdateCurrentList();

                            EditorUtility.SetDirty( _sequence );

                        }

                    }

                },

                // Decide how to draw each list element
                drawElementCallback = ( rect, index, isActive, isFocused ) => {

                    if(_currentList.arraySize > 0) {

                        //Debug.Log( "Draw element callback index: " + index );

                        var element = _currentList.GetArrayElementAtIndex( index );
                        var action = _sequence.GetListByPage(_selectedPageIndex)[index];

                        rect.x += 10;

                        Rect _rect = new Rect( rect.x + 5, rect.y, rect.width - 30, rect.height + 15 );
                        Rect _rectangleRect = new Rect( rect.x - 30, rect.y + 1, rect.width + 26, EditorGUIUtility.singleLineHeight );
                        Rect _toggleRect = new Rect( rect.width + 29, rect.y + 1, EditorGUIUtility.singleLineHeight, EditorGUIUtility.singleLineHeight );

                        action.enabled = EditorGUI.Toggle( _toggleRect, action.enabled );

                        if(!action.enabled) GUI.enabled = false;

                        EditorGUI.LabelField( _rectangleRect, "", GUI.skin.box );

                        EditorGUI.PropertyField( _rect, element, new GUIContent( action.visibleName + action.visibleParameters ), true );

                        GUI.enabled = true;

                    }

                },

                // Decide how to calculate element height
                elementHeightCallback = index => {

                    SerializedProperty element = _currentList.GetArrayElementAtIndex( index );

                    if (element.isExpanded) {

                        // Return height based on the number of child properties
                        return EditorGUIUtility.singleLineHeight * element.CountInProperty() + EditorGUIUtility.singleLineHeight;

                    }

                    return EditorGUIUtility.singleLineHeight;

                },

                onRemoveCallback = list =>  {

                    RemoveAction();

                },

                onAddCallback = list => {

                    DisplayActionListPopup();

                }

            };

        }

        private void DisplayActionListPopup() {

            Vector2 mousePos = Event.current.mousePosition;

            GenericMenu menu = new GenericMenu();

            string[] _names = _sequence.GetFactory().GetNames();

            for (int i = 0; i < _names.Length; i++) {

                int n = i;

                menu.AddItem( new GUIContent( _names[i] ), false, () => AddAction( n ) );

            }

            menu.DropDown( new Rect(mousePos.x, mousePos.y, 0, 0) );

            Event.current.Use();

        }

    }

}


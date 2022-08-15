using UnityEngine;
using UnityEditor;
using System;
using System.Linq;
using System.Reflection;

namespace Theogramme.SequenceScript
{
    
    [CustomEditor(typeof(SequencePlayer))]
    public class SequencePlayerEditor : Editor
    {

        private bool _allowInEditMode = false;

        private bool _foldoutSettings = false;
        private bool _foldoutReferences = false;

        private string[] _pageOptions;

        private Sequence _lastSequence;
        private SequencePlayer _sequencePlayer;

        private void OnEnable() {

            BuildPageOptions();

        }

        public override void OnInspectorGUI() {

            EditorGUI.BeginChangeCheck();

            _sequencePlayer = serializedObject.targetObject as SequencePlayer;

            DrawControls();
            DrawSettings();

            if(EditorGUI.EndChangeCheck()) {

                serializedObject.ApplyModifiedProperties();

            }

        }

        private void DrawControls() {

            SequencePlayer _sequencePlayer = (target as SequencePlayer);

            EditorGUILayout.LabelField( "Editor controls: " );

            if (_sequencePlayer.sequence == null || _sequencePlayer.sequence != _lastSequence) {

                _lastSequence = _sequencePlayer.sequence;

                BuildPageOptions();

            }

            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.PropertyField( serializedObject.FindProperty( "sequence" ), new GUIContent( "" ) );
            EditorGUILayout.PropertyField( serializedObject.FindProperty( "container" ), new GUIContent( "" ) );

            if (!_allowInEditMode || _sequencePlayer.sequence == null) {

                GUI.enabled = false;

            }


            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();

            if (_pageOptions != null) {

                _sequencePlayer.currentPage = EditorGUILayout.Popup( _sequencePlayer.currentPage, _pageOptions );

            }

            if (GUILayout.Button( "Play", GUILayout.Width( 75 ) )) _sequencePlayer.Play();
            if (GUILayout.Button( "Stop", GUILayout.Width( 75 ) )) _sequencePlayer.Stop();

            GUI.enabled = true;

            EditorGUILayout.EndHorizontal();

        }

        private void DrawSettings() {

            _foldoutSettings = EditorGUILayout.BeginFoldoutHeaderGroup( _foldoutSettings, "Testing" );

            if (_foldoutSettings) {

                EditorGUILayout.BeginHorizontal();

                _allowInEditMode = EditorGUILayout.ToggleLeft( "Allow Sequencing in Edit Mode", _allowInEditMode );

                if (_allowInEditMode) {

                    EditorGUILayout.HelpBox( "Not recommended!", MessageType.None );

                }

                EditorGUILayout.EndHorizontal();

                EditorGUILayout.Space();


            }

            EditorGUILayout.EndFoldoutHeaderGroup();

        }

        private void BuildPageOptions() {

            var _sequencePlayer = (target as SequencePlayer);

            if(_sequencePlayer.sequence != null) {

                int _number = _sequencePlayer.sequence.GetNumberOfPages();

                _pageOptions = new string[_number];

                for (int i = 0; i < _number; i++) {

                    _pageOptions[i] = "Page " + (i + 1);

                }

            }

        }

    }

}


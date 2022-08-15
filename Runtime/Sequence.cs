using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Theogramme.SequenceScript
{
    
    [System.Serializable]
    public class ActionSet {

        [SerializeReference]
        public List<Action> actions = new List<Action>();

    }

    [CreateAssetMenu(fileName = "Sequence", menuName = "Game/Scripting/Sequence")]
    public class Sequence : ScriptableObject
    {

        public System.Action onReset = delegate { };

        public List<ActionSet> actionSets = new List<ActionSet>();

        [SerializeField]
        private SimpleFactory<Action> _factory = null;

        [SerializeField]
        private int _numberOfPages;

        private void Awake() {
            
            CreateFactory();

        }

        private void OnEnable() {

            CreateFactory();

        }

        private void Reset() {

            SetNumberOfPages( 3 );

            onReset();

        }

        private void OnValidate() {

            foreach (ActionSet set in actionSets) {

                foreach (Action a in set.actions) {

                    a.OnValidate();

                }

            }

        }

        private void CreateFactory() {

            // Get all subclasses of Action in current Assembly
            System.Type[] _types = Assembly.GetAssembly( typeof( Action ) )
                .GetTypes()
                .Where( t => t.IsClass && !t.IsAbstract && t.IsSubclassOf( typeof( Action ) ) ).ToArray();

            // Create Factory
            _factory = new SimpleFactory<Action>( _types );

        }

        public void AddAction(int pageIndex, int typeIndex, int index = -1) {

            Action _action = _factory.CreateInstance( _factory.GetNames()[typeIndex] );

            _action.OnAdd();

            var _list = actionSets[pageIndex].actions;

            if (index != -1) {

                // Insert new action at index position
                _list.Insert( index, _action );

            } else {

                // Add the action to the end of the list
                _list.Add( _action );

            }

        }

        public void RemoveActionAt(int pageIndex, int index) {

            Action _action = actionSets[pageIndex].actions[index];

            if (_action != null) {

                _action.OnRemove();

                actionSets[pageIndex].actions.RemoveAt( index );

            }

        }

        public List<Action> GetListByPage( int pageIndex ) {

            return actionSets[pageIndex].actions;

        }

        public SimpleFactory<Action> GetFactory() {

            if(_factory == null) CreateFactory();

            return _factory;

        }

        public int GetNumberOfPages() {

            return _numberOfPages;

        }

        public void SetNumberOfPages(int number) {

            Debug.Log( "Setting number of pages to " + number );

            _numberOfPages = number;

            if (actionSets.Count < _numberOfPages) {

                while (actionSets.Count < _numberOfPages) {

                    actionSets.Add( new ActionSet() );

                }

            } else {

                while (actionSets.Count > _numberOfPages) {

                    actionSets.RemoveAt( actionSets.Count - 1 );

                }

            }

        }

        /*public string[] GetActionNames() {

            string[] _names = GetFactory().GetNames();

            for (int i = 0; i < _names.Length; i++) {

                _names[i] = Regex.Replace( _names[i], "([A-Z])", " $1" ).Trim();

            }

            return _names.ToArray();

        }*/

    }

}

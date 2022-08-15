using System.Collections;
using UnityEngine;

namespace Theogramme.SequenceScript
{
    
    public class SequencePlayer : MonoBehaviour
    {

        public Sequence sequence;
        public ObjectContainer container;

        [HideInInspector]
        public bool playing;

        [HideInInspector]
        public int currentPage = 0;

        public void Play() {

            StartCoroutine( _Play( currentPage ) );

        }

        private IEnumerator _Play(int index) {

            playing = true;

            var _actions = sequence.GetListByPage(index);

            if(_actions.Count > 0) {

                foreach (Action a in _actions) {

                    if(!a.enabled) continue;

                    // Start coroutine and pass in the resolver
                    yield return StartCoroutine(a.GetCoroutine(container));

                }

            }

            yield return null;

            playing = false;

        }

        public void Stop() {



        }

    }

}


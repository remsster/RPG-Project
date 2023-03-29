using RPG.Control;
using RPG.Core;
using UnityEngine;
using UnityEngine.Playables;

namespace RPG.Cinematics
{
    public class CinematicControlRemover : MonoBehaviour
    {
        private PlayableDirector playableDirector;
        GameObject player;

        private void Awake()
        {
            player = GameObject.FindWithTag("Player");
            playableDirector = GetComponent<PlayableDirector>();
        }

        private void OnEnable()
        {
            playableDirector.played += DisableContol;
            playableDirector.stopped += EnableControl;
        }

        private void OnDisable()
        {
            playableDirector.played -= DisableContol;
            playableDirector.stopped -= EnableControl;
        }

        private void DisableContol(PlayableDirector director)
        {
            
            player.GetComponent<ActionScheduler>().CancelCurrentAction();
            player.GetComponent<PlayerController>().enabled = false;
        }

        private void EnableControl(PlayableDirector director)
        {
            player.GetComponent<PlayerController>().enabled = true;
        }
    }
}

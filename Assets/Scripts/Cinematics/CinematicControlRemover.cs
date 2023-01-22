using UnityEngine;
using UnityEngine.Playables;

namespace RPG.Cinematics
{
    public class CinematicControlRemover : MonoBehaviour
    {
        private PlayableDirector playableDirector;

        private void Awake()
        {
            playableDirector = GetComponent<PlayableDirector>();
        }

        private void Start()
        {
            playableDirector.played += DisableContol;
            playableDirector.stopped += EnableControl;
        }

        private void DisableContol(PlayableDirector director)
        {
            print("DisableControl");
        }

        private void EnableControl(PlayableDirector director)
        {
            print("EnableControl");
        }
    }
}

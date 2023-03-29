using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

namespace RPG.SceneManagement
{
    public class Portal : MonoBehaviour
    {
        enum DestinationIdentifier
        {
            A,B,C,D,E
        }

        [SerializeField] private int sceneToLoad = -1;
        [SerializeField] private Transform spawnPoint;
        [SerializeField] DestinationIdentifier destination;

        [SerializeField] private float fadeOutTime = 1f;
        [SerializeField] private float fadeInTime = 3f;
        [SerializeField] private float fadeWaitTime = .5f;


        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Player")
            {
                StartCoroutine(Transition());
            }
        }

        private IEnumerator Transition()
        {
            if (sceneToLoad < 0)
            {
                Debug.LogError("Cannot load scene");
                yield break;
            }

            DontDestroyOnLoad(this.gameObject);
            Fader fader = FindObjectOfType<Fader>();
            SavingWrapper savingWraper = FindObjectOfType<SavingWrapper>();

            yield return fader.FadeOut(fadeOutTime);
            savingWraper.Save();
            yield return SceneManager.LoadSceneAsync(sceneToLoad);

            Portal otherPortal = GetOtherPortal();
            savingWraper.Load();
            UpdatePlayer(otherPortal);
            savingWraper.Save();
            yield return new WaitForSeconds(fadeWaitTime);
            yield return fader.FadeIn(fadeInTime);

            Destroy(this.gameObject);
        }

        private void UpdatePlayer(Portal otherPortal)
        {
            GameObject player = GameObject.FindWithTag("Player");
            player.GetComponent<NavMeshAgent>().enabled = false;
            player.transform.position = otherPortal.spawnPoint.position;
            player.transform.rotation = otherPortal.spawnPoint.rotation;
            player.GetComponent<NavMeshAgent>().enabled = true;
            // player.GetComponent<NavMeshAgent>().Warp(otherPortal.spawnPoint.position);
        }

        private Portal GetOtherPortal()
        {
            foreach(Portal portal in FindObjectsOfType<Portal>())
            {
                if (portal == this) continue;
                if (portal.destination != this.destination) continue;
                return portal;
            }
            // Could not find the other portal
            return null;
        }
    }
}

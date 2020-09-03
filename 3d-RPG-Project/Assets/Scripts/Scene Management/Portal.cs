using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AI;

namespace RPG.SceneManagement{
    public class Portal : MonoBehaviour
    {

        enum DestinationIdentifier
        {
            A, B, C, D, E
        }

        [SerializeField] DestinationIdentifier destination;

        [SerializeField] int loadScene = -1;
        [SerializeField] Transform spawnPoint;

        [SerializeField] float fadeInTime = 1f;
        [SerializeField] float fadeOutTime = 2f;
        [SerializeField] float fadeWaitTime = 0.5f;

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Player")
            {
                StartCoroutine(Transition());
            }
        }

        IEnumerator Transition()
        {
            DontDestroyOnLoad(gameObject);
            Fader fader = FindObjectOfType<Fader>();
            
            yield return fader.FadeOut(fadeOutTime);
            
            yield return SceneManager.LoadSceneAsync(loadScene);

            Portal otherPortal = GetOtherPortal();
            UpdatePlayer(otherPortal);

            yield return new WaitForSeconds(fadeWaitTime);
            yield return fader.FadeIn(fadeInTime);
            
            Destroy(gameObject);
        }

        private void UpdatePlayer(Portal otherPortal)
        {
            GameObject player = GameObject.FindWithTag("Player");
            player.transform.position = otherPortal.spawnPoint.position;
            player.transform.rotation = otherPortal.spawnPoint.rotation;

        }

        private Portal GetOtherPortal()
        {
            foreach (Portal portal in FindObjectsOfType<Portal>())
            {
                if (portal == this)
                {
                    continue;
                }

                if (portal.destination != destination) continue;
                return portal;
            }
            return null;
        }
    }

}
using System;
using System.Collections;
using UnityEditor;
using UnityEngine;


namespace RPG.Combat
{
    public class WeaponPickup : MonoBehaviour
    {
        [SerializeField] private Weapon weapon;
        [SerializeField] private float respawnDelay = 5f;

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                other.GetComponent<Fighter>().EquipWeapon(weapon);
                StartCoroutine(HideForSeconds(respawnDelay));
            }
        }

        private IEnumerator HideForSeconds(float seconds)
        {
            ShowPickup(false);
            yield return new WaitForSeconds(seconds);
            ShowPickup(true);
        }

        private void ShowPickup(bool shouldShow)
        {
            gameObject.GetComponent<Collider>().enabled = shouldShow;
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(shouldShow);
            }
        }
    }
}

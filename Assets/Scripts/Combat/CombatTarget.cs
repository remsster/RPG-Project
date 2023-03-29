using UnityEngine;

using RPG.Attributes;

namespace RPG.Combat
{
    /// <summary>
    /// Class used for identifting combat targets  for the player to interact with
    /// </summary>
    [RequireComponent(typeof(Health))]
    public class CombatTarget : MonoBehaviour { }
}
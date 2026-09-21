using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class NPC : MonoBehaviour
{
    [Header("Targeting")]
    public Transform player;
    [SerializeField] private float interactRange = 5f;

    [Header("Rig")]
    [SerializeField] private Rig rig; // trascina qui il componente Rig dal tuo GameObject
    [SerializeField] private float blendSpeed = 2f;

    private void Update()
    {
        if (player == null || rig == null) return;

        float distance = Vector3.Distance(transform.position, player.position);
        float targetWeight = distance <= interactRange ? 1f : 0f;

        // transizione morbida
        rig.weight = Mathf.MoveTowards(rig.weight, targetWeight, blendSpeed * Time.deltaTime);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, interactRange);
    }
}

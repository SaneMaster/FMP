using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollActivation : MonoBehaviour
{
    private Rigidbody[] rigidbodies;
    private Collider[] colliders;

    void Start()
    {
        // Get all the Rigidbody and Collider components of the ragdoll parts
        rigidbodies = GetComponentsInChildren<Rigidbody>();
        colliders = GetComponentsInChildren<Collider>();

        // Disable the Rigidbody and Collider components initially
        SetRagdollEnabled(false);
    }

    public void SetRagdollEnabled(bool isEnabled)
    {
        // Enable or disable Rigidbody and Collider components based on the provided flag
        foreach (Rigidbody rb in rigidbodies)
        {
            rb.isKinematic = !isEnabled;
        }

        foreach (Collider col in colliders)
        {
            col.enabled = isEnabled;
        }
    }
}


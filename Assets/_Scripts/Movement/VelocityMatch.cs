
using System.Collections.Generic;
using UnityEngine;

namespace Lily.MovementSystem
{

  [RequireComponent(typeof(MovementController))]
  public class VelocityMatch : MonoBehaviour
  {
    public float facingCosine = 90;
    public float timeToTarget = 0.1f;
    public float maxAcceleration = 4f;

    float facingCosineVal;

    Rigidbody rb;
    MovementController steeringBasics;

    void Awake()
    {
        facingCosineVal = Mathf.Cos(facingCosine * Mathf.Deg2Rad);

        rb = GetComponent<Rigidbody>();
        steeringBasics = GetComponent<MovementController>();
    }

    public Vector3 GetSteering(ICollection<Rigidbody> targets)
    {
        Vector3 accel = Vector3.zero;
        int count = 0;

        foreach (Rigidbody r in targets)
        {
            if (steeringBasics.IsLooking(r.position, facingCosineVal))
            {
                /* Calculate the acceleration we want to match this target */
                Vector3 a = r.velocity - rb.velocity;
                /* Rather than accelerate the character to the correct speed in 1 second, 
                * accelerate so we reach the desired speed in timeToTarget seconds 
                * (if we were to actually accelerate for the full timeToTarget seconds). */
                a = a / timeToTarget;

                accel += a;

                count++;
            }
        }

        if (count > 0)
        {
            accel = accel / count;

            /* Make sure we are accelerating at max acceleration */
            if (accel.magnitude > maxAcceleration)
            {
                accel = accel.normalized * maxAcceleration;
            }
        }

        return accel;
    }
  }
}
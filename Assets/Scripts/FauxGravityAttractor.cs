using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// script pro objekt s gravitací
public class FauxGravityAttractor : MonoBehaviour
{
    public float gravity = -10f;

    public void Attract(Transform body, Rigidbody rigidbody)
    {

        Vector3 gravityUp = (body.position - transform.position).normalized;
        Vector3 bodyUp = body.up;

        rigidbody.AddForce(gravityUp * gravity);
        Quaternion targetRotation = Quaternion.FromToRotation(bodyUp, gravityUp) * body.rotation;
        body.rotation = Quaternion.Slerp(body.rotation, targetRotation, 50 * Time.deltaTime);
    }
}
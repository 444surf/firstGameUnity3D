using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityGun : MonoBehaviour
{
    public PlayerLook look;

    private Rigidbody body;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (InputManager.instance.GetKeyDown("Shoot"))
        {
            RaycastHit hit;

            Ray ray = new Ray(look.transform.position, look.transform.forward);

            Physics.Raycast(ray, out hit, 9);

            body = hit.rigidbody;
        }
        if (InputManager.instance.GetKey("Shoot"))
        {
            if (body)
            {
                body.AddForce((look.transform.position + look.transform.forward * 5.0f - body.position) * 1.5f, ForceMode.VelocityChange);
                //body.AddForceAtPosition(10.0f * (look.transform.position + look.transform.forward * 5.0f - body.position), look.transform.position + look.transform.forward * 5.0f);

                body.velocity = body.velocity.normalized * (look.transform.position + look.transform.forward * 5.0f - body.position).magnitude * 2.0f;

                if (((look.transform.position + look.transform.forward * 5.0f) - body.transform.position).magnitude < 1f)
                {
                    body.velocity = Vector3.zero;
                }
            }
        }
    }
}

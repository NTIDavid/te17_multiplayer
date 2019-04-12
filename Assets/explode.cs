using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class explode : NetworkBehaviour
{
    public GameObject explosion;
    // Start is called before the first frame updat
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnCollisionEnter(Collision hit) {
        if((hit.transform.name != "Terrain") && (hit.transform.name != "spike")) {
            if(hit.transform.tag != "myPlayer") {
                Debug.Log(hit.transform.tag);
                
                GameObject ne = Instantiate(explosion);
                ne.SetActive(true);
                ne.transform.position = transform.position+transform.up;
                NetworkServer.Spawn(ne);
                Collider[] hitColliders = Physics.OverlapSphere(transform.position, 50);
                int i = 0;
                while (i < hitColliders.Length)
                {
                    if(hitColliders[i].GetComponent<Rigidbody>() != null) {
                        hitColliders[i].GetComponent<Rigidbody>().AddExplosionForce(1600, transform.position, 50, 2f);
                    }
                    i++;
                }
                GameObject.Destroy(gameObject);
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class control : NetworkBehaviour
{
    public float speed = 0.0001F;
    CharacterController controller;
    Animator ani;
    bool hitting = false;
    public Transform hand;
    public GameObject spike;
    [SyncVar]
    Color playerColor;
    void Start()
    {
        //controller = GetComponent<CharacterController>();
        if(isLocalPlayer) {
            playerColor = new Color(Random.value, Random.value, Random.value, 1.0f);
            tag = "myPlayer";
            ani = GetComponent<Animator>();
        } else {
            transform.Find("Main Camera").GetComponent<Camera>().enabled = false;
            //controller.enabled = false;
        }

        GameObject.Find("lobbycam").GetComponent<Camera>().enabled = false;
    }
    void Update()
    {
        if(isLocalPlayer) {
            transform.Find("Mesh_Brute").GetComponent<Renderer>().material.color = playerColor;
            transform.Rotate(new Vector3(0, Input.GetAxis("Horizontal")*3, 0));
            if(Input.GetAxis("Vertical") > 0f) {
                //transform.Translate(new Vector3(0f, 0f, Input.GetAxis("Vertical")*0.1f));
                float s = Input.GetAxis("Vertical")*(1+(Input.GetAxis("Run")*0.5f));
                if(s < 1f) {
                    s = 1f;
                }
                if(hitting != true) {
                    ani.speed = s*1.6f;
                }
                ani.SetBool("MovingBack", false);
                ani.SetBool("Moving", true);
            } else if(Input.GetAxis("Vertical") < 0f) {
                //transform.Translate(new Vector3(0f, 0f, Input.GetAxis("Vertical")*0.1f));
                float s = Input.GetAxis("Vertical");
                if(s < 1f) {
                    s = 1f;
                }
                if(hitting != true) {
                    ani.speed = s*1f;
                }
                ani.SetBool("Moving", false);
                ani.SetBool("MovingBack", true);
            } else {
                ani.SetBool("Moving", false);
                ani.SetBool("MovingBack", false);
            }
            if(Input.GetButtonDown("Fire1")) {
                hitting = true;
                ani.speed = 1;
                transform.GetComponent<NetworkAnimator>().SetTrigger("Attack1Trigger");
                ani.SetTrigger("Attack1Trigger");
            }
            if(Input.GetButtonDown("Fire2")) {
                hitting = true;
                ani.speed = 1;
                transform.GetComponent<NetworkAnimator>().SetTrigger("Attack2Trigger");
                ani.SetTrigger("Attack2Trigger");
            }
        }
    }
    void Hit() {
        RaycastHit hit;
        if(Physics.Raycast(hand.position, hand.forward, out hit, 100f)){
            //Debug.Log("hit");
        }
        
        CmdFire1();
        hitting = false;
        ani.ResetTrigger("Attack1Trigger");
    }
    void Hit2() {
        RaycastHit hit;
        if(Physics.Raycast(hand.position, hand.TransformDirection(Vector3.forward), out hit, 100f)){
            //Debug.Log("hit2");
        }
        hitting = false;
        ani.ResetTrigger("Attack2Trigger");
    }
    void FootR() {
        //Debug.Log("Right leg");
    }
    void FootL() {
        //Debug.Log("Left leg");
    }
    [Command]
    void CmdFire1() {
        GameObject newSpike = Instantiate(spike);
        newSpike.SetActive(true);
        newSpike.transform.position = transform.position+(transform.forward*5)+(transform.up*0.2f);
        newSpike.GetComponent<Rigidbody>().AddForce(transform.forward*20000);
        NetworkServer.Spawn(newSpike);
    }
}

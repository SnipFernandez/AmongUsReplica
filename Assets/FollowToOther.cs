using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowToOther : MonoBehaviour
{
    public Transform target;
    public bool modifyX;
    public bool modifyY;
    public bool modifyZ;

    float initialX;
    float initialY;
    float initialZ;

    float initialtargetX;
    float initialtargetY;
    float initialtargetZ;


    // Start is called before the first frame update
    void Start()
    {
        initialX = transform.position.x;
        initialY = transform.position.y;
        initialZ = transform.position.z;
        initialtargetX = target.position.x;
        initialtargetY = target.position.y;
        initialtargetZ = target.position.z;

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 dest = new Vector3((modifyX) ? (target.position.x-initialtargetX) +initialX : transform.position.x,
            (modifyY) ? (target.position.y-initialtargetY)+initialY : transform.position.y,
            (modifyZ) ? (target.position.z-initialtargetZ)+initialZ : transform.position.z);
        
        transform.position = dest;
    }
}

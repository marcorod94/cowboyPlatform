using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player;
    public float offset = 6f;
    
    // Update is called once per frame
    void Update()
    {
        if(this.player == null)
        {
            return;
        }
        this.transform.position = new Vector3(this.player.position.x + offset, this.transform.position.y, this.transform.position.z);    
    }
}

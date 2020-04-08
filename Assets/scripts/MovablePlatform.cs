using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovablePlatform : MonoBehaviour
{
    public GameObject platform;
    public float speed;
    public Transform currenntPoint;
    public Transform[] poitns;
    public int pointSelection;
    // Start is called before the first frame update
    void Start()
    {
        this.currenntPoint = this.poitns[this.pointSelection];
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(this.platform.gameObject == null)
        {
            return;
        }
        this.platform.transform.position = Vector3.MoveTowards(this.platform.transform.position,
            this.currenntPoint.position, Time.deltaTime * speed);
        if (this.platform.transform.position == this.currenntPoint.position)
        {
            this.pointSelection ++;
            if(this.pointSelection == this.poitns.Length)
            {
                this.pointSelection = 0;
            }
            this.currenntPoint = this.poitns[this.pointSelection];
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
    private void OnMouseDrag() {
        float rotSpeed = 20;
        float rotX = Input.GetAxis("Mouse X") * rotSpeed * Mathf.Deg2Rad;
       // float rotY = Input.GetAxis("Mouse Y") * rotSpeed * Mathf.Deg2Rad;
        transform.RotateAround(Vector3.up, -rotX);
        //transform.RotateAround(Vector3.right, rotY);

    }

    // Update is called once per frame
    void Update () {
        //transform.localEulerAngles = new Vector3(90, 0, 0);
        if (Input.GetButtonDown("Left")) {
            Debug.Log("Left");
            //Debug.Log(transform.eulerAngles);
            //Debug.Log(transform.localRotation);
            //Debug.Log(transform.rotation);
            //if (transform.localEulerAngles.x == 90) transform.localEulerAngles = new Vector3(-180, 0, 0);
            //else
            //transform.localEulerAngles = transform.localEulerAngles + new Vector3(90, 0, 0);
            //Debug.Log(transform.RotateAround);
            //transform.rotat
            //transform.
            //Quaternion  q = new Quaternion(0, 0, 0, 0);
            //x
            //transform.rotation = new Quaternion(90 - 25, 0 - 50, 0 + 25, 0) ;
            //transform.rotation = new Quaternion( - 0.01f, 0, 0, 0);
            //float rotX = Input.GetAxis("Mouse X") * rotSpeed * Mathf.Deg2Rad;
            float rotX = 90 * Mathf.Deg2Rad;
            // float rotY = Input.GetAxis("Mouse Y") * rotSpeed * Mathf.Deg2Rad;
            //transform.RotateAround(Vector3.up, -rotX);
            transform.RotateAroundLocal(Vector3.back, -rotX);

            Debug.Log(transform.localEulerAngles);
            if (transform.localEulerAngles.x == -90 || transform.localEulerAngles.x == 270) {
                Debug.Log(1);
                //transform.Rotate(new Vector3(0, 90, 0));
                //transform.localEulerAngles = new Vector3(0, -90, 90);
                //transform.Rotate(Vector3.down * 90);
            }
            else {

                //around z
                //transform.Rotate(new Vector3(0, 0, 90));

            }


        }
        if (Input.GetButtonDown("Right")) {
            Debug.Log("Right");
            //transform.Rotate(new Vector3(-90, 0, 0));
            float rotX = 90 * Mathf.Deg2Rad;
            // float rotY = Input.GetAxis("Mouse Y") * rotSpeed * Mathf.Deg2Rad;
            //transform.RotateAround(Vector3.up, -rotX);
            transform.RotateAroundLocal(Vector3.right, -rotX);
        }
    }
}

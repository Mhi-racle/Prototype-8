using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMotor : MonoBehaviour
{
    public Transform lookAt;
    public Vector3 offset;
    public Vector3 rotation = new Vector3(35, 0, 0);
    public bool IsMoving { set; get; }

    void Start()
    {
        transform.position = lookAt.position + offset;
        IsMoving = false;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        
        Vector3 desiredPosition = lookAt.position + offset;
        desiredPosition.x = 0;
        transform.position = Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime);
        transform.rotation = Quaternion.Euler(rotation);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    private float rotateTimer = 0f;
    private Vector3 rotation;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        /*if (rotateTimer <= 0f)
        {
            rotation = new Vector3(Random.Range(-0.2f, 0.2f), Random.Range(-0.2f, 1.2f), 0f);
            rotateTimer = 3f;
        }
        rotateTimer -= Time.deltaTime;
        gameObject.transform.Rotate(rotation * Time.deltaTime);*/
    }
}

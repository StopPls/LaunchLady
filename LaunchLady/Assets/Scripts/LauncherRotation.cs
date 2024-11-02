using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class LauncherRotation : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0f; // zero z

        var XDif = mouseWorldPos.x - transform.position.x;
        var YDif = mouseWorldPos.y -transform.position.y;
        var tanny = Mathf.Atan2(YDif, XDif);
        transform.rotation = Quaternion.Euler(Vector3.forward * tanny*(360/(Mathf.PI*2)));
    }
}

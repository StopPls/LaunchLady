using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using System.Linq;
using UnityEngine;
public class LauncherRotation : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] Transform Muzzle;
    [SerializeField] GameObject ProjectilePrefab;

    private float fireDebounce = 0.5f;
    private float lastShotTime;
    private float bulletSpeed;
    private List <GameObject> Bullets = new List<GameObject>(); 
    void Start()
    {
        lastShotTime = Time.timeSinceLevelLoad;
        bulletSpeed = .05f;
    }

    // Update is called once per frame
    void Update()
    {
        for(int i = 0;i< Bullets.Count; i++)
        {
            if(Bullets.ElementAt(i) != null)
            {
                Bullets.ElementAt(i).transform.position = Bullets.ElementAt(i).transform.position + Bullets.ElementAt(i).transform.right * bulletSpeed;
            }
        }
        var mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0f; // zero z

        var XDif = mouseWorldPos.x - transform.position.x;
        var YDif = mouseWorldPos.y -transform.position.y;
        var tanny = Mathf.Atan2(YDif, XDif);
        transform.rotation = Quaternion.Euler(Vector3.forward * tanny*(360/(Mathf.PI*2)));
        if(Input.GetMouseButtonDown(0))
        {
            if(Time.timeSinceLevelLoad - lastShotTime > fireDebounce)
            {
                Debug.Log("Fire A Shot!");
                lastShotTime = Time.timeSinceLevelLoad;
                var Bullet = Instantiate(ProjectilePrefab,Muzzle.position, transform.rotation);
                Bullets.Add(Bullet);
            }
        }
    }
}

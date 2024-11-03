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
    [SerializeField] GameObject PreviewDotPrefab;

    //Initialize Bullet Variables;
    private float fireDebounce;
    private float lastShotTime;
    private float bulletSpeed, bulletGravity;
    private List<GameObject> Bullets = new List<GameObject>();

    //Initialize Preview Variables
    private float numPreviewDots;
    private float maxPreviewTime;
    private List<GameObject> PreviewDots = new List<GameObject>();

    void Start()
    {
        //Assign Bullet Variables
        lastShotTime = Time.timeSinceLevelLoad;
        //bulletSpeed = 10f;
        bulletSpeed = 10f;
        bulletGravity = 5f;
        fireDebounce = 0.5f;

        //Assign Preview Variables
        numPreviewDots = 5f;
        maxPreviewTime = 1.2f;

        //Initialize Preview Array and Dot Game Objects
        for(int i = 0;i<numPreviewDots;i++)
        {
            var previewdot = Instantiate(PreviewDotPrefab, new Vector3(100, 100, 0), Quaternion.identity);
            PreviewDots.Add(previewdot);
        }
    }

    // Update is called once per frame
    void Update()
    {
        float DownwardsSpeed;

        //Loop through all bullets, applying gravity and projectile speed
        for (int i = 0;i< Bullets.Count; i++)
        {
            if(Bullets.ElementAt(i) != null)
            {
                //use a variable on the prefab in order to simulate accelerating gravity
                DownwardsSpeed = (float)Variables.Object(Bullets.ElementAt(i).GetComponent<Variables>()).Get("DownVelocity");
                Variables.Object(Bullets.ElementAt(i).GetComponent<Variables>()).Set("DownVelocity", DownwardsSpeed + bulletGravity*Time.deltaTime);
                DownwardsSpeed = (float)Variables.Object(Bullets.ElementAt(i).GetComponent<Variables>()).Get("DownVelocity");

                //Define our target(way we are moving and bullet positions
                Vector3 targetPos = (Bullets.ElementAt(i).transform.position + Bullets.ElementAt(i).transform.right * bulletSpeed * Time.deltaTime) -new Vector3(0, DownwardsSpeed * Time.deltaTime, 0);
                Vector3 bulletPos = (Bullets.ElementAt(i).transform.position);

                //Actually Move Our Bullet
                Bullets.ElementAt(i).transform.position = (Bullets.ElementAt(i).transform.position + Bullets.ElementAt(i).transform.right * bulletSpeed * Time.deltaTime) - new Vector3(0, DownwardsSpeed * Time.deltaTime,0);
               
                //Adjust The Visual Rotation of the projectile 
                if (Bullets.ElementAt(i).transform.Find("Visual") != null)
                {
                    Bullets.ElementAt(i).transform.Find("Visual").transform.right = targetPos - bulletPos;
                }
            }
        }

        //Rotate our launcher towards the mouse position
        var mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0f; // zero z

        var XDif = mouseWorldPos.x - transform.position.x;
        var YDif = mouseWorldPos.y -transform.position.y;
        var tanny = Mathf.Atan2(YDif, XDif);
        transform.rotation = Quaternion.Euler(Vector3.forward * tanny*(360/(Mathf.PI*2)));

        //Generate Path Arc "Preview"
        for(int i = 0; i<PreviewDots.Count; i++)
        {
            float dotTime = (i+1) * (maxPreviewTime / numPreviewDots);
            Debug.Log(dotTime);
            PreviewDots.ElementAt(i).transform.position = transform.position + (new Vector3((transform.right * bulletSpeed).x * dotTime *1.15f , 0 , 0) + new Vector3(0,((transform.right * bulletSpeed).y * dotTime * 1.1f) + (0.5f * -bulletGravity * (dotTime * dotTime)),0));
        }

        //Fire a Projectile if The Debounce says we can!
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
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCameraControl : MonoBehaviour
{
    [SerializeField] private Transform target, player, obstruction;

    [SerializeField] private float rotationSpeed = 1;
    [SerializeField] private float zoomSpeed = 2f;
    [SerializeField] private float mouseX, mouseY;
    [SerializeField] private float prevPosX,prevPosY;
   


    [SerializeField] private bool snapTo;

 
  
    private void Awake()
    {
        obstruction = target;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
  

    void CamControl()
    {
    
        mouseX += Input.GetAxis("Mouse X") * rotationSpeed;
        mouseY -= Input.GetAxis("Mouse Y") * rotationSpeed;
        mouseY = Mathf.Clamp(mouseY, -35, 60);
   
      
        transform.LookAt(target);



        if (Input.GetKey(KeyCode.Tab))
        {
            
            snapTo = false;
            target.rotation = Quaternion.Euler(mouseY, mouseX, 0);
        }
        else
        {

            if (!snapTo)
            {
                mouseX = prevPosX;
                mouseY = prevPosY;
                snapTo = true;

            }
            else 
            {
                target.rotation = Quaternion.Euler(mouseY , mouseX, 0) ;
                player.rotation = Quaternion.Euler(0, mouseX, 0);
            }
        }
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            prevPosX = mouseX;
            prevPosY = mouseY;
        }
      
        
    }
  
    void ViewObstructed()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, target.position - transform.position, out hit, 4.5f))
        {

            if (hit.collider.gameObject.tag != "Player" && hit.collider.gameObject.tag != "Target")
            {
                obstruction = hit.transform;

                obstruction.gameObject.GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly;

                if (Vector3.Distance(obstruction.position, transform.position) >= 3f && Vector3.Distance(transform.position, target.position) >= 1.5f)
                { transform.Translate(Vector3.forward * zoomSpeed * Time.deltaTime); }
            }
            else
            {
                if (obstruction.gameObject.GetComponent<MeshRenderer>() != null)
                {
                    obstruction.gameObject.GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
              //      if (Vector3.Distance(transform.position, target.position) < 4.5f)
                //        transform.Translate(Vector3.back * zoomSpeed * Time.deltaTime);
                }
                else
                {
                    return;               
                }
            }

        }
    }
    private void FixedUpdate()
    {
        CamControl();
        ViewObstructed();
    }
}
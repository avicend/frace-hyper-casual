using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;
using DG.Tweening;

public class PlayerFollower : MonoBehaviour
{
    public PathCreator pathCreator;
    public float speed = 5;
    float distanceTravelled;
    Animator animator;

    PlayerRagdoll ragdollActivate;
    Rigidbody playerRB;
    public EndOfPathInstruction endOfPathInstruction;

    public GameObject _camera;
    private CameraController _camCont;


    private bool _userInput = false;
    private bool _isInAnyZone = false;
    private bool _shouldRun = true;
    private bool _isCollidedWithTrap = false;
    private bool _isCamZone = false;

    private void Start()
    {
        animator = GetComponent<Animator>();
        transform.position = transform.position = pathCreator.path.GetPointAtDistance(distanceTravelled, endOfPathInstruction);
        transform.rotation = pathCreator.path.GetRotationAtDistance(distanceTravelled, endOfPathInstruction);
        ragdollActivate = GetComponent<PlayerRagdoll>();
        ragdollActivate.SetActiveRagdollPhysics(false);
        //playerRB = GetComponent<Rigidbody>();
        _camCont = _camera.GetComponent<CameraController>();
        _camCont.cameraTransition = 2;
        
    }
    // Update is called once per frame
    void Update()
    {
        if (!_isCollidedWithTrap)
        {
            if (Input.touchCount > 0 || Input.GetKey("space"))
            {
                
                _userInput = true;
                TravelAlongPath();
                Debug.Log("Update if");
                //running along path no zone effect
                if (_shouldRun)
                {
                    animator.SetBool("isRunning", true);
                }
               
                animator.SetBool("isIdle", false);


            }
            
            else
            {
                Debug.Log("Update else");
                _userInput = false;
                //staying idle in no zone
                
                animator.SetBool("isRunning", false);
                if (!_isInAnyZone || _isCamZone)
                {
                    animator.SetBool("isIdle", true);
                }
                else if (_isInAnyZone)
                {
                    animator.SetBool("isIdle", false);
                }


            }
        }

        


    }


    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Trap" || other.gameObject.tag == "turningTrap" || other.gameObject.tag == "loopTrap")
        {
           
            _isCollidedWithTrap = true;
            ragdollActivate.SetActiveRagdollPhysics(true);
            speed = 20f;
            StartCoroutine(CloseTheControls());
            

        }



        if (
            other.gameObject.tag == "CrouchZone" 
            || other.gameObject.tag == "FlyZone" 
            || other.gameObject.tag == "CrawlZone" 
            || other.gameObject.tag == "DanceZone" 
            || other.gameObject.tag == "BehindZone"
            || other.gameObject.tag == "ProfileZone"

            )
        {
            
            _isInAnyZone = true;
        }

        if (other.gameObject.tag == "CrawlZone")
        {
            speed = speed / 3;
        }

        if (other.gameObject.tag == "CrouchZone" || other.gameObject.tag == "BehindZone")
        {
            _camCont.cameraTransition = 2;
        }
        else if (other.gameObject.tag == "CrawlZone") 
        {
            _camCont.cameraTransition = 3;
        }
        else if (other.gameObject.tag == "DanceZone")
        {
            _camCont.cameraTransition = 4;
            animator.SetBool("isIdle", false);
            animator.SetBool("isRunning", false);
            animator.SetBool("isCrouching", false);
            animator.SetBool("isCrawling", false);
            animator.SetBool("isDancing",true);

            _isCollidedWithTrap = true;

          
        }
        else if (other.gameObject.tag == "ProfileZone")
        {
            _camCont.cameraTransition = 5;
            _isCamZone = true;
        }
        else if (other.gameObject.tag == "BehindZone")
        {
            _camCont.cameraTransition = 1;
            _isCamZone = true;
        }
        else if (!_isInAnyZone)
        {
            _camCont.cameraTransition = 2;
        }

    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "CrouchZone")
        {
            
            if (_userInput)
            {
               
                animator.SetBool("isRunning", true);
                animator.SetBool("isCrouching", false);
                animator.SetBool("isIdle", false);
               
                
            }
            else if (!_userInput)
            {
               
                animator.SetBool("isRunning", false);
                animator.SetBool("isCrouching", true);
                animator.SetBool("isIdle", false);

                
            }
        }

        if (other.gameObject.tag == "FlyZone")
        {
            animator.SetBool("isIdle", false);
            animator.SetBool("isRunning", false);

            if (_userInput)
            {
                animator.SetBool("isFlying", true);
                animator.SetBool("isFloating", false);
                animator.SetBool("isIdle", false);
            }
            else if (!_userInput)
            {
                animator.SetBool("isFlying", false);
                animator.SetBool("isFloating", true);
                animator.SetBool("isIdle", false);
            }
        }

        if (other.gameObject.tag == "CrawlZone")
        {
            _shouldRun = false;
            if (_userInput)
            {

                animator.SetBool("isCrawling", true);
                animator.SetBool("isCrouching", false);             
                animator.SetBool("isIdle", false);
           

            }
            else if (!_userInput)
            {

                animator.SetBool("isCrawling", false);
                animator.SetBool("isCrouching", true);
                animator.SetBool("isIdle", false);
                

            }
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "CrouchZone" || other.gameObject.tag == "FlyZone" || other.gameObject.tag == "CrawlZone")
        {

            animator.SetBool("isRunning", true);
            animator.SetBool("isFlying", false);
            animator.SetBool("isCrouching", false);
            animator.SetBool("isIdle", true);
            animator.SetBool("isCrawling", false);

            _isInAnyZone = false;
            _shouldRun = true;
            _camCont.cameraTransition = 2;
            
        }

        if (other.gameObject.tag == "CrawlZone")
        {

            speed = speed * 3;
        }

        if (other.gameObject.tag == "ProfileZone" || other.gameObject.tag == "BehindZone")
        {
            _isCamZone = false;
            _isInAnyZone = false;
            _shouldRun = true;
        }
    }

    void TravelAlongPath()
    {
        distanceTravelled += speed * Time.deltaTime;

        transform.position = pathCreator.path.GetPointAtDistance(distanceTravelled, endOfPathInstruction);

        transform.rotation = pathCreator.path.GetRotationAtDistance(distanceTravelled, endOfPathInstruction);
    }

    IEnumerator CloseTheControls()
    {

        yield return new WaitForSeconds(5);
        distanceTravelled = distanceTravelled - 50f;
        if (distanceTravelled<0)
        {
            distanceTravelled = 0;
        }
        transform.position = pathCreator.path.GetClosestPointOnPath(pathCreator.path.GetPointAtDistance(distanceTravelled, endOfPathInstruction));
        transform.rotation = pathCreator.path.GetRotationAtDistance(distanceTravelled, endOfPathInstruction);
       
        ragdollActivate.SetActiveRagdollPhysics(false);

        

        animator.SetBool("isRunning", false);
        animator.SetBool("isFlying", false);
        animator.SetBool("isCrouching", false);
        animator.SetBool("isIdle", true);
        _isCollidedWithTrap = false;
        _isInAnyZone = false;
    }

   
}

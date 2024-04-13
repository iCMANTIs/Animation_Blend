using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class PlayerController : MonoBehaviour
{
    public Animator animator;

    public LayerMask layerMask;

    private float resetTimer;
    public float AttackSwitchDelay = 0.5f;

    private int AttackCount;

    [Range(0,1f)]
    public float distance;

    private void Update()
    {

        bool isWalking = Input.GetKey(KeyCode.W);
        animator.SetBool("IsWalking", isWalking);


        bool isRunning = Input.GetKey(KeyCode.LeftControl);
        animator.SetBool("IsRunning", isRunning);


        bool isSprinting = Input.GetKey(KeyCode.LeftShift);
        animator.SetBool("IsSprinting", isSprinting);


        //bool Attack = Input.GetKey(KeyCode.Space);





        if (Input.GetMouseButtonDown(0))
        {
            if (resetTimer <= 0 || AttackCount == 0)  
            {
                AttackCount = 1;
                animator.SetTrigger("IsAttacking");
            }
            else if (resetTimer > 0 && AttackCount < 5)
            {
                AttackCount++;
                animator.SetTrigger("IsAttacking");
            }
            animator.SetInteger("AttackCount", AttackCount);
            
            resetTimer = AttackSwitchDelay;
        }

        
        if (Input.GetKeyDown(KeyCode.G))
        {
            animator.SetTrigger("AttackE_Trigger");
            AttackCount = 0;  
        }

        
        if (resetTimer >= 0)
        {
            resetTimer -= Time.deltaTime;
        }
        else
        {
            if (AttackCount != 0)
            {
                animator.SetTrigger("ResetAttack");
                Debug.LogError("reset");
                AttackCount = 0;
                Debug.LogError("AttackCount = " + AttackCount);
            }
        }



        if (Input.GetKeyUp(KeyCode.W))
        {
            animator.SetTrigger("StopWalking");
        }

        if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            animator.SetTrigger("StopRunning");
        }

        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            animator.SetTrigger("StopSprinting");
        }
        //if (Input.GetKeyUp(KeyCode.Space))
        //{
        //    animator.SetTrigger("StopAttack");
        //    //animator.SetBool("Attack", false); 

        //}
        


    }

    private void OnAnimatorIK(int layerIndex)
    {
        if (animator)
        {
            animator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, 1f);
            animator.SetIKRotationWeight(AvatarIKGoal.LeftFoot, 1f);

            RaycastHit hit;
            Ray ray = new Ray(animator.GetIKPosition(AvatarIKGoal.LeftFoot) + Vector3.up, Vector3.down);
            if (Physics.Raycast(ray, out hit, distance + 1f, layerMask))
            {
                if (hit.transform.tag == "CanStepOn")
                {
                    Vector3 footPos = hit.point;

                    footPos.y += distance;
                    animator.SetIKPosition(AvatarIKGoal.LeftFoot, footPos);
                    animator.SetIKRotation(AvatarIKGoal.LeftFoot, Quaternion.LookRotation(transform.forward, hit.normal));

                    Debug.Log("Raycast hit: " + hit.point);
                    Debug.Log("IK Position Set To: " + footPos);
                }
            }

        }
    }
}

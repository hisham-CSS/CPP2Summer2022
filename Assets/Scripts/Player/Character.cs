using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(CharacterController))]
public class Character : MonoBehaviour
{
    CharacterController controller;

    [Header("Player Settings")]
    [Space(10)]
    [Tooltip("Speed value between 1 and 6")]
    [Range(1.0f, 6.0f)]
    public float speed = 6;
    public float gravity = 9.81f;
    public float jumpSpeed = 10.0f;

    Vector3 moveDir;

    [Header("Weapon Settings")]
    [Space(10)]
    public float projectileForce = 10.0f;
    public Rigidbody projectilePrefab;
    public Transform projectileSpawnPoint;


    // Start is called before the first frame update
    void Start()
    {
        try
        {
            controller = GetComponent<CharacterController>();

            controller.minMoveDistance = 0.0f;

            if (speed <= 0.0f)
            {
                speed = 6.0f;
                throw new UnassignedReferenceException("Speed not set on " + name + "defaulting to " + speed.ToString());
            }

            if (jumpSpeed <= 0)
            {
                jumpSpeed = 10.0f;
            }

        }
        catch (NullReferenceException e)
        {
            Debug.Log(e.Message);
        }
        catch (UnassignedReferenceException e)
        {
            Debug.Log(e.Message);
        }
        finally
        {
            Debug.Log("This code always runs");
        }
    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        if (controller.isGrounded)
        {
            moveDir = new Vector3(horizontal, 0.0f, vertical);
            moveDir *= speed;

            moveDir = transform.TransformDirection(moveDir);

            if (Input.GetButtonDown("Jump"))
            {
                moveDir.y = jumpSpeed;
            }

        }

        moveDir.y -= gravity * Time.deltaTime;
        controller.Move(moveDir * Time.deltaTime);

        if (Input.GetButtonDown("Fire1"))
            Fire();
    }

    void Fire()
    {
        if (projectilePrefab && projectileSpawnPoint)
        {
            Rigidbody temp = Instantiate(projectilePrefab, projectileSpawnPoint.position, projectileSpawnPoint.rotation);

            temp.AddForce(projectileSpawnPoint.forward * projectileForce, ForceMode.Impulse);

            Destroy(temp.gameObject, 2.0f);
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {

    }
}

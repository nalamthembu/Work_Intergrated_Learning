using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement_Player : MonoBehaviour
{
    public CharacterController player;
    [SerializeField] float walkSpeed = 500f;
    float horizontal;
    float vertical;

    void Update()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(horizontal, -1f, vertical).normalized;

        if (direction.magnitude >= 0.1f)
        {
            player.Move(direction * walkSpeed * Time.deltaTime);
        }
    }
}

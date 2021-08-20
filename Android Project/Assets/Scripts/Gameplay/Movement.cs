using System;
using System.Collections;
using System.Collections.Generic;
using Audio;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;

public class Movement : MonoBehaviour
{
    public float movementSpeed = 5f;
    private Rigidbody2D _rigidbody2D;
    public FloatingJoystick floatingJoystick;
    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        _rigidbody2D.angularVelocity = 0f;
        Vector3 direction = Vector3.up * floatingJoystick.Vertical + Vector3.right * floatingJoystick.Horizontal;
        if (direction == Vector3.zero)
        {
            AudioManager.instance.StopSound("thruster");
            return;
        }
        _rigidbody2D.AddForce(direction * (movementSpeed * Time.fixedDeltaTime), ForceMode2D.Impulse);
        transform.right = direction;
        AudioManager.instance.PlaySound("thruster");
    }
}
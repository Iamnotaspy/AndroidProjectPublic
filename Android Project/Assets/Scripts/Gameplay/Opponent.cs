using System;
using System.Collections;
using System.Collections.Generic;
using Audio;
using UnityEngine;
using Random = UnityEngine.Random;

public class Opponent : MonoBehaviour
{

    public float maxSpeed = 20f;
    private Rigidbody2D _rigidbody2D;
    private const float MaxDistance = 10f;
    public float minDesireToStayInCenter = 0.5f;
    public float maxDesireToStayInCenter = 3f;
    private float desireToStayInCenter;
    public GameObject closestProjectile;
    public float specialAvoidStrengthMultiplier = 2f;    //Used to avoid the closest projectile more than the others

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        desireToStayInCenter = Random.Range(minDesireToStayInCenter, maxDesireToStayInCenter);
    }

    public void FixedUpdate()
    {
        var distanceToClosestProjectile = 9999f;
        Seek(Vector3.zero, desireToStayInCenter);
        foreach (var projectile in EntityManager.Instance.projectiles)
        {
            if(projectile == null) continue;
            var distanceToProjectile = Vector3.Distance(transform.position, projectile.transform.position);
            if(distanceToProjectile == 0f) continue;

            if (distanceToProjectile < distanceToClosestProjectile)
            {
                distanceToClosestProjectile = distanceToProjectile;
                closestProjectile = projectile;
            }
            
            var avoidStrength = (MaxDistance / distanceToProjectile) - 1f;
            avoidStrength = Mathf.Clamp01(avoidStrength);
            avoidStrength *= Random.Range(0.25f, 1f);
            avoidStrength /= EntityManager.Instance.projectiles.Count + 1;
            Seek(projectile.transform.position, -1f * avoidStrength);
        }

        //Opponent should especially try to avoid closest projectile
        if (closestProjectile != null)
        {
            Seek(closestProjectile.transform.position, -1f * specialAvoidStrengthMultiplier);
        }
        
        _rigidbody2D.angularVelocity = 0f;
    }

    private void Update()
    {
        if(closestProjectile == null) return;
        transform.right = closestProjectile.transform.position.normalized;
    }

    private void Seek(Vector3 target, float speedMultiplier)
    {
        var desired = target - transform.position;
        desired.z = 0f;
        desired.Normalize();
        desired *= maxSpeed * speedMultiplier;
        var steer = (Vector2) desired - _rigidbody2D.velocity;
        steer = Vector3.ClampMagnitude(steer, 50f);
        _rigidbody2D.AddForce(steer * Time.fixedDeltaTime, ForceMode2D.Impulse);
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        if(other.gameObject.CompareTag("Ship")) return;
        //Increase desireToStayInCenter until they stop clinging to the wall
        desireToStayInCenter *= 1.1f;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        //Decrease desireToStayInCenter until they move away from it
        if (other.gameObject.CompareTag("Center"))
        {
            desireToStayInCenter /= 1.1f;
        }
    }
}
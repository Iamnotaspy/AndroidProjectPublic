using System;
using System.Collections;
using System.Collections.Generic;
using Audio;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class Projectile : MonoBehaviour
{
    public enum GravitateBehaviour
    {
        gravitateTowards,
        gravitateAway,
        defaultBehaviour,
    }
    public Vector3 movementDirection;
    public float movementSpeed;
    public bool dangerous;
    private Rigidbody2D _rigidbody2D;
    private SpriteRenderer spriteRenderer;
    public float gravitateStrength;
    private Vector2 gravitatePoint;
    public GravitateBehaviour CurrentGravitateBehaviour;
    private float wallCollisionTimer;
    private float wallCollisionMaxTimer = 0.2f;

    // Start is called before the first frame update
    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        dangerous = false;
        CurrentGravitateBehaviour = GravitateBehaviour.defaultBehaviour;
        spriteRenderer.color = new Color(255f,255f,255f, 0.2f);
    }

    private void Start()
    {
        EntityManager.Instance.projectiles.Add(gameObject);
        EntityManager.Instance._projectiles.Add(this);
    }

    private void FixedUpdate()
    {
        wallCollisionTimer += Time.deltaTime;
        if(EventManager.Instance.gamePaused) return;
        _rigidbody2D.AddForce(movementDirection * (movementSpeed * Time.deltaTime), ForceMode2D.Impulse);
        if (CurrentGravitateBehaviour != GravitateBehaviour.defaultBehaviour && dangerous)
        {
            var direction = CurrentGravitateBehaviour.GetHashCode() * 2 - 1;
            _rigidbody2D.AddForce(((Vector2)transform.position - gravitatePoint) * (direction * gravitateStrength * Time.deltaTime), ForceMode2D.Impulse);
            movementDirection = _rigidbody2D.velocity.normalized;
        }
        _rigidbody2D.angularVelocity = 0f;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("Ship"))
        {
            if(!dangerous) return;
            if (other.gameObject.CompareTag("Player")) EventManager.Instance.PlayerHit();
            if (other.gameObject.CompareTag("Ship"))
            {
                ScoreManager.Instance.IncrementCurrentScore(10);
                EntityManager.OpponentHit();
            }
            Destroy(other.gameObject);
            EntityManager.Instance.projectiles.Remove(gameObject);
            EntityManager.Instance._projectiles.Remove(this);
            Destroy(gameObject);
            return;
        }

        if(wallCollisionTimer>wallCollisionMaxTimer) HitNonShipCollider(other);
    }

    private void HitNonShipCollider(Collision2D other)
    {
        AudioManager.instance.PlaySound("UI");

        spriteRenderer.color = new Color(255f, 255f, 255f, 1f);
        dangerous = true;

        if (other.gameObject.transform.position.x == 0)
        {
            movementDirection.y *= -1f;
        }

        if (other.gameObject.transform.position.y == 0)
        {
            movementDirection.x *= -1f;
        }

        wallCollisionTimer = 0f;
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        if (!other.gameObject.CompareTag("Player") && !other.gameObject.CompareTag("Ship"))
        {
            if(wallCollisionTimer>wallCollisionMaxTimer) HitNonShipCollider(other);
            return;
        }
        if(!dangerous) return;
        if (other.gameObject.CompareTag("Player")) EventManager.Instance.PlayerHit();
        else
        {
            ScoreManager.Instance.IncrementCurrentScore(10);
            EntityManager.OpponentHit();
        }
        Destroy(other.gameObject);
        EntityManager.Instance.projectiles.Remove(gameObject);
        EntityManager.Instance._projectiles.Remove(this);
        Destroy(gameObject);
    }

    public void GravitateTowardsPoint(Vector2 point, bool towardsPoint)
    {
        CurrentGravitateBehaviour = towardsPoint ? GravitateBehaviour.gravitateTowards : GravitateBehaviour.gravitateAway;
        gravitatePoint = point;
        AudioManager.instance.PlaySound("fireProjectile");
        StartCoroutine(ClearGravitatePoint());
    }

    IEnumerator ClearGravitatePoint()
    {
        yield return new WaitForSeconds(1f);
        CurrentGravitateBehaviour = GravitateBehaviour.defaultBehaviour;
    }
}
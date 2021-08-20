using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gravity : MonoBehaviour
{
    private float currentTimer;
    private float maxTimer = 8f;
    public SpriteRenderer spriteRenderer;
    private bool gravitateTowards;
    private Color abilityTriggeredColor;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        gravitateTowards = true;
        abilityTriggeredColor = Color.blue;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        currentTimer += Time.deltaTime;
        if (currentTimer > maxTimer)
        {
            TriggerGravity();
            currentTimer = 0;
            abilityTriggeredColor = gravitateTowards ? Color.blue : Color.red;
        }

        spriteRenderer.color = Color.Lerp(Color.white, abilityTriggeredColor, currentTimer / maxTimer);
    }

    private void TriggerGravity()
    {
        foreach (var projectile in EntityManager.Instance._projectiles)
        {
            projectile.GravitateTowardsPoint(transform.position, gravitateTowards);
        }
        gravitateTowards = !gravitateTowards;
    }
}

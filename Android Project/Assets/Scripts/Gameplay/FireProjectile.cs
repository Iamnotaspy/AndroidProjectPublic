using System.Collections;
using System.Collections.Generic;
using Audio;
using UnityEngine;

public class FireProjectile : MonoBehaviour
{
    public GameObject projectile;
    public bool isOpponent = false;
    private float timer;
    // Start is called before the first frame update
    private void Start()
    {
        timer = 0f;
    }

    // Update is called once per frame
    private void Update()
    {
        timer += Time.deltaTime;
        if (!(timer > 5f)) return;
        timer = 0f;
        var newProjectile = Instantiate(projectile, transform.position + (transform.right * 0.1f), Quaternion.identity);
        newProjectile.GetComponent<Projectile>().movementDirection = transform.right;
        AudioManager.instance.PlaySound("fireProjectile");
        if(!isOpponent) ScoreManager.Instance.IncrementCurrentScore(1);
    }
}

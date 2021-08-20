using System;
using System.Collections;
using System.Collections.Generic;
using Audio;
using UnityEngine;
using UnityEngine.Serialization;

public class EntityManager : MonoBehaviour
{
    #region Singleton
    public static EntityManager Instance;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.Log("EntityManager already exists");
            Destroy(this);
        }
        Instance = this;
    }
    

    #endregion

    private void Start()
    {
        AudioManager.instance.gameObject.GetComponent<AudioSource>().Stop();
        AudioManager.instance.gameObject.GetComponent<AudioSource>().Play();
    }

    public List<GameObject> projectiles;
    public List<Projectile> _projectiles;

    public static void OpponentHit()
    {
        AudioManager.instance.PlaySound("explosion");
        Debug.Log("Playing explosion");
    }
}

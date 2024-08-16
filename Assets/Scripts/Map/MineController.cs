using System;
using System.Collections;
using UnityEngine;

public class MineController : MonoBehaviour
{
    public float exploseDamage = 100f;
    public float lightOnRange;
    public Light pointLight;
    public Player player;
    public StatusController stc;
    public AudioSource audioSource;
    
    void Start()
    {
        player = FindObjectOfType<Player>();
        stc = FindObjectOfType<StatusController>();
    }

    
    void Update()
    {
        
        if (Vector3.Distance(transform.position, player.transform.position) < lightOnRange)
        {
            pointLight.intensity = 1f;
            audioSource.Play();
        }
        else
        {
            pointLight.intensity = 0f;
            audioSource.Stop();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(Explose());
            
        }
    }

    IEnumerator Explose()
    {
        yield return new WaitForSeconds(2);
        stc.playerHp -= exploseDamage;
    }
}

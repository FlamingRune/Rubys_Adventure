using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthCollectible : MonoBehaviour
{
     public AudioClip collectedClip;
     public ParticleSystem healthEffect;

     void Start()
     {
        healthEffect.Stop();
     }
   void OnTriggerEnter2D(Collider2D other)
   {
    RubyController controller = other.GetComponent<RubyController>();
        
    if (controller != null)
        {
        if(controller.health < controller.maxHealth)
            {
                 healthEffect.Play();
                 controller.ChangeHealth(1);
                 Destroy(gameObject);
                 controller.healthEffect.Play();
                 controller.PlaySound(collectedClip);
            }
        }
    }
}

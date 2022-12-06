using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHardestController : MonoBehaviour
{
    public float speed;
    public bool vertical;
    public float changeTime = 3.0f;

    Rigidbody2D rigidbody2D;
    float timer;
    int direction = 1;
    bool broken = true;
    public ParticleSystem smokeEffect;
    private RubyController rubyController;

    public int maxHealth = 2;
    public int health { get { return currentHealth; } }
    public int currentHealth;
    
    // Start is called before the first frame update
    Animator animator;

    void Start()
    {
     rigidbody2D = GetComponent<Rigidbody2D>();
     timer = changeTime;
     animator = GetComponent<Animator>();
     GameObject rubyControllerObject = GameObject.FindWithTag("RubyController");
       if (rubyControllerObject != null)
        {
            rubyController = rubyControllerObject.GetComponent<RubyController>();
            //print("Found the RubyConroller Script!");
            
        }
        if (rubyController == null)
        {
            print("Cannot find GameController Script!");
        }
    }

    void Update()
    {
        timer -= Time.deltaTime;

        if (timer < 0)
        {
            direction = -direction;
            timer = changeTime;
        }
        
	//remember ! inverse the test, so if broken is true !broken will be false and return won’t be executed.
    if(!broken)
        {
        return;
        }
    }
    
    void FixedUpdate()
    {
        Vector2 position = rigidbody2D.position;
        
        if (vertical)
        {
            animator.SetFloat("Move X", 0);
            animator.SetFloat("Move Y", direction);
            position.y = position.y + Time.deltaTime * speed * direction;;
        }
        else
        {
            animator.SetFloat("Move X", direction);
            animator.SetFloat("Move Y", 0);
            position.x = position.x + Time.deltaTime * speed * direction;;
        }
        
        rigidbody2D.MovePosition(position);
        
        if(!broken)
        {
            return;
        }
    }
    void OnCollisionEnter2D(Collision2D other)
    {
    RubyController player = other.gameObject.GetComponent<RubyController>();

        if (player != null)
        {
        player.ChangeHealth(-2);
        }
    }
    public void Fix()
    {
    broken = false;
    rigidbody2D.simulated = false;
    animator.SetTrigger("Fixed");
    smokeEffect.Stop();

    rubyController.ChangeScore(1);
    }
    
    public void ChangeHealth(int amount)
    {
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);

        if (currentHealth == 2)
        {
            Fix();
        }
    }    
}


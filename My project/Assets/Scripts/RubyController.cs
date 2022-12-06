using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class RubyController : MonoBehaviour
{
    public float speed = 3.0f;

    public int cogs = 4;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI cogsText;
    
    
    
    public int maxHealth = 5;

    public int score = 0;
    public GameObject winText;
    public GameObject loseText;
    public GameObject jambiText;
    
    public GameObject projectilePrefab;
    
    public AudioClip throwSound;
    public AudioClip hitSound;
    public AudioClip winSound;
    public AudioClip loseSound;
    public AudioClip pickupSound;
    public AudioClip speedSound;


    
    public int health { get { return currentHealth; }}
    int currentHealth;
    
    bool gameOver;

    public float timeInvincible = 2.0f;
    bool isInvincible;
    float invincibleTimer;
    
    Rigidbody2D rigidbody2d;
    float horizontal;
    float vertical;

    public ParticleSystem hitEffect;
    public ParticleSystem healthEffect;
    
    Animator animator;
    Vector2 lookDirection = new Vector2(1,0);
    
    AudioSource audioSource;
    
    // Start is called before the first frame update
    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        
        
        animator = GetComponent<Animator>();

        currentHealth = maxHealth;

        audioSource = GetComponent<AudioSource>();
        hitEffect.Stop();
        healthEffect.Stop();

        winText.SetActive(false);

        loseText.SetActive(false);
        
        jambiText.SetActive(false);

        gameOver = false;

        //rigidbody2d = GetComponent<Rigidbody2D>();
        //score = 0;
       // Score();
    }

    // Update is called once per frame
    void Update()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
        
        Vector2 move = new Vector2(horizontal, vertical);
        
        if(!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f))
        {
            lookDirection.Set(move.x, move.y);
            lookDirection.Normalize();
        }
        
        animator.SetFloat("Look X", lookDirection.x);
        animator.SetFloat("Look Y", lookDirection.y);
        animator.SetFloat("Speed", move.magnitude);
        
        if (isInvincible)
        {
            invincibleTimer -= Time.deltaTime;
            if (invincibleTimer < 0)
                isInvincible = false;
        }
        
        if(Input.GetKeyDown(KeyCode.C))
        {
            Launch();
        }
        
        if (Input.GetKeyDown(KeyCode.X))
        {
            RaycastHit2D hit = Physics2D.Raycast(rigidbody2d.position + Vector2.up * 0.2f, lookDirection, 1.5f, LayerMask.GetMask("NPC"));
            if (hit.collider != null)
            {
                NonPlayerCharacter character = hit.collider.GetComponent<NonPlayerCharacter>();
                 if (score == 4 && character.gameObject.CompareTag("Jambi"))
                {
                    SceneManager.LoadScene("Scene2");
                } 
            }
        }
         if (Input.GetKey(KeyCode.R))
        {
            if (gameOver == true)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }  
        cogsText.text = "Cogs: " + cogs.ToString();
        scoreText.text = "Score: " + score.ToString();
        if (score == 10)
        {
            winText.SetActive(true);
            speed = 0f;
            gameOver = true;
            

           GameObject bgm = GameObject.Find("BackgroundMusic");
            bgm.SetActive(false);
            PlaySound(winSound);
        }
    }
    
    void FixedUpdate()
    {
        Vector2 position = rigidbody2d.position;
        position.x = position.x + speed * horizontal * Time.deltaTime;
        position.y = position.y + speed * vertical * Time.deltaTime;

        rigidbody2d.MovePosition(position);
    }

    public void ChangeHealth(int amount)
    {
        if (amount < 0)
        {
            animator.SetTrigger("Hit");
            if (isInvincible)
                return;
            
            isInvincible = true;
            invincibleTimer = timeInvincible;
            
            PlaySound(hitSound);
            hitEffect.Play();
        }
    
        
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        
        UIHealthBar.instance.SetValue(currentHealth / (float)maxHealth); 
        if (currentHealth == 0)
        {
            loseText.gameObject.SetActive(true);
            speed = 0f;
            gameOver = true;
            GameObject bgm = GameObject.Find("BackgroundMusic");
            bgm.SetActive(false);
            PlaySound(loseSound);
        }
    }
    
    void Launch()
    {
        if (cogs >= 1)
        {
            GameObject projectileObject = Instantiate(projectilePrefab, rigidbody2d.position + Vector2.up * 0.5f, Quaternion.identity);

        Projectile projectile = projectileObject.GetComponent<Projectile>();
        projectile.Launch(lookDirection, 300);

        animator.SetTrigger("Launch");

        cogs = cogs - 1;

        cogsText.text = cogs.ToString();

        
        PlaySound(throwSound);
        }
    } 
    
    public void PlaySound(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }

    
       
    
     private void OnCollisionEnter2D(Collision2D collision)
    {
       if (collision.collider.tag == "Cog")
        {
            cogs = cogs + 4;

            PlaySound(pickupSound);
            
            Destroy(collision.collider.gameObject);
        }
        if (collision.collider.tag == "Speed")
        {
            speed = 5.0f;

            PlaySound(speedSound);

            Destroy(collision.collider.gameObject);
        }
    }
    //void Win()
    //{
      
        
        
        //
       
    //}
    public void ChangeScore(int scoreAmount)
    {
        score = score + 1;
        scoreText.text = score.ToString();

        //talk to Jambi
        if (score == 4)
        {
            jambiText.gameObject.SetActive(true);
        }
    }
  
 
        
}
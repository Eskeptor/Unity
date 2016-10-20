using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Player_Health : MonoBehaviour {
    public int StartingHealth = 100;
    public int CurrentHealth;
    public Slider HealthSlider;
    public Image DamageImage;
    public float FlashSpeed = 5f;
    public Color FlashColor = new Color(1f, 0f, 0f, 0.1f);

    Animator anim;
    Player_Move playerMove;
    bool isDead;
    bool damaged;

    void Awake()
    {
        anim = GetComponent<Animator>();
        playerMove = GetComponent<Player_Move>();
        CurrentHealth = StartingHealth;
    }

    void Update()
    {
        if (damaged)
        {
            DamageImage.color = FlashColor;
        }
        else
        {
            DamageImage.color = Color.Lerp(DamageImage.color, Color.clear, FlashSpeed * Time.deltaTime);
        }
        damaged = false;
    }

    public void TakeDamage(int amount)
    {
        damaged = true;
        CurrentHealth -= amount;
        HealthSlider.value = CurrentHealth;
        if (CurrentHealth <= 0 && !isDead)
        {
            Death();
        }
    }

    void Death()
    {
        isDead = true;
        //anim.SetTrigger("Die");
        playerMove.enabled = false;
    }
}

using UnityEngine;
using System.Collections;

public class Enemy_Health : MonoBehaviour {
    public int StartingHealth = 100;
    public int CurrentHealth;
    public float SinkSpeed = 2.5f; //죽은후에 여분의 시간
    public int ScoreValue = 10;

    ParticleSystem hitParticles;
    SphereCollider sphere;
    bool isDead;
    bool isSinking;

    void Awake()
    {
        hitParticles = GetComponentInChildren<ParticleSystem>();
        sphere = GetComponent<SphereCollider>();
        CurrentHealth = StartingHealth;
    }
	
	void Update ()
    {
        if (isSinking)
        {
            transform.Translate(-Vector3.up * SinkSpeed * Time.deltaTime); //가라앉게 만듬
        }
	}

    public void TakeDamage(int amount,Vector3 hitPoint)
    {
        if (isDead)
            return;
        CurrentHealth -= amount;
        hitParticles.transform.position = hitPoint;
        hitParticles.Stop();
        hitParticles.Play();
        if (CurrentHealth <= 0)
        {
            Death();
        }
    }

    void Death()
    {
        isDead = true;
        sphere.isTrigger = true;
        StartSinking();
    }
    public void StartSinking()
    {
        GetComponent<NavMeshAgent>().enabled = false;
        //.enabled는 컴포넌트를 대상으로, .setActive는 게임 오브젝트를 대상으로
        GetComponent<Rigidbody>().isKinematic = true;
        isSinking = true;
        Destroy(gameObject, 2f);
    }
}

using UnityEngine;
using System.Collections;

public class Player_Shooting : MonoBehaviour {
    public int Damage = 20;
    public float TimeBetweenBullets = 0.15f;
    public float Range = 100f; //사거리

    float timer;
    Ray shootRay;
    RaycastHit shootHit;
    int shootableMask;
    ParticleSystem gunParticles;
    LineRenderer gunLine;
    Light gunLight;
    float effectsDisplayTime = 0.2f;

    void Awake()
    {
        shootableMask = LayerMask.GetMask("Shootable");
        gunParticles = GetComponent<ParticleSystem>();
        gunLine = GetComponent<LineRenderer>();
        gunLight = GetComponent<Light>();
    }

    void Update ()
    {
        timer += Time.deltaTime;
        if (Input.GetMouseButton(0) && timer >= TimeBetweenBullets)
        {
            Shoot();
        }
        if (timer >= TimeBetweenBullets * effectsDisplayTime)
        {
            DisableEffects();
        }
	}
    
    public void DisableEffects()
    {
        gunLine.enabled = false;
        gunLight.enabled = false;
    }

    void Shoot()
    {
        timer = 0f;
        gunLight.enabled = true;
        gunParticles.Stop();
        gunParticles.Play();

        gunLine.enabled = true;
        gunLine.SetPosition(0, transform.position);
        //shootRay.origin은 광선이 시작되는 점
        shootRay.origin = transform.position;
        shootRay.direction = transform.forward;

        if(Physics.Raycast(shootRay,out shootHit, Range, shootableMask))
        {
            Enemy_Health enemyHealth = shootHit.collider.GetComponent<Enemy_Health>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(Damage, shootHit.point);
            }
            gunLine.SetPosition(1, shootHit.point);
            // ray가 맞춘 곳에서 광선이 멈춤
        }
        else
        {
            //적을 맞추지 않으면 그저 선만 긋는다.
            gunLine.SetPosition(1, shootRay.origin + shootRay.direction * Range);
        }
        
    }
}

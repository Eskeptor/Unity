using UnityEngine;
using System.Collections;

public class Missile_Fire : MonoBehaviour {
    public float MissileSpeed = 20f;

    void Update () {
        // 미사일이 생성되자마자 MissileSpeed 속도로 날라감
        this.transform.Translate(new Vector3(0, 0, 1) * MissileSpeed * Time.deltaTime);
    }

    /* 미사일 부딛힘 감지 */
    void OnCollisionEnter(Collision col)
    {
        // 부딛힌 상대의 태그가 "Enemy"일때
        if (col.collider.tag == "Enemy")
        {
            // 미사일의 콜라이더를 false처리 한다.
            // (false처리가 됨으로 "Attack_Missile.cs"에서 메모리로 다시 불러들임
            this.GetComponent<Collider>().enabled = false;
            Debug.Log("적과 부딛힘");
        }
    }
}

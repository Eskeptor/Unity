using UnityEngine;
using System.Collections;

public class GameEvent_TargetDamage : MonoBehaviour {


    public Transform FireMissileRightLocation;
    public Transform FireMissileLeftLocation;
    RaycastHit hit;

    public IEnumerator MissileAttack()
    {
        Physics.Raycast(FireMissileLeftLocation.position, FireMissileLeftLocation.forward, out hit, 20f);
        Physics.Raycast(FireMissileRightLocation.position, FireMissileRightLocation.forward, out hit, 20f);
        if (hit.collider)
        {
            if(hit.collider.tag == "Enemy")
            {
                Debug.Log("에너미 태그 발견");
               // Destroy(hit.collider.gameObject);

            }
            Debug.Log("hit.collider");
        }
        Debug.Log("코루틴 실행");
        yield return null;
    }

}

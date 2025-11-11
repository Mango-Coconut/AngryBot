using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.UIElements;

public class Damage : MonoBehaviour
{
    Renderer[] renderers;
    int maxHP = 100;
    public int curHP;
    Animator animator;
    CharacterController cc;
    readonly int hashDie = Animator.StringToHash("Die");
    readonly int hashRespawn = Animator.StringToHash("Respawn");
    void Awake()
    {
        curHP = maxHP;
        renderers = GetComponentsInChildren<Renderer>();
        animator = GetComponent<Animator>();
        cc = GetComponent<CharacterController>();
    }
    void OnCollisionEnter(Collision collision)
    {
        if (curHP > 0 && collision.collider.CompareTag("BULLET"))
        {
            curHP -= 10;
            if (curHP <= 0)
            {
                StartCoroutine(PlayerDie());
            }   
        }
    }

    IEnumerator PlayerDie()
    {
        cc.enabled = false;
        animator.SetBool(hashRespawn, false);
        animator.SetTrigger(hashDie);
        yield return new WaitForSeconds(3);

        animator.SetBool(hashRespawn, true);
        SetPlayerVisible(false);
        yield return new WaitForSeconds(1.5f);
        Transform[] points = GameObject.Find("SpawnPointGroup").GetComponentsInChildren<Transform>();
        int idx = Random.Range(1, points.Length);
        transform.position = points[idx].position;
        curHP = maxHP;
        SetPlayerVisible(true);
        cc.enabled = true;

    }
    void SetPlayerVisible(bool isVisible)
    {
        for(int i=0; i<renderers.Length; i++)
        {
            renderers[i].enabled = isVisible;
        }
    }
}

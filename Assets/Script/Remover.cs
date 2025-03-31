using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Remover : MonoBehaviour
{
    public int Health = 100;
    public float Timer = 1.0f;
    public int AttackPoint = 50;
    // �Q\�� �������� ������Ʈ �Ǳ� �� �� �� �����Ѵ�.
    void Start()
    {
        
        Health += 100;
        
    }

    //���� ���� ���� �� ������ ���� ȣ���
    void Update()
    {
        CharactorHealthUp();
        Timer -= Time.deltaTime;

        if(Timer <= 0)
        {
            Timer = 1;
            Health += 10;
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Health -= AttackPoint;
        }
        CheckDeath();
    }

    public void CharactorHit(int Damage)
    {
        Health -= Damage;
    }
    void CheckDeath()
    {
        if (Health <= 0)
        {
            Destroy(gameObject);
        }
    }

    void CharactorHealthUp()
    {
        Timer -= Time.deltaTime;

        if(Timer <= 0)
        {
            Timer = 1;
            Health += 10;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordAttack : MonoBehaviour
{
    public float damage = 3;
    public Collider2D swordCollider;
    Vector2 rightAttackOffset;

    // Start is called before the first frame update
    void Start()
    {
        rightAttackOffset = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AttackRight(){
        print("Attack Right");
        swordCollider.enabled = true;
        transform.localPosition = rightAttackOffset;
    }
    
    public void AttackLeft(){
        print("Attack Left");
        swordCollider.enabled = true;
        transform.localPosition = new Vector3(rightAttackOffset.x * -1, rightAttackOffset.y);
    }

    public void StopAttack(){
        swordCollider.enabled = false;
    }

    public void OnTriggerEnter2D(Collider2D other){
        if(other.tag != "Enemy")
            return;
        
        Enemy enemy = other.GetComponent<Enemy>();

        if(enemy != null)
            enemy.Health -= damage;
    }
}

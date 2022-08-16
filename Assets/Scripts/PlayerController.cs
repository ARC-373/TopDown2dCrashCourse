using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 1f;
    public float collisionOffset = 0.05f;
    public ContactFilter2D movementFilter;
    public SwordAttack swordAttack;

    Vector2 movementInput;
    SpriteRenderer spriteRenderer;
    Rigidbody2D rb;
    Animator animator;
    List<RaycastHit2D> castCollisions = new List<RaycastHit2D>();

    bool canMove = true;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        PlayerMovement();
    }

    void OnMove(InputValue movementValue)
    {
        movementInput = movementValue.Get<Vector2>();
    }

    void OnFire(){
        animator.SetTrigger("swordAttack");
    }

    void PlayerMovement(){
        if(movementInput == Vector2.zero){
            animator.SetBool("isMoving", false);
            return;
        }

        List<Vector2> directions = new List<Vector2>{
            movementInput, 
            new Vector2(movementInput.x, 0), 
            new Vector2(0, movementInput.y)
        };
        
        foreach(Vector2 direction in directions){
            if(!CanMove(direction))
                continue;
            
            if(canMove){
                animator.SetBool("isMoving", true);
                Move(direction);
            }

            if(direction.x < 0)
                spriteRenderer.flipX = true;
            else if(direction.x > 0)
                spriteRenderer.flipX = false;

            break;
        }
    }

    bool CanMove(Vector2 direction){
        int count = rb.Cast(
            direction,
            movementFilter,
            castCollisions,
            moveSpeed * Time.fixedDeltaTime + collisionOffset
        );

        return count == 0;
    }

    void Move(Vector2 direction){
        rb.MovePosition(rb.position + (direction * moveSpeed * Time.fixedDeltaTime));
    }

    public void SwordAttack(){
        LockMove();

        if(spriteRenderer.flipX)
            swordAttack.AttackLeft();
        else
            swordAttack.AttackRight();
    }

    public void EndSwordAttack(){
        UnlockMove();

        swordAttack.StopAttack();
    }

    void LockMove(){
        canMove = false;
    }

    void UnlockMove(){
        canMove = true;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour{

    //This is the code for the Player animation on walking.

    private const string IS_WALKING = "IsWalking";

    [SerializeField] private Player player;

    private Animator animator;

    private void Awake() {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update(){
        
        animator.SetBool(IS_WALKING, player.IsWalking());

    }
}

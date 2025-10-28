using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpscareObject : MonoBehaviour
{
    [SerializeField] AudioSource jumpscareAudio;
    [SerializeField] float scareDuration = 1.5f;
    [SerializeField] Animator jumpscareAnimator;
    void Start()
    {
        gameObject.SetActive(false);
    }
    void OnEnable()
    {
        if (jumpscareAudio == null)
        {
            Debug.LogWarning("Jumpscare Audio is Null!");
            return;
        }

        StartCoroutine(PlayJumpscare());

    }
    
    private IEnumerator PlayJumpscare()
    {
        try
        {
            jumpscareAnimator.SetTrigger("isJumpscaring");
        } catch (Exception e)
        {
            Debug.LogError("isJumpscaring bool in Animator not found: " + e.Message);
        }

        jumpscareAudio.Play();

        yield return new WaitForSeconds(scareDuration);
    }
}

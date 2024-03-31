using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SticmanMovemet : MonoBehaviour
{
    [SerializeField]
    private GameEngine gameEngine;
    [SerializeField]
    private List<Rigidbody> ragdoolRB;
    [SerializeField]
    private List <Collider> colliderRB;

    [SerializeField]
    private PlayerMovement playerMovement;

    [SerializeField]
    private AudioSource jumpSound;
    [SerializeField]
    private AudioSource deathSound;
    [SerializeField]
    private AudioSource yyySound;
    [SerializeField]
    private AudioSource boySound;

    Animator animator;
    private bool isReadyToMove = true;
    private int countJump = 0;

    private bool isStoped = false;

    private void Start()
    {
        animator = GetComponent<Animator>();
        IsRigidbodyKinematic(true);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isStoped)
        {
            if (other.CompareTag("Floor") || other.CompareTag("WallBox"))
            {
                isReadyToMove = false;
                isStoped = !isStoped;
                StopMove();
            }
        }
    }

    public void MoveBox(float verticalOffset, float horizontalOffset)
    {
        if (isReadyToMove)
            transform.position = new Vector3(horizontalOffset, transform.position.y, transform.position.z + verticalOffset);
    }

    private void StopMove()
    {
        animator.StopPlayback();
        animator.enabled = false;
        playerMovement.isGameStarted = false;
        IsRigidbodyKinematic(false);
        if (gameEngine.isSound)
        {
            if (gameEngine.isGenreActivated && gameEngine.isGenre)
            {
                boySound.Play();
            }
            else
            {
                deathSound.Play();
            }
        }
        StartCoroutine(LoadNextScene());
    }

    private IEnumerator LoadNextScene()
    {
        yield return new WaitForSeconds(3f);
        GameObject.Find("Main Camera").GetComponent<GameEngine>().ReloadLevel(countJump);
    }

    public void Jump()
    {
        Time.timeScale += 0.02f;
        animator.SetTrigger("Jump");
        countJump++;
        if (gameEngine.isSound)
        {
            if (gameEngine.isGenreActivated && gameEngine.isGenre)
            {
                yyySound.Play();
            }
            else
            {
                jumpSound.Play();
            }
        }
        gameEngine.newBoxValue(countJump);
    }

    private void IsRigidbodyKinematic(bool isKinematic)
    {
        ragdoolRB.ForEach(rb => rb.isKinematic = isKinematic);
        colliderRB.ForEach(cl => cl.enabled = !isKinematic);

        transform.GetComponent<BoxCollider>().enabled = isKinematic;
    }
}

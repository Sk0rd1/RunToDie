using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupBox : MonoBehaviour
{
    public bool isReadyToMove = true;

    [SerializeField]
    private AudioSource punchSound;
    [SerializeField]
    private AudioSource aaaSound;

    public void MoveBox(float verticalOffset, float horizontalOffset)
    {
        if (isReadyToMove)
            transform.position = new Vector3(horizontalOffset, transform.position.y, transform.position.z + verticalOffset);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("DefaultBox"))
        {
            StartCoroutine(CreateBox(other));
            other.gameObject.GetComponent<DefaultBox>().DestoyThis();
        }

        if (other.CompareTag("WallBox"))
        {
            StartCoroutine(DestroyThis());
        }
    }

    private IEnumerator CreateBox(Collider other)
    {
        yield return new WaitForEndOfFrame();
        GetComponentInParent<PlayerMovement>().AddBox();
    }

    private IEnumerator DestroyThis()
    {
        isReadyToMove = false;
        GameEngine gameEngine = GameObject.Find("Main Camera").GetComponent<GameEngine>();
        PlayerMovement playerMovement = GameObject.Find("PlayerGroup").GetComponent<PlayerMovement>();
        if (gameEngine.isSound)
        {
            if (gameEngine.isGenreActivated && gameEngine.isGenre && playerMovement.isGameStarted)
            {
                aaaSound.Play();
            }
            else
            {
                punchSound.Play();
            }
        }
        yield return new WaitForSeconds(10f);
        Destroy(gameObject);
    }
}

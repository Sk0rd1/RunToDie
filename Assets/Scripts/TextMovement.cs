using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextMovement : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        transform.position += new Vector3(0, 0.5f, -0.5f);
        StartCoroutine(Move());
    }

    private IEnumerator Move()
    {
        for(float i = 0; i < 3f; i += Time.deltaTime)
        {
            transform.position += new Vector3(0, 2f * Time.deltaTime, -5f * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
        Destroy(gameObject);
    }

}

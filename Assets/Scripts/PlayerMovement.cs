using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private float positionCorrection = 0.1f;
    [SerializeField]
    private float fingerPercentOffset = 20f;
    [SerializeField]
    private float verticalSpeed = 10f;
    [SerializeField]
    private List<PickupBox> cubes = new List<PickupBox>();
    [SerializeField]
    private SticmanMovemet stickman;
    [SerializeField]
    private GameObject trailEffect;

    private GameObject pickupPrefab;
    private GameObject followPrefab;
    private GameObject collectPrefab;
    /*private float[] segments = { -2, -1.8f, -1.6f, -1.4f, -1.2f,
                                 -1, -0.8f, -0.6f, -0.4f, -0.2f,
                                 0, 0.2f, 0.4f, 0.6f, 0.8f,
                                 1, 1.2f, 1.4f, 1.6f, 1.8f, 2.0f};*/

    private readonly int HORIZONTALSIZE = Screen.width;

    public bool isGameStarted { get; set; } = false;

    private float fingerWidthCenter;
    private float fingerWidthPosition;
    private int widthOffset;
    private float verticalOffset;
    private float horizontalPosition;

    void Start()
    {
        LevelGenerator.CreateFirstObstacles();
        fingerWidthCenter = HORIZONTALSIZE / 2f;
        fingerWidthPosition = fingerWidthCenter;
        pickupPrefab = Resources.Load("PickUp") as GameObject;
        followPrefab = Resources.Load("FollowPlane") as GameObject;
        collectPrefab = Resources.Load("CollectCubeText") as GameObject;
        verticalOffset = verticalSpeed;
        horizontalPosition = 0;
        widthOffset = (int)(HORIZONTALSIZE * fingerPercentOffset / 100f);
    }

    void Update()
    {
        Touch touch;
        if (Input.touchCount > 0 && isGameStarted)
        {
            touch = Input.GetTouch(0);

            isGameStarted = true;
            fingerWidthPosition = touch.position.x;

            if (touch.phase == TouchPhase.Moved)
                fingerWidthPosition = touch.position.x;
        }
        Debug.Log(fingerWidthPosition);
        MoveHorizontal(fingerWidthPosition);
    }

    private void FixedUpdate()
    {
        if (isGameStarted)
        {
            foreach (PickupBox c in cubes)
            {
                if (cubes != null)
                    c.MoveBox(verticalOffset, horizontalPosition);
                else
                    cubes.Remove(c);
            }

            stickman.MoveBox(verticalOffset, horizontalPosition);

            trailEffect.transform.position = new Vector3(stickman.transform.position.x, 2.05f, stickman.transform.position.z);
        }
    }

    private void MoveHorizontal(float fingerWidthPosition)
    {
        float singleValue = 0f;
        if (fingerWidthPosition <= widthOffset)
            singleValue = -1f;
        else if (fingerWidthPosition >= HORIZONTALSIZE - widthOffset)
            singleValue = 1f;
        else if (fingerWidthPosition < fingerWidthCenter)
            singleValue = -1 + (fingerWidthPosition - widthOffset) / (fingerWidthCenter - widthOffset);
        else if(fingerWidthPosition > fingerWidthCenter)
            singleValue = (fingerWidthPosition - fingerWidthCenter) / (fingerWidthCenter - widthOffset);
        else 
            singleValue = 0f;

        singleValue *= 2f;
        Debug.Log(singleValue);
        horizontalPosition = singleValue;
    }

    public void AddBox()
    {
        for(int i = 0; i < cubes.Count; i++)
        {
            if (cubes[i] == null)
            {
                cubes.Remove(cubes[i]);
                i--;
            }
        }

        GameObject cube = Instantiate(pickupPrefab, transform);
        GameObject collect = Instantiate(collectPrefab, stickman.transform);

        cube.transform.position = stickman.transform.position;
        stickman.transform.position += new Vector3(0, 1, 0);
        stickman.Jump();

        cubes.Add(cube.GetComponent<PickupBox>());
    }
}

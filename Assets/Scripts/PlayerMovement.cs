using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private float speed = 5f;
    public GameObject player;

    Vector3 curPos, lastPos;


    private void Start()
    {
        transform.position = GameManager.instance.nextHeroPosition;
    }

    void FixedUpdate()
    {

        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.position += Vector3.right * speed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.position += Vector3.left * speed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            transform.position += Vector3.up * speed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            transform.position += Vector3.down * speed * Time.deltaTime;
        }

        curPos = transform.position;
        if (curPos == lastPos)
        {
            GameManager.instance.isWalking = false;
        }
        else
        {
            GameManager.instance.isWalking = true;
        }
        lastPos = curPos;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "EnterTown")
        {
            CollisionHandler col = other.gameObject.GetComponent<CollisionHandler>();
            GameManager.instance.nextHeroPosition = col.spawnPoint.transform.position;
            GameManager.instance.sceneToLoad = col.sceneToLoad.name;
            GameManager.instance.LoadNextScene();
        }

        if (other.tag == "LeaveTown")
        {
            CollisionHandler col = other.gameObject.GetComponent<CollisionHandler>();
            GameManager.instance.nextHeroPosition = col.spawnPoint.transform.position;
            GameManager.instance.sceneToLoad = col.sceneToLoad.name;
            GameManager.instance.LoadNextScene();
        }

        if (other.tag == "Region1")
        {
            GameManager.instance.curRegions = 0;
        }

        if (other.tag == "Region2")
        {
            GameManager.instance.curRegions = 1;
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Region1" || other.tag == "Region2")
        {
            GameManager.instance.canGetEncounter = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Region1" || other.tag == "Region2")
        {
            GameManager.instance.canGetEncounter = false;
        }
    }
}

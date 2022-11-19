using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enlightment : MonoBehaviour
{
    public bool isActiveInLight;
    [SerializeField]
    private int inLightCount;
    [SerializeField]
    private bool isActivated;
    private bool wasActivated;
    private Collider2D collider2d;
    private GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        collider2d = gameObject.GetComponent<Collider2D>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (isActivated != wasActivated)
        {
            Physics2D.IgnoreCollision(player.GetComponent<BoxCollider2D>(), collider2d, !isActivated);
            Physics2D.IgnoreCollision(player.GetComponent<CircleCollider2D>(), collider2d, !isActivated);
        }

    }

    private void FixedUpdate()
    {
        wasActivated = isActivated;
        isActivated = (isActiveInLight && (inLightCount > 0)) || (!isActiveInLight && (inLightCount <= 0));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Light"))
        {
            inLightCount++;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Light"))
        {
            inLightCount--;
        }

    }
}

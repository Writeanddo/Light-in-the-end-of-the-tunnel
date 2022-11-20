using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.Utilities;

public class Enlightment : MonoBehaviour
{
    public bool isActiveInLight;
    [SerializeField]
    private bool isSolid;
    private bool wasSolid;
    private Collider2D collider2d;
    private GameObject player;
    [ShowInInspector, ListDrawerSettings(IsReadOnly = true)]
    private List<LightController> lighters = new List<LightController>();


    // Start is called before the first frame update
    void Start()
    {
        collider2d = gameObject.GetComponent<Collider2D>();
        player = GameObject.FindGameObjectWithTag("Player");

        Physics2D.IgnoreCollision(player.GetComponent<BoxCollider2D>(), collider2d, !isSolid);
        Physics2D.IgnoreCollision(player.GetComponent<CircleCollider2D>(), collider2d, !isSolid);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        wasSolid = isSolid;
        isSolid = (isActiveInLight && (lighters.Count > 0)) || (!isActiveInLight && (lighters.Count <= 0));
        if (isSolid != wasSolid)
        {
            Physics2D.IgnoreCollision(player.GetComponent<BoxCollider2D>(), collider2d, !isSolid);
            Physics2D.IgnoreCollision(player.GetComponent<CircleCollider2D>(), collider2d, !isSolid);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Light"))
        {
            if (!lighters.Contains(collision.GetComponent<LightController>()))
            {
                lighters.Add(collision.GetComponent<LightController>());
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Light"))
        {
            lighters.Remove(collision.GetComponent<LightController>());
        }

    }
}

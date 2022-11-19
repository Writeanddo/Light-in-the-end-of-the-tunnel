using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class Looter : MonoBehaviour
{
    [ShowInInspector]
    Transform lootedLight;
    [ShowInInspector]
    public bool isHolding;
    [ShowInInspector]
    Transform interactableTrigger;
    [ShowInInspector]
    public bool isAtSwitch;
    [SerializeField]
    GameObject helpText;

    // Start is called before the first frame update
    void Start()
    {
        lootedLight = null;
        isHolding = false;
        helpText.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void LootOrDrop()
    {
        if (interactableTrigger != null)
        {
            if(!isHolding && interactableTrigger.CompareTag("Loot"))
            {
                DropPoint dp = interactableTrigger.parent.parent.GetComponent<DropPoint>();
                if (dp != null)
                {
                    dp.currentLight = null;
                    dp.SyncDP();
                }

                lootedLight = interactableTrigger.parent;
                Quaternion lootRotation = interactableTrigger.parent.rotation;
                interactableTrigger.parent.SetParent(transform);
                interactableTrigger.parent.SetLocalPositionAndRotation(Vector3.zero, lootRotation);

                isHolding = true;
            }
            if(isHolding && interactableTrigger.CompareTag("Drop"))
            {
                Quaternion lootRotation = lootedLight.rotation;
                lootedLight.SetParent(interactableTrigger.parent);
                // lootedLight.SetLocalPositionAndRotation(Vector3.zero, lootRotation);
                lootedLight.SetPositionAndRotation(interactableTrigger.parent.position, lootRotation);

                DropPoint dp = interactableTrigger.parent.GetComponent<DropPoint>();
                if (dp != null)
                {
                    dp.currentLight = lootedLight.GetComponent<LightController>();
                    dp.SyncDP();
                }

                lootedLight = null;
                isHolding = false;
            }
        }
    }

    public void RotateLootTo(Vector2 poi)
    {
        if (isHolding)
        {
            lootedLight.localEulerAngles = Vector3.forward * Vector2.SignedAngle(Vector2.up, poi);
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((collision.CompareTag("Loot") && !isHolding) || (collision.CompareTag("Drop") && isHolding))
        {
            interactableTrigger = collision.transform;
            helpText.SetActive(true);
        }
        if (collision.CompareTag("Switch"))
        {
            isAtSwitch = true;
            helpText.SetActive(true);
        }
        if (collision.CompareTag("Trigger"))
        {
            collision.GetComponent<Trigger>().Execute();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Loot") || collision.CompareTag("Drop"))
        {
            interactableTrigger = null;
            helpText.SetActive(false);
        }
        if (collision.CompareTag("Switch"))
        {
            isAtSwitch = false;
            helpText.SetActive(false);
        }
    }
}

using Photon.Pun;
using UnityEngine;

public enum ItemType {none, hp,stamina,coin }
public class ItemObjectScript : MonoBehaviourPun, IItemUse
{
    public ItemType myType = ItemType.none;
    public float Value;

    Rigidbody body;
    private void Start()
    {
        body = GetComponent<Rigidbody>();
        body.AddForce(Vector3.up * 5, ForceMode.Impulse);
        //Vector3 randomvec = Random.insideUnitSphere;
        //randomvec.y = 1f;
        //randomvec.Normalize();
        //body.AddForce(randomvec * 3,ForceMode.Impulse);
    }

    public IncreaseStat UseItem()
    {
        gameObject.SetActive(false);
        ItemObjectFactory.Instance.RequestDelete(photonView.ViewID);
        switch (myType) 
        {
            case ItemType.hp:
                return new IncreaseStat(ItemType.hp, Value);
            case ItemType.stamina:
                return new IncreaseStat(ItemType.stamina, Value);
            case ItemType.coin:
                return new IncreaseStat(ItemType.coin, Value);
            default:
                return new IncreaseStat();
        }

    }
}

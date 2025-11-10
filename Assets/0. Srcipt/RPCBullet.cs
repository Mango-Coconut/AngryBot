using UnityEngine;

public class RPCBullet : MonoBehaviour
{
    public GameObject effect;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Rigidbody>().AddRelativeForce(Vector3.forward * 1000);
        Destroy(this.gameObject, 3);
    }

    void OnCollisionEnter(Collision collision)
    {
        var contact = collision.GetContact(0);
        var obj = Instantiate(effect, contact.point, Quaternion.identity);
        Destroy(obj, 2);
        Destroy(gameObject);
    }
}

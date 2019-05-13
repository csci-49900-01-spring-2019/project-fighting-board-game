using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public GameObject prefab1;
    public GameObject prefab2;
    public GameTile current_tile1;
    public GameTile current_tile2;
    Manager theManager;
    // Start is called before the first frame update
    void Start()
    {
        theManager = GameObject.FindObjectOfType<Manager>();
        Transform tf = GetComponent<Transform>();

        Vector3 pos = current_tile1.GetTilePosition();
        Vector3 pos2 = current_tile2.GetTilePosition();
        Quaternion rot = Quaternion.Euler(-90, 0, 0);
        Instantiate(prefab1, pos, rot);
        Instantiate(prefab2, pos2, rot);

    }

    public IEnumerator MoveStore1(Vector3 destination)
    {
        float n = 0.0f;
        Vector3 currentPosition = current_tile1.GetTilePosition();

        while (n < 1.1f)
        {
            Transform tf = GetComponent<Transform>();
            Vector3 newPosition = destination;
            newPosition.y = tf.position.y;
            currentPosition.y = tf.position.y;

            //float journeyLength = Vector3.Distance(currentPosition, destination);
            tf.position = Vector3.Lerp(currentPosition, newPosition, n);
            n += 0.1f;
            yield return null;
        }
        yield return null;
    }
    public IEnumerator MoveStore2(Vector3 destination)
    {
        float n = 0.0f;
        Vector3 currentPosition = current_tile2.GetTilePosition();

        while (n < 1.1f)
        {
            Transform tf = GetComponent<Transform>();
            Vector3 newPosition = destination;
            newPosition.y = tf.position.y;
            currentPosition.y = tf.position.y;

            //float journeyLength = Vector3.Distance(currentPosition, destination);
            tf.position = Vector3.Lerp(currentPosition, newPosition, n);
            n += 0.1f;
            yield return null;
        }
        yield return null;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

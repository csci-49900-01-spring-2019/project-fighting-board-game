using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponList : MonoBehaviour
{
    public List<Weapon> wepList = new List<Weapon>();   //separate weapon tiers by their placement in array, i.e.: tier 1 (0-9) tier 2 (10-19) tier 3 (20-25)
    public string[] availableNames = new string[3];

    // Start is called before the first frame update
    void Start()
    {
        availableNames[0] = "sword";
        availableNames[1] = "hammer";
        availableNmaes[2] = "stick";
        for (int i = 0; i < 25; i++)
        {
            int j = Random.Range(0, 2);
            wepList.Add(new Weapon(availableNames[j]));
        }
    }

    public

    // Update is called once per frame
    void Update()
    {

    }
}

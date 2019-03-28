using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class Weapon_List : MonoBehaviour
{
    public List<Weapon> wepList = new List<Weapon> { };   //separate weapon tiers by their placement in array, i.e.: tier 1 (0-9) tier 2 (10-19) tier 3 (20-25)
    public string[] availableNames = new string[3];
    public string[] availableAdj = new string[4];
    public string[] availableAdj2 = new string[11];

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Weapon Start is called");
        availableAdj2[0] = "";
        availableAdj2[1] = "long";
        availableAdj2[2] = "short";
        availableAdj2[3] = "strong";
        availableAdj2[4] = "weak";
        availableAdj2[5] = "quick";
        availableAdj2[6] = "grand";
        availableAdj2[7] = "critical";
        availableAdj2[8] = "broken";
        availableAdj2[9] = "gamer breaker";
        availableAdj2[10] = "chance";

        availableAdj[0] = "";
        availableAdj[1] = "burning";
        availableAdj[2] = "poisoning";
        availableAdj[3] = "stunning";

        availableNames[0] = "sword";
        availableNames[1] = "hammer";
        availableNames[2] = "stick";
        for (int i = 0; i < 25; i++)
        {
            int j = Random.Range(0, 2);
            int k = Random.Range(0, 3);
            int l = Random.Range(0, 10);
            wepList.Add(new Weapon(availableNames[j], availableAdj[k], availableAdj2[l]));
            Debug.Log(i + "has been added");
        }
        for (int i = 0; i < wepList.Count; i++)
            Debug.Log(wepList[i].finalName);

        wepList = wepList.OrderBy(x => x.dRangeLimit).ToList();
        Debug.Log("Weapon List has been ordered by max Damage");
        for (int i = 0; i < wepList.Count; i++)
            Debug.Log(wepList[i].finalName);
    }

    //return final.OrderBy(s => s.PlayOrder).ThenBy(s => s.Name);       use this to sort the weapon list so that it can be properly tiered i.e. max damage then status effects
    public void rankList()
    {
        for (int i = 0; i < wepList.Count; i++)
            wepList[i].rank = i + 1;
    }


    public Weapon GetWeapon()
    {
        int j = Random.Range(0, 24);
        return wepList[j];
    }
    // Update is called once per frame
    void Update()
    {

    }
}

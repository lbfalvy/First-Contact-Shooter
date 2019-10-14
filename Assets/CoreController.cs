using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CoreController : MonoBehaviour
{
    public float maxDistance;
    HashSet<NetworkPlayer> ActivePlayers;

    // Start is called before the first frame update
    void Start()
    {
        var players = FindObjectsOfType<NetworkPlayer>();
        ActivePlayers = new HashSet<NetworkPlayer>(players);
    }

    // Update is called once per frame
    void Update()
    {
        var removed = new HashSet<NetworkPlayer>();
        foreach (var p in ActivePlayers.Where(isTooFar)) {
            var msg = string.Format("{0} lost radio signal.", p.playerID);
            Debug.Log(msg);
            p.takeDamage(100);
            removed.Add(p);
        }
        ActivePlayers.ExceptWith(removed);
    }
    public void addPlayer(NetworkPlayer p) {
        ActivePlayers.Add(p);
    }

    bool isTooFar(NetworkPlayer p) {
        Vector3 deltaPos = p.transform.position - transform.position;
        return deltaPos.magnitude > maxDistance;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Linq;

public class NetworkPlayer : NetworkBehaviour
{    
    #region PlayerID
    string makePlayerId() => string.Format("Player {0}", netId.Value);
    public override void OnStartLocalPlayer() { 
        CmdSetPlayerID(makePlayerId());
    } // Local sends to server
    [Command] void CmdSetPlayerID(string newID) => playerID = newID; 
    // Server updates
    [SyncVar(hook = "OnPlayerIdChange")] public string playerID; 
    // Gets synced
    void OnPlayerIdChange(string id) {
        playerID = id;
        UpdateName();
    } // Updates nametag
    public override void OnStartClient() => UpdateName(); // Inits nametag
    void UpdateName() => GetComponent<NameTagWithDistance>().Name = playerID;
    #endregion

    #region Health and damage
    [SyncVar(hook="OnTakeDamage")] public int Health = 100;
    [ClientRpc]
    void RpcDoExplosion() {
        Debug.LogError("U totally ded 4ever");
    }
    public void takeDamage(int damage) {
        if (isClient) return;
        Health -= damage;
        if (Health <= 0) {
            Health = 0;
            RpcDoExplosion();
            GameObject.Destroy(gameObject, 0.5f);
        }
    }
    
    #endregion

    void OnGUI() { if(isLocalPlayer) drawGui(); }
    void drawGui() {
        var hpRect = new Rect(10,10,100,100);
        var hpString = string.Format("{0} HP", Health);
        GUI.Label(hpRect, hpString);
    }
}

/*
    public override bool OnCheckObserver(NetworkConnection conn) {
        PlayerController pc = getPlayerFromConnection(conn);
        Transform playertf = pc.gameObject.transform;
        float playerDistance = Vector3.Distance(transform.position, playertf.position);
        return playerDistance < visRange;
    }

    PlayerController getPlayerFromConnection(NetworkConnection conn) {
        return conn.playerControllers.Single();
    }

    public override OnRebuildObservers(HashSet<NetworkConnection> observers, bool initialize) {
        bool didWork = false;
        var removed = new HashSet<NetworkConnection>();
        foreach (var observer in observers) {
            Transform tf = getPlayerFromConnection(observer).gameObject.transform;
            if (Vector3.Distance(tf.position, transform.position) > visRange)
                removed.Add(observer);
        }
        if (removed.Count > 0) {
            observers.ExceptWith(removed);
            didWork = true;
        }
        var hits = GetObservers();
    }
    IEnumerable<NetworkIdentity> GetObservers() {
        HashSet<NetworkIdentity> observers;
        Collider[] cols = Physics.OverlapSphere(transform.position, visRange);
        foreach (var col in cols) {
            var uv = col.GetComponent<NetworkIdentity
        }
            .Select(x => GetIdentity(x.gameObject)).Where(x => x != null);
    }
    NetworkIdentity GetIdentity(GameObject go) {
        NetworkIdentity ni;
        if (go.TryGetComponent<NetworkIdentity>(out ni))
            return ni;
        return null;
    }
*/
using System.Collections.Generic;
using UnityEngine;
using Mirror;

[AddComponentMenu("Network/ Interest Management/ Zone/Zone Interest Management")]
public class ZoneInterestManagement : InterestManagement
{
    [Tooltip("Rebuild all every 'rebuildInterval' seconds.")]
    public float rebuildInterval = 0.1f;
    double lastRebuildTime;

    // cache zones for checking players in them
    HashSet <NetworkZone> zones = new HashSet<NetworkZone>();
    HashSet<NetworkZone2D> zones2D = new HashSet<NetworkZone2D>();

    
    [ServerCallback]
    public override bool OnCheckObserver(NetworkIdentity identity, NetworkConnectionToClient newObserver)
    {
        return true;
    }

    
    [ServerCallback]
    public override void OnRebuildObservers(NetworkIdentity identity, HashSet<NetworkConnectionToClient> newObservers)
    {
        
        foreach (var zone in zones)
        {
            foreach (var zoneObject in zone.zoneObjects)
            {
                if (identity.connectionToClient != null && zone.zoneObjects.Contains(identity.connectionToClient))
                    newObservers.Add(zoneObject);
            }
        }

        foreach (var zone2D in zones2D)
        {
            foreach (var zoneObject in zone2D.zoneObjects)
            {
                if (identity.connectionToClient != null && zone2D.zoneObjects.Contains(identity.connectionToClient))
                    newObservers.Add(zoneObject);
            }
        }

    }

    [ServerCallback]
    public override void OnSpawned(NetworkIdentity identity) 
    {
        if (identity.TryGetComponent(out NetworkZone zone))
            zones.Add(zone);
        else if (identity.TryGetComponent(out NetworkZone2D zone2D))
            zones2D.Add(zone2D);
    }

    [ServerCallback]
    public override void OnDestroyed(NetworkIdentity identity)
    {
        if (identity.TryGetComponent(out NetworkZone zone))
            zones.Remove(zone);
        else if (identity.TryGetComponent(out NetworkZone2D zone2D))
            zones2D.Remove(zone2D);
    }


    [ServerCallback]
    public override void SetHostVisibility(NetworkIdentity identity, bool visible)
    {
        base.SetHostVisibility(identity, visible);
    }

    
    [ServerCallback]
    public override void Reset() { lastRebuildTime = 0D; }

    [ServerCallback]
    internal void Update()
    {
        if (NetworkTime.localTime >= lastRebuildTime + rebuildInterval)
        {
            RebuildAll();
            lastRebuildTime = NetworkTime.localTime;
        }
    }
}

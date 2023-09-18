using System.Collections.Generic;
using UnityEngine;

namespace Mirror
{
    [DisallowMultipleComponent]
    [AddComponentMenu("Network/ Interest Management/ Zone/Network Zone")]
    public class NetworkZone : NetworkBehaviour
    {
        public readonly HashSet<NetworkConnectionToClient> zoneObjects = new HashSet<NetworkConnectionToClient>();

        // Checking on rigidbody component in object 
        [ServerCallback]
        private void Start()
        {
            if (!this.TryGetComponent(out Rigidbody rb))
                Debug.LogWarning("Missing Rigidbody component");
        }

        [ServerCallback]
        void OnTriggerEnter(Collider collision)
        {
            if(collision != null)
            {
                if (collision.TryGetComponent(out NetworkIdentity identity))
                    if (identity.connectionToClient != null) 
                        zoneObjects.Add(identity.connectionToClient);
            }
        }

        [ServerCallback]

        private void OnTriggerExit(Collider collision)
        {
            if (collision != null)
            {
                if (collision.TryGetComponent(out NetworkIdentity identity))
                    if (identity.connectionToClient != null)
                        zoneObjects.Remove(identity.connectionToClient);
            }
        }
    }
}


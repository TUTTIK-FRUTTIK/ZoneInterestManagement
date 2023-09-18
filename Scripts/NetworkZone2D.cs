using System.Collections.Generic;
using UnityEngine;

namespace Mirror
{
    [DisallowMultipleComponent]
    [AddComponentMenu("Network/ Interest Management/ Zone/Network Zone 2D")]
    public class NetworkZone2D : NetworkBehaviour
    {
        public readonly HashSet<NetworkConnectionToClient> zoneObjects = new HashSet<NetworkConnectionToClient>();


        // Checking on rigidbody component in object 
        [ServerCallback]
        private void Start()
        {
            if (!this.TryGetComponent(out Rigidbody2D rb2D))
                Debug.LogWarning("Missing Rigidbody2D component");
        }

        [ServerCallback]
        void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision != null)
            {
                if (collision.TryGetComponent(out NetworkIdentity identity))
                    if (identity.connectionToClient != null)
                        zoneObjects.Add(identity.connectionToClient);
            }
        }

        [ServerCallback]

        private void OnTriggerExit2D(Collider2D collision)
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



using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lily
{
    [RequireComponent(typeof(Entity))]
    public class KillBy : MonoBehaviour
    {
        Entity entity;
        // Start is called before the first frame update
        void Awake()
        {
            entity = GetComponent<Entity>();
        }

        // Update is called once per frame
        void Update()
        {
        
            if (transform.position.y <= -5) entity.TakeDamage(10);
        
        }
    }
}

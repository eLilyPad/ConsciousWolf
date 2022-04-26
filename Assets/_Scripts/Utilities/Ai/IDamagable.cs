using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lily
{
    public interface IDamagable
    { 
        // int Health { get; set;}

        void TakeDamage(int damage);
    }
}

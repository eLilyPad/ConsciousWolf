using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lily
{
    public class ShaderHandler : MonoBehaviour
    {
        [SerializeField] private WindZone windZone;

        private new Renderer renderer;

        private void Awake()
        {
            renderer = GetComponent<Renderer>();
        }

        private void Update()
        {
            Vector3 velocity = windZone.transform.forward * windZone.windMain;
            renderer.material.SetVector("_WindVelocity", velocity);
            renderer.material.SetFloat("_WindFrequency", windZone.windPulseFrequency);
        }
    }
}

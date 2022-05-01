using UnityEngine;

namespace Lily
{
  public class Entity : MonoBehaviour
  {
    public int entityID;
    public string Name;
    public Rigidbody rb;

    public EntityManager EntityManager; 
    public GameManager GameManager; 

    void  start()
    {
      EntityManager = GetComponent<EntityManager>();
    }
  }
}
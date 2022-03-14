using UnityEngine;

namespace Lily.MovementSystem.Controller
{
   using Locomotion;
   using Conditions;
   using Steering;

    public class MovementController
    {
		  #region Variables 

        #region Movement Settings
          //[SerializeField] MovementSettings movementSettings;
          [SerializeField] Transform orientation;
          float acceleration;
          float maxSpeed;
          float currentSpeed;
        #endregion

        #region Movement Componments
          MovementConditions conditions;
          MovementSteering steering;
          MovementLocomotion locomotion;

        #endregion

      #endregion

      #region Setup
      void Awake()
      {
         //conditions = GetComponent<MovementConditions>();
         //orientation = GetComponent<Transform>();
         //this.maxSpeed = movementSettings.MaxSpeed;
         //this.acceleration = movementSettings.Acceleration;
      }

      #endregion

      #region Conditions
      #endregion

      #region Steering
      public void TurnTowardsTarget(Vector3 target, float speed)
      {
         steering.TurnTowardsTarget(target, speed);
      }

      #endregion

      #region Locomotion

      public void Walk(bool forward = true)
      {
         locomotion.WalkForward();
      }

      public void StopMoving()
		{
         locomotion.StopMoving();
		}

      #endregion

      #region Animation

      void AnimationController()
		{
			//float vel = Vector3.Magnitude(rb.velocity);
			//anim.SetFloat("velocity", vel, 0.1f, Time.deltaTime);
		}
      #endregion
   }
}

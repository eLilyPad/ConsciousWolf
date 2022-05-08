using UnityEngine;

namespace Lily.MovementSystem
{
    using Lily.Ai.Pathfinder;
    using Lily.MathsUtils;
    public class FollowPath : MovementController
    {
        public float stopRadius = 0.005f;

        public float pathOffset = 0.71f;

        public float pathDirection = 1f;

        public Vector3 GetSteering(Path path)
        {
            Vector3 targetPosition;
            return GetSteering(path, out targetPosition);
        }

        public Vector3 GetSteering(Path path, out Vector3 targetPosition)
        {

            /* If the path has only one node then just go to that position. */
            if (path.Length == 1)
            {
                targetPosition = path[0];
            }
            /* Else find the closest spot on the path to the character and go to that instead. */
            else
            {
                /* Get the param for the closest position point on the path given the character's position */
                float param = MathsUtils.PositionOnLine(transform.position, path.Waypoints);

                //Debug.DrawLine(transform.position, path.getPosition(param, pathLoop), Color.red, 0, false);
                    Vector3 finalDestination;

                    /* If we are close enough to the final destination then stop moving */
                    if (IsAtEndOfPath(path, param, out finalDestination))
                    {
                        targetPosition = finalDestination;

                        rb.velocity = Vector3.zero;
                        return Vector3.zero;
                    }


                /* Move down the path */
                param += pathDirection * pathOffset;

                /* Set the target position */
                targetPosition = MathsUtils.PositionOnLine(path.Waypoints, param);

                //Debug.DrawLine(transform.position, targetPosition, Color.red, 0, false);
            }

            return Arrive(targetPosition);
        }

        /// <summary> 
        /// Will return true if the character is at the end of the given path 
        /// </summary>
        public bool IsAtEndOfPath(Path path)
        {
            /* If the path has only one node then just check the distance to that node. */
            if (path.Length == 1)
            {
                Vector3 endPos = path[0];
                return Vector3.Distance(rb.position, endPos) < stopRadius;
            }
            /* Else see if the character is at the end of the path. */
            else
            {
                Vector3 finalDestination;

                /* Get the param for the closest position point on the path given the character's position */
                float param = MathsUtils.PositionOnLine(transform.position, path.Waypoints);

                return IsAtEndOfPath(path, param, out finalDestination);
            }
        }

        bool IsAtEndOfPath(Path path, float param, out Vector3 finalDestination)
        {
            bool result;

            /* Find the final destination of the character on this path */
            finalDestination = (pathDirection > 0) ? path[path.Length - 1] : path[0];

            /* If the param is closest to the last segment then check if we are at the final destination */
            if (param >= path.Distances[path.Length - 2])
            {
                result = Vector3.Distance(rb.position, finalDestination) < stopRadius;
            }
            /* Else we are not at the end of the path */
            else
            {
                result = false;
            }

            return result;
        }
    }
}
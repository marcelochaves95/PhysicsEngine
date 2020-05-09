﻿using UnityEngine;
using Vector3 = Kinematics.MathModule.Vector3;

namespace Kinematics.CollisionModule
{
    [RequireComponent(typeof(Rigidbody))]
    [AddComponentMenu("Kinematics/CollisionModule/PhysicsMaterial")]
    public class PhysicsMaterial : MonoBehaviour
    {
        [SerializeField] private float ue = 0.3f;
        [SerializeField] private float uc = 0.3f;

        private Vector3 fe;
        private Vector3 fc;

        public Vector3 CalculateFriction(Vector3 velocity, Vector3 force, float normal, float mass)
        {
            var time = Time.fixedDeltaTime;
            var acceleration = new Vector3();

            fe = ue * -Vector3.Normalize(velocity) * normal;
            fc = uc * -Vector3.Normalize(velocity) * normal;

            if (Vector3.Magnitude(velocity) > 0f)
                acceleration = (force + fc / mass);
            else
                acceleration = (force + fe / mass);

            if (((Vector3.Magnitude(force) - Vector3.Magnitude(fe)) > 0f) || (Vector3.Magnitude(velocity) > 0f))
                velocity += acceleration * time;
            else
                velocity = Vector3.Zero;

            return velocity;
        }
    }
}

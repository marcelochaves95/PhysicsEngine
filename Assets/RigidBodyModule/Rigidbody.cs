﻿using Vector3 = PhysicsEngine.MathModule.Vector3;

using UnityEngine;

namespace PhysicsEngine.CollisionModule
{
    public class Rigidbody : MonoBehaviour
    {
        [SerializeField] private float mass = 0.1f;
        [SerializeField] private float drag = 0.0f;

        [SerializeField] private bool useGravity = false;

        [SerializeField] private Vector3 gravity = new Vector3(0.0f, -9.81f, 0.0f);

        [SerializeField] private bool isKinematic = false;

        [Header("Freeze Position")]
        [SerializeField] private bool x = false;
        [SerializeField] private bool y = false;
        [SerializeField] private bool z = false;

        private Vector3 velocity = Vector3.Zero;
        private Vector3 position = Vector3.Zero;

        new Collider collider;

        /* UnityEngine Properties */
        private float deltaTime;
        private float fixedDeltaTime;

        UnityEngine.Vector3 pos = UnityEngine.Vector3.zero;
        
        private void Start()
        {
            collider = GetComponent<Collider>();
            position.X = transform.position.x;

            deltaTime = Time.deltaTime;
            fixedDeltaTime = Time.fixedDeltaTime;
        }

        private void Update()
        {
            velocity += gravity * fixedDeltaTime * (useGravity ? 1 : 0);

            if (!isKinematic)
                velocity *= (1 - (drag * fixedDeltaTime));

            velocity = (Vector3.Right * velocity.X * (x ? 0 : 1)) + (Vector3.Up * velocity.Y * (y ? 0 : 1)) + (Vector3.Forward * velocity.Z * (z ? 0 : 1));

            if (CheckCollision(Vector3.Up * velocity.Y * deltaTime) != null)
            {
                if (collider.GetPhysicsMaterial() != null)
                    velocity = collider.GetPhysicsMaterial().CalculateFriction(velocity, Vector3.Zero, Vector3.Magnitude(gravity) * mass, mass);

                if (CheckCollision(Vector3.Up * velocity.Y * deltaTime).GetCenter().Y + CheckCollision(Vector3.Up * velocity.Y * deltaTime).GetSize().Y <= collider.GetCenter().Y)
                    velocity.Y = 0;
            }

            position += velocity * deltaTime;
            pos.x = position.X;
            pos.y = position.Y;
            pos.z = position.Z;
            this.transform.position = pos;
        }

        public void AddForce(Vector3 force)
        {
            if (!isKinematic)
                velocity += ((force / mass) + gravity * (useGravity ? 1 : 0)) * fixedDeltaTime;
        }

        public void SetVelocity(Vector3 velocity)
        {
            this.velocity = velocity;
        }

        public Collider CheckCollision(Vector3 position)
        {
            if (collider != null)
            {
                Vector3 aux = collider.center;
                collider.center = position;

                for (int i = 0; i < Collider.rb_Colliders.Count; i++)
                {
                    if (Collider.CheckCollision(collider, Collider.rb_Colliders[i]) != null)
                        return Collider.CheckCollision(collider, Collider.rb_Colliders[i]);
                }

                collider.center = aux;
            }

            return null;
        }
    }
}
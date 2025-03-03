using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utils.Physics
{
    public class RigidbodyStorer
    {
        private float mass = 0;
        private float drag = 0;
        private float angDrag = 0;
        private bool useGrav = false;
        private bool isKinem = false;
        private RigidbodyInterpolation interp = 0;
        private CollisionDetectionMode colDet = 0;
        private RigidbodyConstraints con = 0;

        private LayerMask exclusionLayers;

        /// <summary>
        /// Stores the values of given rigidbody
        /// </summary>
        /// <param name="rb">Store values from this</param>
        public void StoreValues(Rigidbody rb)
        {
            if (!rb) return;
            mass = rb.mass;
            drag = rb.linearDamping;
            angDrag = rb.angularDamping;
            useGrav = rb.useGravity;
            isKinem = rb.isKinematic;
            interp = rb.interpolation;
            colDet = rb.collisionDetectionMode;
            con = rb.constraints;

            exclusionLayers = rb.excludeLayers;
        }

        /// <summary>
        /// Copies the stored values to rigidbody
        /// </summary>
        /// <param name="rb"></param>
        public void CopyValues(Rigidbody rb)
        {
            if (!rb) return;
            rb.mass = mass;
            rb.linearDamping = drag;
            rb.angularDamping = angDrag;
            rb.useGravity = useGrav;
            rb.isKinematic = isKinem;
            rb.interpolation = interp;
            rb.collisionDetectionMode = colDet;
            rb.constraints = con;

            rb.excludeLayers = exclusionLayers;
        }
    }
}

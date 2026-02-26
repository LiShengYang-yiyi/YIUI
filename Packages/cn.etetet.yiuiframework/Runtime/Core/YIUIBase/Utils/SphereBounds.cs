using UnityEngine;

namespace YIUIFramework
{
    /// <summary>
    /// 球面的边界。
    /// </summary>
    public struct SphereBounds
    {
        public SphereBounds(Vector3 center, float radius)
        {
            this.Center = center;
            this.Radius = radius;
        }

        /// <summary>
        /// 获取或设置球体的中心。
        /// </summary>
        public Vector3 Center { get; set; }

        /// <summary>
        /// 获取或设置球体的半径。
        /// </summary>
        public float Radius { get; set; }

        /// <summary>
        /// 检查是否与其他球体相交。
        /// </summary>
        public bool Intersects(SphereBounds bounds)
        {
            var sqrDistance = (this.Center - bounds.Center).sqrMagnitude;
            var minDistance = this.Radius + bounds.Radius;
            return sqrDistance <= minDistance * minDistance;
        }

        /// <summary>
        /// 检查是否与其他AABB相交。
        /// </summary>
        public bool Intersects(Bounds bounds)
        {
            // 检查球体是否在AABB内部
            if (bounds.Contains(this.Center))
            {
                return true;
            }

            // 检查球体和AABB是否相交。
            var boundsMin = bounds.min;
            var boundsMax = bounds.max;

            float s = 0.0f;
            float d = 0.0f;
            if (this.Center.x < boundsMin.x)
            {
                s =  this.Center.x - boundsMin.x;
                d += s * s;
            }
            else if (this.Center.x > boundsMax.x)
            {
                s =  this.Center.x - boundsMax.x;
                d += s * s;
            }

            if (this.Center.y < boundsMin.y)
            {
                s =  this.Center.y - boundsMin.y;
                d += s * s;
            }
            else if (this.Center.y > boundsMax.y)
            {
                s =  this.Center.y - boundsMax.y;
                d += s * s;
            }

            if (this.Center.z < boundsMin.z)
            {
                s =  this.Center.z - boundsMin.z;
                d += s * s;
            }
            else if (this.Center.z > boundsMax.z)
            {
                s =  this.Center.z - boundsMax.z;
                d += s * s;
            }

            return d <= this.Radius * this.Radius;
        }
    }
}

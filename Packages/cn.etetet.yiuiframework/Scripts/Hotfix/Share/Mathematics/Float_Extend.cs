using Unity.Mathematics;

namespace ET
{
    public static class Float_Extend
    {
        //float3.equals 的精度非常高,
        //这里提供一个精度更低的比较方法
        public static bool Approximately(this float3 a, float3 b, float epsilon = 0.001f)
        {
            bool xEqual = math.abs(a.x - b.x) < epsilon;
            bool yEqual = math.abs(a.y - b.y) < epsilon;
            bool zEqual = math.abs(a.z - b.z) < epsilon;
            return xEqual && yEqual && zEqual;
        }

        public static bool ApproximatelyXZ(this float3 a, float3 b, float epsilon = 0.001f)
        {
            bool xEqual = math.abs(a.x - b.x) < epsilon;
            bool zEqual = math.abs(a.z - b.z) < epsilon;
            return xEqual && zEqual;
        }

        public static bool Approximately(this float2 a, float2 b, float epsilon = 0.001f)
        {
            bool xEqual = math.abs(a.x - b.x) < epsilon;
            bool yEqual = math.abs(a.y - b.y) < epsilon;
            return xEqual && yEqual;
        }

        public static bool Approximately(this float4 a, float4 b, float epsilon = 0.001f)
        {
            bool xEqual = math.abs(a.x - b.x) < epsilon;
            bool yEqual = math.abs(a.y - b.y) < epsilon;
            bool zEqual = math.abs(a.z - b.z) < epsilon;
            bool wEqual = math.abs(a.w - b.w) < epsilon;
            return xEqual && yEqual && zEqual && wEqual;
        }
    }
}
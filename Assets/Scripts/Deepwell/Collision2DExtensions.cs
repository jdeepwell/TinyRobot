using UnityEngine;

namespace Deepwell
{
    public static class Collision2DExtensions
    {
        public static bool WasHitFromBelow(this Collision2D col)
        {
            var normalY = col.contacts[0].normal.y;
            return normalY > .5f;
        }
        
        public static bool WasHitFromAbove(this Collision2D col)
        {
            var normalY = col.contacts[0].normal.y;
            return normalY < -.5f;
        }
    }

    public static class ContactPoint2DExtensions
    {
        public static float Angle(this ContactPoint2D conPtn)
        {
            Vector3 normal = conPtn.normal;
            var angle = Vector3.Angle(Vector3.down, normal);
            return angle;
        }
    }
}

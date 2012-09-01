namespace PresentPerfect.Posture
{
    public class HandsJoinedPosture : IPosture
    {
        private const float Epsilon = 0.1f;

        public bool IsDetected(SkeletonVector skeletonVector)
        {
            if (!skeletonVector.LeftHandPosition.HasValue || !skeletonVector.RightHandPosition.HasValue)
                return false;

            var distance = (skeletonVector.LeftHandPosition.Value - skeletonVector.RightHandPosition.Value).Length;

            if (distance > Epsilon)
                return false;

            return true;
        }

        public string Name 
        { 
            get
            {
                return "HandsJoined";
            }
        }
    }
}

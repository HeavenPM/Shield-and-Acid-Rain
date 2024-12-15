using System.Collections;
using UnityEngine;
using UnityEngine.Playables;

namespace WindowSystem.Runtime.Scripts
{
    public static class TimelineExtensions
    {
        private static readonly WaitForEndOfFrame FrameWait = new();
        private static readonly WaitForFixedUpdate WaitForFixedUpdate = new();

        public static IEnumerator Reverse(this PlayableDirector timeline)
        {
            var defaultUpdateMode = timeline.timeUpdateMode;
            timeline.timeUpdateMode = DirectorUpdateMode.Manual;

            if (timeline.time.ApproxEquals(timeline.duration) || timeline.time.ApproxEquals(0))
            {
                timeline.time = timeline.duration;
            }
            
            timeline.Evaluate();

            yield return FrameWait;
            
            var dt = (float) timeline.duration;
            while (dt > 0)
            {
                dt -= Time.fixedDeltaTime;
                
                timeline.time = Mathf.Max(dt, 0);
                timeline.Evaluate();
                
                yield return WaitForFixedUpdate;
            }

            timeline.Reset();
            timeline.timeUpdateMode = defaultUpdateMode;
            timeline.Stop();
        }

        private static void Reset(this PlayableDirector timeline)
        {
            timeline.time = 0;
            timeline.Evaluate();
        }

        private static bool ApproxEquals(this double num, float other) 
            => Mathf.Approximately((float)num, other);

        private static bool ApproxEquals(this double num, double other) 
            => Mathf.Approximately((float)num, (float)other);
    }
}
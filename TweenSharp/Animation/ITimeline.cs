using System.Threading.Tasks;

namespace TweenSharp.Animation
{
    public interface ITimeline
    {
        bool IsDone { get; }
        TimelineOptions Options { get; set; }
        Task AwaitCompletion();
    }
}
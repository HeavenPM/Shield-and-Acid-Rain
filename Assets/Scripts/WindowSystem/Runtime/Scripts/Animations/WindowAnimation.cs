using System.Threading.Tasks;

namespace WindowSystem.Runtime.Scripts.Animations
{
    public abstract class WindowAnimation
    {
        public abstract void Initialize(WindowAnimator window);
    
        public abstract Task Play();
    }
}
namespace CodeBase.Gameplay
{
    public class BuffModel : IBuffModel
    {
        private float _duration;

        public float Duration => _duration;
        public bool IsExpired => _duration <= 0f;
        
        public void Tick(float deltaTime)
        {
            _duration = Duration - deltaTime;            
        }
    }
}
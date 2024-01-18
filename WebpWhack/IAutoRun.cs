namespace WebpWhack
{
    public class AutoRun : IAutoRun
    {
        public void Set() => throw new NotImplementedException();
        public void Unset() => throw new NotImplementedException();
    }

    public interface IAutoRun
    {
        void Set();
        void Unset();
    }
}

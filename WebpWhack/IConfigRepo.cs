namespace WebpWhack
{
    public interface IConfigRepo
    {
        Config LoadConfig();
        void SaveConfig( Config config );
    }
}

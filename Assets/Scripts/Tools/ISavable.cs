namespace Tools
{
    public interface ISavable : IReloadable
    {
        void Save();
        void Load();
    }
}
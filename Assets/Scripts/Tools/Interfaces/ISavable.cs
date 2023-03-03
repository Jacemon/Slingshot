namespace Tools.Interfaces
{
    public interface ISavable : IReloadable
    {
        void SaveData();
        void LoadData();
    }
}
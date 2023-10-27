namespace MVC.Contracts
{
    public interface ILocalStorageService
    {
        void CleareStorage(List<string> keys);

        bool Exist(string key);

        T GetStorageValue<T>(string key);

        void SetStorageValue<T>(string key, T value);
    }
}
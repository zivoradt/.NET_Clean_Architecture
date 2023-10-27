using Hanssens.Net;
using MVC.Contracts;

namespace MVC.Services
{
    public class LocalStorageService : ILocalStorageService
    {
        private LocalStorage _localStorage;

        public LocalStorageService()
        {
            var config = new LocalStorageConfiguration
            {
                AutoLoad = true,
                AutoSave = true,
                Filename = "HR.LEAVEMGMT"
            };
            _localStorage = new LocalStorage(config);
        }

        public void CleareStorage(List<string> keys)
        {
            foreach (string key in keys)
            {
                _localStorage.Remove(key);
            }
        }

        public bool Exist(string key)
        {
            return _localStorage.Exists(key);
        }

        public T GetStorageValue<T>(string key)
        {
            return _localStorage.Get<T>(key);
        }

        public void SetStorageValue<T>(string key, T value)
        {
            _localStorage.Store(key, value);
            _localStorage.Persist();
        }
    }
}
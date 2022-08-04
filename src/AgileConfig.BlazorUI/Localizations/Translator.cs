using System.Threading.Tasks;
using AgileConfig.BlazorUI.Consts;
using AgileConfig.BlazorUI.Enums;
using Blazored.LocalStorage;

namespace AgileConfig.BlazorUI.Localizations
{
    public class Translator
    {
        private readonly ILocalStorageService _localStorage;
        private readonly ISyncLocalStorageService _syncLocalStorage;

        public Translator(ILocalStorageService localStorage,
            ISyncLocalStorageService syncLocalStorage)
        {
            _localStorage = localStorage;
            _syncLocalStorage = syncLocalStorage;
        }

        public async Task SetLanguage(SupportLanguage language)
            => await _localStorage.SetItemAsync(CacheKey.LANGUAGE, language);

        public SupportLanguage GetLanguage()
            => _syncLocalStorage.GetItem<SupportLanguage>(CacheKey.LANGUAGE);
    }
}

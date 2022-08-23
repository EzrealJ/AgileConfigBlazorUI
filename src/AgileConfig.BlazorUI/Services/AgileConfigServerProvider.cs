using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AgileConfig.BlazorUI.Auth;
using AgileConfig.BlazorUI.Model;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using WebApiClientCore;

namespace AgileConfig.BlazorUI.Services
{
    public class AgileConfigServerProvider
    {
        private readonly AuthService _authService;
        private readonly ILocalStorageService _localStorageService;
        private readonly ISyncLocalStorageService _syncLocalStorageService;
        private readonly IWebAssemblyHostEnvironment _hostEnvironment;

        public AgileConfigServerProvider(
            AuthService authService,
            ILocalStorageService localStorageService,
            ISyncLocalStorageService syncLocalStorageService,
            IWebAssemblyHostEnvironment hostEnvironment)
        {
            _authService = authService;
            _localStorageService = localStorageService;
            _syncLocalStorageService = syncLocalStorageService;
            _hostEnvironment = hostEnvironment;
        }

        public async ValueTask<List<AgileConfigServerSetting>> GetAllAsync()
        {
            var list = await _localStorageService.GetItemAsync<List<AgileConfigServerSetting>>(Consts.CacheKey.AGILE_CONFIG_SERVER_SETTINGS);
            list ??= new();
            if (list.Count == 0)
            {
                list.Add(new AgileConfigServerSetting
                {
                    Name = "默认的演示Api，由AgileConfig提供",
                    Url = "http://agileconfig_server.xbaby.xyz/",
                    IsCurrent = true,
                });
            }
            return list;
        }

        public async ValueTask SetCurrentAsync(string url)
        {
            var currentUrl = _hostEnvironment.BaseAddress;
            var uri = new Uri(currentUrl);
            var uriTarget = new Uri(url);
            if (uri.Scheme != uriTarget.Scheme)
            {
                throw new Exception($"当前AgileBlazorUI运行在{uri.Scheme},您不能将{uriTarget.Scheme}的地址设置为活动");
            }
            var list = await GetAllAsync();
            list.ForEach(s =>
            {
                if (s.Url == url)
                {
                    s.IsCurrent = true;
                }
                else
                {
                    s.IsCurrent = false;
                }
            });

            await _authService.LogoutAsync();
            await ReSetAsync(list);
        }

        internal async Task AddAsync(AgileConfigServerSetting currentObject)
        {
            AssertIsValid(currentObject);
            var list = await GetAllAsync();
            var server = list.FirstOrDefault(s => s.Url == currentObject.Url);
            if (server != default)
            {
                throw new Exception("这个服务已经添加过了");
            }
            else
            {
                list.Add(currentObject);
            }
            await ReSetAsync(list);
        }
        internal async ValueTask RemoveAsync(string url)
        {
            var list = await GetAllAsync();
            var server = list.FirstOrDefault(s => s.Url == url);
            list.Remove(server);
            await ReSetAsync(list);
        }
        private async Task ReSetAsync(List<AgileConfigServerSetting> list)
        {

            await _localStorageService.SetItemAsync(Consts.CacheKey.AGILE_CONFIG_SERVER_SETTINGS, list);
        }

        internal async Task UpdateAsync(AgileConfigServerSetting currentObject)
        {
            AssertIsValid(currentObject);
            var list = await GetAllAsync();
            var server = list.FirstOrDefault(s => s.Url == currentObject.Url);
            if (server == default)
            {
                throw new Exception("没有找到这个服务");
            }
            else
            {
                list.Remove(server);
                list.Add(currentObject);
            }
            await ReSetAsync(list);
        }


        private static void AssertIsValid(AgileConfigServerSetting serverSetting)
        {
            if (serverSetting == default)
            {
                throw new Exception("请填写服务信息");
            }
            try
            {
                string url = serverSetting.Url;
                var uri = new Uri(url);
                if (uri.Scheme.ToLower() != "http" && uri.Scheme.ToLower() != "https")
                {
                    throw new Exception("请填写合适的地址");
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        internal async Task<AgileConfigServerSetting> GetCurrentAsync()
        {

            var list = await GetAllAsync();
            var server = list.FirstOrDefault(s => s.IsCurrent);
            return server;
        }
    }
}

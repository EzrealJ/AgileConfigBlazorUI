using System.Collections.Generic;
using System.Threading.Tasks;
using AgileConfig.BlazorUI.Components.AgileConfigServer;
using AgileConfig.BlazorUI.Enums;
using AgileConfig.BlazorUI.Model;
using AgileConfig.BlazorUI.Services;
using Microsoft.AspNetCore.Components;

namespace AgileConfig.BlazorUI.Pages
{
    public partial class AgileConfigServerManager
    {
        [Inject]
        private AgileConfigServerProvider AgileConfigServerProvider { get; set; }

        private List<AgileConfigServerSetting> AgileConfigServerSettings { get; set; }
        private string ShowTypeString => _itemShowType == EnumItemShowType.Card ? "表格显示" : "卡片显示";
        private EnumItemShowType _itemShowType;
        private EnumEditType _editType;
        private EditAgileConfigServer _editNode;

        protected override async Task OnParametersSetAsync()
        {
            await LoadAsync();
        }

        private async Task LoadAsync()
        {
            AgileConfigServerSettings = await AgileConfigServerProvider.GetAllAsync();
            StateHasChanged();
        }

        private void ChangeShowType()
        {
            _itemShowType = _itemShowType == EnumItemShowType.TableRow
                ? EnumItemShowType.Card
                : EnumItemShowType.TableRow;
        }

        private async Task SetCurrentServerAsync(string serverUrl)
        {
            await AgileConfigServerProvider.SetCurrentAsync(serverUrl);
            await LoadAsync();
        }

        private async Task DeleteServerAsync(string serverUrl)
        {
            await AgileConfigServerProvider.RemoveAsync(serverUrl);
            await LoadAsync();
        }

        private void EditServer(AgileConfigServerSetting serverSetting)
        {
            _editNode.CurrentObject = serverSetting;
            _editType = EnumEditType.Edit;
            _editNode.Visible = true;
        }


    }
}

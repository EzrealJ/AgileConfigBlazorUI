namespace AgileConfig.BlazorUI.Pages
{
    public partial class App
    {
        #region 搜索条件
        class FormClass
        {
            public string Id { get; set; }
            public string Name { get; set; }
            public string Group { get; set; }
        }

        FormClass _formClass=new();
        #endregion
    }
}

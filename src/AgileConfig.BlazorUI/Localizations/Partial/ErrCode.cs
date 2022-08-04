using System.Text.Json.Serialization;

namespace AgileConfig.BlazorUI.Localizations.Partial
{
    public class ErrCode
    {
        /// <summary>
        /// 原始密码不能为空
        /// </summary>
        [JsonPropertyName("err_resetpassword_01")] 
        public string ErrResetpassword01 { get; set; }
        /// <summary>
        /// 原始密码错误，请重新再试
        /// </summary>
        [JsonPropertyName("err_resetpassword_02")] 
        public string ErrResetpassword02 { get; set; }
        /// <summary>
        /// 新密码不能为空
        /// </summary>
        [JsonPropertyName("err_resetpassword_03")] 
        public string ErrResetpassword03 { get; set; }
        /// <summary>
        /// 新密码最长不能超过50位
        /// </summary>
        [JsonPropertyName("err_resetpassword_04")] 
        public string ErrResetpassword04 { get; set; }
        /// <summary>
        /// 输入的两次新密码不一致
        /// </summary>
        [JsonPropertyName("err_resetpassword_05")] 
        public string ErrResetpassword05 { get; set; }
    }


}

# AgileConfig.BlazorUI

#### 使用Blazor Wasm技术构建的AgileConfig管理UI

### 服务端请移步:[AgileConfig](https://github.com/dotnetcore/AgileConfig)

下载:[Github-Releases](https://github.com/EzrealJ/AgileConfigBlazorUI/releases)

使用:

* 0、请务必解压。

* 1、更改appsettings.json(**对于host和winapp则是wwwroot下的appsettings.json**)中的AgileConfigServer的值为你的Server地址
* 2、启动：
  - 对于dist,请使用一个web服务器讲其视为静态资源托管它，它是必定每次都会发布的
  - 对于simplehost，需要安装.NET 6运行时，然后双击AgileConfig.BlazorUI.SimpleHost.exe；注意，server是http则访问http的地址，server是https请使用https的地址，
  - 对于winapp，暂时是实验版本，未来可能不会继续发布它

若有疑问和bug和遗漏项请issue交流，或者在AgileConfig的QQ群@我，

项目依赖

| Package                   | Version | Link                                                         | Licence                                                      |
| ------------------------- | ------- | ------------------------------------------------------------ | ------------------------------------------------------------ |
| AntDesign.Charts          | 0.2.2   | [AntDesign.Charts](https://github.com/ant-design-blazor/ant-design-charts-blazor) | [Apache-2.0 License](https://github.com/ant-design-blazor/ant-design-charts-blazor/blob/master/LICENSE) |
| AntDesign.ProLayout       | 0.1.10  | [AntDesign.ProLayout](https://github.com/ant-design-blazor/blazor-pro-components) | [Apache-2.0 License](https://github.com/ant-design-blazor/blazor-pro-components/blob/master/LICENSE) |
| Blazor.Extensions.Logging | 2.0.4   | [Blazor.Extensions.Logging](https://github.com/BlazorExtensions/Logging) | [MIT License](https://github.com/BlazorExtensions/Logging/blob/master/LICENSE) |
| Blazored.LocalStorage     | 4.2.0   | [Blazored.LocalStorage ](https://github.com/Blazored/LocalStorage) | [MIT License](https://github.com/Blazored/LocalStorage/blob/main/LICENSE) |
| WebApiClientCore          | 2.0.2   | [WebApiClientCore](https://github.com/dotnetcore/WebApiClient) | [MIT License](https://github.com/dotnetcore/WebApiClient/blob/master/LICENSE) |

项目进度

| 功能           | 进度      |
| -------------- | --------- |
| 登录           | Completed |
| 首页           | Completed |
| 节点           | Completed |
| 应用           | Completed |
| 客户端         | Completed |
| 服务           | Completed |
| 用户           | Completed |
| 日志           | Completed |
| 修改密码       | Completed |
| English        | 0%        |
| **操作员权限** | Completed |
| 其它           | 未统计    |
| 遗漏项         | 未统计    |


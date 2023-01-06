# WPFGenericHost

在WPF应用中引入Microsoft.Extensions.Hosting，以便具备依赖注入、日志、配置等功能

参考：https://laurentkempe.com/2019/09/03/WPF-and-dotnet-Generic-Host-with-dotnet-Core-3-0/

# 问题：
1. 视图模型使用了依赖注入后，构造函数里带有参数，无法在xaml中绑定`DataContext`，只能在隐藏代码里处理，这就导致了无法使用Visual Studio的xaml设计视图来处理绑定，只能手工编写xaml代码，并且xaml设计视图无法实时预览绑定数据，只能运行程序后看到效果。
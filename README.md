# Alipay.Demo.PCPayment
支付宝PC网站支付Demo，实现支付、支付同步回调、支付异步通知、订单查询、退款、退款查询、订单关闭功能。采用支付宝服务端SDK：[Alipay.AopSdk.Core](https://github.com/stulzq/Alipay.AopSdk.Core "Alipay.AopSdk.Core")，使用ASP.NET Core MVC 2.0。

# 更新日志

- 2017-11-3 使用AopSdk For ASP.NET Core 组件

- 2017-10-30 修复在Linux上无法使用的BUG

# 说明文档

- [ASP.NET Core 2.0 使用支付宝PC网站支付](http://www.cnblogs.com/stulzq/p/7606164.html "ASP.NET Core 2.0 使用支付宝PC网站支付")

- [ASP.NET Core 2.0 支付宝当面付之扫码支付](http://www.cnblogs.com/stulzq/p/7647948.html "ASP.NET Core 2.0 支付宝当面付之扫码支付")

- [常见问题解答](http://www.cnblogs.com/stulzq/p/7873909.html "常见问题解答")

>若遇到“私钥错误”请务必仔细确认自己的配置。

# 功能演示

- 2017-10-11新增扫码支付

![](Alipay.Demo.PCPayment/image/scancode.gif)

- 支付创建

![](Alipay.Demo.PCPayment/image/2payorder.jpg)

- 支付

![](Alipay.Demo.PCPayment/image/3pay.jpg)

- 输入支付密码

![](Alipay.Demo.PCPayment/image/4pay.jpg)

- 支付成功

![](Alipay.Demo.PCPayment/image/5paysuccess.jpg)

- 支付成功同步回调

![](Alipay.Demo.PCPayment/image/6paysuccess.jpg)

- 订单查询

![](Alipay.Demo.PCPayment/image/7orderquery.jpg)

- 退款

![](Alipay.Demo.PCPayment/image/8refund.jpg)

- 退款查询

![](Alipay.Demo.PCPayment/image/9refundquery.jpg)

- 订单关闭

![](Alipay.Demo.PCPayment/image/10orderclose.jpg)

异步通知已经实现了的，但是由于没有公网ip和花生壳等，没有测试，以后测试了加图

# 配置

```csharp
public void ConfigureServices(IServiceCollection services)
{
    services.AddAlipay(options =>
	        {
		        options.AlipayPublicKey = "支付宝公钥";
		        options.AppId = "应用ID";
		        options.CharSet = "密钥编码";
		        options.Gatewayurl = "支付网关";
		        options.PrivateKey = "商家私钥";
		        options.SignType = "签名方式 RSA/RSA2";
		        options.Uid = "商户ID";
	        });
}
```

移步：https://github.com/stulzq/Alipay.AopSdk.Core 查看详细配置

公钥、私钥直接填写**字符串**

如果支付接口没用申请下来，可以使用支付宝沙箱来测试。

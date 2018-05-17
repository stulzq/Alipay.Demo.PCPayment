# Alipay.Demo.PCPayment

支付宝PC网站支付Demo，实现支付、支付同步回调、支付异步通知、订单查询、退款、退款查询、订单关闭功能。采用支付宝服务端SDK：[Alipay.AopSdk.Core](https://github.com/stulzq/Alipay.AopSdk.Core "Alipay.AopSdk.Core")，使用ASP.NET Core MVC 2.0。

## 一.运行

>配置的 公钥、私钥 直接填写**字符串**，不能带pem格式。如果支付接口没用申请下来，可以使用支付宝沙箱来测试。

打开`alipay.json`，按照字段说明写入信息：

````json
"Alipay": {
    "AlipayPublicKey": "",
    "AppId": "",
    "CharSet": "UTF-8",
    "Gatewayurl": "https://openapi.alipaydev.com/gateway.do",
    "PrivateKey": "",
    "SignType": "RSA2",
    "Uid": ""
  }
````
- AlipayPublicKey：支付宝公钥。由支付宝提供，可到后台获取。
- AppId：应用ID。由支付宝提供，可到后台获取。
- CharSet：密钥编码，一般为 UTF-8
- Gatewayurl：支付网关url。
- PrivateKey：商户私钥，由我们自己生成。格式为pkcs1,长度2048或者1024，推荐2048。
- SignType：签名类型，2048长度密钥请使用`RSA2`，1024使用`RSA`
- Uid：商户ID。由支付宝提供，可到后台获取。

配置好以后就可以直接启动了。

## 二.密钥生成

下载本项目源码，打开`tool`文件夹，解压`keygen.zip`压缩包，运行`start.bat`即可在keys目录下生成公钥和私钥，长度为2048。

这里生成的公钥和私钥，只需将**私钥**配置到`alipay.json`文件中，公钥需要设置到支付宝后台。然后设置`SignType`为`RSA2`。配置文件中的公钥配置，不是我们自己生成的这个，需要到支付宝后台获取，这里需要注意一下。

![1526457521967](assets/1526457521967.png)

## 三.使用帮助

- [ASP.NET Core 2.0 使用支付宝PC网站支付](http://www.cnblogs.com/stulzq/p/7606164.html "ASP.NET Core 2.0 使用支付宝PC网站支付")

- [ASP.NET Core 2.0 支付宝当面付之扫码支付](http://www.cnblogs.com/stulzq/p/7647948.html "ASP.NET Core 2.0 支付宝当面付之扫码支付")

- [常见问题解答](http://www.cnblogs.com/stulzq/p/7873909.html "常见问题解答")

>若遇到“私钥错误”请务必仔细确认自己的配置。

## 四.功能演示

- 2017-10-11新增扫码支付

![](assets/scancode.gif)

- 支付创建

![](assets/2payorder.jpg)

- 支付

![](assets/3pay.jpg)

- 输入支付密码

![](assets/4pay.jpg)

- 支付成功

![](assets/5paysuccess.jpg)

- 支付成功同步回调

![](assets/6paysuccess.jpg)

- 订单查询

![](assets/7orderquery.jpg)

- 退款

![](assets/8refund.jpg)

- 退款查询

![](assets/9refundquery.jpg)

- 订单关闭

![](assets/10orderclose.jpg)

异步通知已经实现了的，但是由于没有公网ip和花生壳等，没有测试，以后测试了加图


namespace Alipay.Demo.PCPayment
{
	public class Config
	{
		// 应用ID,您的APPID
		public static string AppId = "";

		// 支付宝网关
		public static string Gatewayurl = "https://openapi.alipaydev.com/gateway.do";

		// 商户私钥，您的原始格式RSA私钥
		public static string PrivateKey = "";

		// 支付宝公钥,查看地址：https://openhome.alipay.com/platform/keyManage.htm 对应APPID下的支付宝公钥。
		public static string AlipayPublicKey = "";

		// 签名方式
		public static string SignType = "RSA2";

		// 编码格式
		public static string CharSet = "UTF-8";
	}
}
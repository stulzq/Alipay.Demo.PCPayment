using System;
using System.Collections.Generic;
using System.DrawingCore;
using System.DrawingCore.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Alipay.AopSdk.F2FPay.Business;
using Alipay.AopSdk.F2FPay.Domain;
using Alipay.AopSdk.F2FPay.Model;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using QRCoder;

namespace Alipay.Demo.PCPayment.Controllers
{
    public class FTFPayController : Controller
    {
	    private readonly IAlipayTradeService _serviceClient = F2FBiz.CreateClientInstance(Config.Gatewayurl, Config.AppId, Config.PrivateKey, "1.0",
		    Config.SignType, Config.AlipayPublicKey, Config.CharSet);
		private readonly IHostingEnvironment _hostingEnvironment;
		public FTFPayController(IHostingEnvironment hostingEnvironment)
		{
			_hostingEnvironment = hostingEnvironment;
		}
        public IActionResult Index()
        {
            return View();
        }


	    #region 扫码支付

	    public IActionResult Scan()
	    {
			return View();
		}

		/// <summary>
		/// 生成支付二维码
		/// </summary>
		/// <param name="orderName">订单名称</param>
		/// <param name="orderAmount">订单金额</param>
		/// <param name="outTradeNo">订单号</param>
		/// <returns></returns>
		[HttpGet]
	    public IActionResult ScanCodeGen(string orderName, string orderAmount, string outTradeNo)
	    {

		    AlipayTradePrecreateContentBuilder builder = BuildPrecreateContent(orderName,orderAmount,outTradeNo);

		    //如果需要接收扫码支付异步通知，那么请把下面两行注释代替本行。
		    //推荐使用轮询撤销机制，不推荐使用异步通知,避免单边账问题发生。
		    AlipayF2FPrecreateResult precreateResult = _serviceClient.tradePrecreate(builder);
			//string notify_url = "http://10.5.21.14/Pay/Notify";  //商户接收异步通知的地址
			//AlipayF2FPrecreateResult precreateResult = serviceClient.tradePrecreate(builder, notify_url);

			//以下返回结果的处理供参考。
			//payResponse.QrCode即二维码对于的链接
			//将链接用二维码工具生成二维码打印出来，顾客可以用支付宝钱包扫码支付。
			var bitmap = new Bitmap(Path.Combine(_hostingEnvironment.WebRootPath, "images/error.png"));
			switch (precreateResult.Status)
		    {
			    case ResultEnum.SUCCESS:
				    bitmap.Dispose();
					bitmap=RenderQrCode(precreateResult.response.QrCode);
				    //轮询订单结果
				    //根据业务需要，选择是否新起线程进行轮询
				    ParameterizedThreadStart parStart = new ParameterizedThreadStart(LoopQuery);
				    Thread myThread = new Thread(parStart);
				    object o = precreateResult.response.OutTradeNo;
				    myThread.Start(o);
					break;
			    case ResultEnum.FAILED:
				    Console.WriteLine("生成二维码失败："+ precreateResult.response.Body);
					break;

				case ResultEnum.UNKNOWN:
				    Console.WriteLine("生成二维码失败：" + (precreateResult.response == null ? "配置或网络异常，请检查后重试" : "系统异常，请更新外部订单后重新发起请求"));
					break;
			}
		    MemoryStream ms = new MemoryStream();
		    bitmap.Save(ms, ImageFormat.Png);
		    byte[] bytes = ms.GetBuffer();
		    return File(bytes, "image/png");
		}


		/// <summary>
		/// 构造支付请求数据
		/// </summary>
		/// <param name="orderName">订单名称</param>
		/// <param name="orderAmount">订单金额</param>
		/// <param name="outTradeNo">订单编号</param>
		/// <returns>请求结果集</returns>
		private AlipayTradePrecreateContentBuilder BuildPrecreateContent(string orderName,string orderAmount,string outTradeNo)
	    {
		    //线上联调时，请输入真实的外部订单号。
		    if (string.IsNullOrEmpty(outTradeNo))
		    {
			    outTradeNo = System.DateTime.Now.ToString("yyyyMMddHHmmss") + "0000" + (new Random()).Next(1, 10000).ToString();
		    }

		    AlipayTradePrecreateContentBuilder builder = new AlipayTradePrecreateContentBuilder();
		    //收款账号
		    builder.seller_id = Config.Uid;
		    //订单编号
		    builder.out_trade_no = outTradeNo;
		    //订单总金额
		    builder.total_amount = orderAmount;
		    //参与优惠计算的金额
		    //builder.discountable_amount = "";
		    //不参与优惠计算的金额
		    //builder.undiscountable_amount = "";
		    //订单名称
		    builder.subject = orderName;
		    //自定义超时时间
		    builder.timeout_express = "5m";
		    //订单描述
		    builder.body = "";
		    //门店编号，很重要的参数，可以用作之后的营销
		    builder.store_id = "test store id";
		    //操作员编号，很重要的参数，可以用作之后的营销
		    builder.operator_id = "test";

		    //传入商品信息详情
		    List<GoodsInfo> gList = new List<GoodsInfo>();
		    GoodsInfo goods = new GoodsInfo();
		    goods.goods_id = "goods id";
		    goods.goods_name = "goods name";
		    goods.price = "0.01";
		    goods.quantity = "1";
		    gList.Add(goods);
		    builder.goods_detail = gList;

		    //系统商接入可以填此参数用作返佣
		    //ExtendParams exParam = new ExtendParams();
		    //exParam.sysServiceProviderId = "20880000000000";
		    //builder.extendParams = exParam;

		    return builder;

	    }

		/// <summary>
		/// 渲染二维码
		/// </summary>
		/// <param name="str"></param>
		/// <returns></returns>
		private Bitmap RenderQrCode(string str)
		{
			QRCodeGenerator.ECCLevel eccLevel = QRCodeGenerator.ECCLevel.L;
			using (QRCodeGenerator qrGenerator = new QRCodeGenerator())
			{
				using (QRCodeData qrCodeData = qrGenerator.CreateQrCode(str, eccLevel))
				{
					using (QRCode qrCode = new QRCode(qrCodeData))
					{

						Bitmap bp= qrCode.GetGraphic(20, Color.Black, Color.White,
							new Bitmap(Path.Combine(_hostingEnvironment.WebRootPath, "images/alipay.png")), 15);
						return bp;
					}
				}
			}

		}

	    /// <summary>
	    /// 轮询支付结果
	    /// </summary>
	    /// <param name="o">订单号</param>
	    public void LoopQuery(object o)
	    {
		    AlipayF2FQueryResult queryResult = new AlipayF2FQueryResult();
		    int count = 100;
		    int interval = 10000;
		    string outTradeNo = o.ToString();

		    for (int i = 1; i <= count; i++)
		    {
			    Thread.Sleep(interval);
			    queryResult = _serviceClient.tradeQuery(outTradeNo);
			    if (queryResult?.Status == ResultEnum.SUCCESS)
			    {
				    DoSuccessProcess(queryResult);
				    return;
			    }
		    }
		    DoFailedProcess(queryResult);
	    }

	    /// <summary>
	    /// 请添加支付成功后的处理
	    /// </summary>
	    private void DoSuccessProcess(AlipayF2FQueryResult queryResult)
	    {
		    //支付成功，请更新相应单据
		    Console.WriteLine("扫码支付成功：商户订单号 " + queryResult.response.OutTradeNo);

	    }

	    /// <summary>
	    /// 请添加支付失败后的处理
	    /// </summary>
	    private void DoFailedProcess(AlipayF2FQueryResult queryResult)
	    {
			//支付失败，请更新相应单据
		    Console.WriteLine("扫码支付失败：商户订单号 " + queryResult.response.OutTradeNo);
		}
		#endregion
	}
}
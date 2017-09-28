using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;
using Alipay.AopSdk.Core;
using Alipay.AopSdk.Core.Domain;
using Alipay.AopSdk.Core.Request;
using Alipay.AopSdk.Core.Response;
using Alipay.AopSdk.Core.Util;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Alipay.Demo.PCPayment.Controllers
{
    public class PayController : Controller
    {
	    #region 发起支付

	    public IActionResult Index()
	    {
		    return View();
	    }

		/// <summary>
		/// 发起支付请求
		/// </summary>
		/// <param name="tradeno">外部订单号，商户网站订单系统中唯一的订单号</param>
		/// <param name="subject">订单名称</param>
		/// <param name="totalAmout">付款金额</param>
		/// <param name="itemBody">商品描述</param>
		/// <returns></returns>
		[HttpPost]
	    public void PayRequest(string tradeno,string subject,string totalAmout,string itemBody)
	    {
		    DefaultAopClient client = new DefaultAopClient(Config.Gatewayurl, Config.AppId, Config.PrivateKey, "json", "2.0",
			    Config.SignType, Config.AlipayPublicKey, Config.CharSet, false);

		    // 组装业务参数model
		    AlipayTradePagePayModel model = new AlipayTradePagePayModel();
		    model.Body = itemBody;
		    model.Subject = subject;
		    model.TotalAmount = totalAmout;
		    model.OutTradeNo = tradeno;
		    model.ProductCode = "FAST_INSTANT_TRADE_PAY";

		    AlipayTradePagePayRequest request = new AlipayTradePagePayRequest();
		    // 设置同步回调地址
		    request.SetReturnUrl("http://localhost:5000/Pay/Callback");
		    // 设置异步通知接收地址
		    request.SetNotifyUrl("");
		    // 将业务model载入到request
		    request.SetBizModel(model);

		    AlipayTradePagePayResponse response = null;
		    try
		    {
			    
			    response = client.SdkExecute(request);
			    Console.WriteLine($"订单支付发起成功，订单号：{tradeno}");
			    Response.Redirect(Config.Gatewayurl+"?"+response.Body);

		    }
		    catch (Exception exp)
		    {
			    throw exp;
		    }
	    }

	    #endregion

		#region 支付异步回调通知

		/// <summary>
		/// 支付异步回调通知 需配置域名 因为是支付宝主动post请求这个action 所以要通过域名访问或者公网ip
		/// </summary>
		public async void Notify()
	    {
		    /* 实际验证过程建议商户添加以下校验。
			1、商户需要验证该通知数据中的out_trade_no是否为商户系统中创建的订单号，
			2、判断total_amount是否确实为该订单的实际金额（即商户订单创建时的金额），
			3、校验通知中的seller_id（或者seller_email) 是否为out_trade_no这笔单据的对应的操作方（有的时候，一个商户可能有多个seller_id/seller_email）
			4、验证app_id是否为该商户本身。
			*/
		    Dictionary<string, string> sArray = GetRequestPost();
		    if (sArray.Count != 0)
		    {
			    bool flag = AlipaySignature.RSACheckV1(sArray, Config.AlipayPublicKey,Config.CharSet, Config.SignType, false);
			    if (flag)
			    {
				    //交易状态
				    //判断该笔订单是否在商户网站中已经做过处理
				    //如果没有做过处理，根据订单号（out_trade_no）在商户网站的订单系统中查到该笔订单的详细，并执行商户的业务程序
				    //请务必判断请求时的total_amount与通知时获取的total_fee为一致的
				    //如果有做过处理，不执行商户的业务程序

				    //注意：
				    //退款日期超过可退款期限后（如三个月可退款），支付宝系统发送该交易状态通知
				    Console.WriteLine(Request.Form["trade_status"]);

				    await Response.WriteAsync("success");
			    }
			    else
			    {
				    await Response.WriteAsync("fail");
			    }
		    }
	    }

		    #endregion

		#region 支付同步回调

		/// <summary>
		/// 支付同步回调
		/// </summary>
		[HttpGet]
	    public  IActionResult Callback()
	    {
		    /* 实际验证过程建议商户添加以下校验。
			1、商户需要验证该通知数据中的out_trade_no是否为商户系统中创建的订单号，
			2、判断total_amount是否确实为该订单的实际金额（即商户订单创建时的金额），
			3、校验通知中的seller_id（或者seller_email) 是否为out_trade_no这笔单据的对应的操作方（有的时候，一个商户可能有多个seller_id/seller_email）
			4、验证app_id是否为该商户本身。
			*/
		    Dictionary<string, string> sArray = GetRequestGet();
		    if (sArray.Count != 0)
		    {
			    bool flag = AlipaySignature.RSACheckV1(sArray, Config.AlipayPublicKey, Config.CharSet, Config.SignType, false);
			    if (flag)
			    {
				    Console.WriteLine($"同步验证通过，订单号：{sArray["out_trade_no"]}");
				    ViewData["PayResult"] = "同步验证通过";
			    }
			    else
			    {
				    Console.WriteLine($"同步验证失败，订单号：{sArray["out_trade_no"]}");
				    ViewData["PayResult"] = "同步验证失败";
			    }
		    }
		    return View();
	    }

	    #endregion

	    #region 订单查询

	    [HttpGet]
	    public IActionResult Query()
	    {
		    return View();
	    }

	    [HttpPost]
	    public JsonResult Query(string tradeno, string alipayTradeNo)
	    {
		    DefaultAopClient client = new DefaultAopClient(Config.Gatewayurl, Config.AppId, Config.PrivateKey, "json", "2.0",
			    Config.SignType, Config.AlipayPublicKey, Config.CharSet, false);
		    AlipayTradeQueryModel model = new AlipayTradeQueryModel();
		    model.OutTradeNo = tradeno;
		    model.TradeNo = alipayTradeNo;

		    AlipayTradeQueryRequest request = new AlipayTradeQueryRequest();
		    request.SetBizModel(model);

		    var response = client.Execute(request);
		    return Json(response.Body);
	    }

	    #endregion

		#region 解析请求参数

		private Dictionary<string, string> GetRequestGet()
	    {
		    Dictionary<string, string> sArray = new Dictionary<string, string>();

		    ICollection<string> requestItem = Request.Query.Keys;
		    foreach (var item in requestItem)
		    {
			    sArray.Add(item,Request.Query[item]);

		    }
		    return sArray;

	    }

	    private Dictionary<string, string> GetRequestPost()
	    {
		    Dictionary<string, string> sArray = new Dictionary<string, string>();

		    ICollection<string> requestItem = Request.Form.Keys;
		    foreach (var item in requestItem)
		    {
			    sArray.Add(item, Request.Form[item]);

		    }
		    return sArray;

	    }

	    #endregion

	    #region 订单退款

	    [HttpGet]
	    public IActionResult Refund()
	    {
		    return View();
	    }

	    [HttpPost]
	    public JsonResult Refund(string tradeno,string alipayTradeNo,string refundAmount,string refundReason,string refundNo)
	    {
		    DefaultAopClient client = new DefaultAopClient(Config.Gatewayurl, Config.AppId, Config.PrivateKey, "json", "2.0",
			    Config.SignType, Config.AlipayPublicKey, Config.CharSet, false);

		    AlipayTradeRefundModel model = new AlipayTradeRefundModel();
		    model.OutTradeNo = tradeno;
		    model.TradeNo = alipayTradeNo;
		    model.RefundAmount = refundAmount;
		    model.RefundReason = refundReason;
		    model.OutRequestNo = refundNo;

		    AlipayTradeRefundRequest request = new AlipayTradeRefundRequest();
		    request.SetBizModel(model);

		    var response = client.Execute(request);
		    return Json(response.Body);
	    }

		#endregion

		#region 退款查询

		/// <summary>
		/// 退款查询
		/// </summary>
		/// <returns></returns>
		public IActionResult RefundQuery()
	    {
		    return View();
	    }

	    [HttpPost]
	    public JsonResult RefundQuery(string tradeno,string alipayTradeNo,string refundNo)
	    {
		    DefaultAopClient client = new DefaultAopClient(Config.Gatewayurl, Config.AppId, Config.PrivateKey, "json", "2.0",
			    Config.SignType, Config.AlipayPublicKey, Config.CharSet, false);

		    if (string.IsNullOrEmpty(refundNo))
		    {
			    refundNo = tradeno;
		    }

		    AlipayTradeFastpayRefundQueryModel model = new AlipayTradeFastpayRefundQueryModel();
		    model.OutTradeNo = tradeno;
		    model.TradeNo = alipayTradeNo;
		    model.OutRequestNo = refundNo;

		    AlipayTradeFastpayRefundQueryRequest request = new AlipayTradeFastpayRefundQueryRequest();
		    request.SetBizModel(model);

		    var response = client.Execute(request);
		    return Json(response.Body);
	    }

	    #endregion

	    #region 订单关闭

	    public IActionResult OrderClose()
	    {
		    return View();
	    }

	    [HttpPost]
	    public JsonResult OrderClose(string tradeno, string alipayTradeNo)
	    {
		    DefaultAopClient client = new DefaultAopClient(Config.Gatewayurl, Config.AppId, Config.PrivateKey, "json", "2.0",
			    Config.SignType, Config.AlipayPublicKey, Config.CharSet, false);

		    AlipayTradeCloseModel model = new AlipayTradeCloseModel();
		    model.OutTradeNo = tradeno;
		    model.TradeNo = alipayTradeNo;

		    AlipayTradeCloseRequest request = new AlipayTradeCloseRequest();
		    request.SetBizModel(model);

		    var response = client.Execute(request);
		    return Json(response.Body);
	    }

	    #endregion
	}
}
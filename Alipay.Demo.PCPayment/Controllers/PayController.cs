using System;
using System.Collections.Generic;
using Alipay.AopSdk.AspnetCore;
using Alipay.AopSdk.Core;
using Alipay.AopSdk.Core.Domain;
using Alipay.AopSdk.Core.Request;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Alipay.Demo.PCPayment.Controllers
{
    public class PayController : Controller
    {
		
	    private readonly IAlipayService  _alipayService;

		public PayController(IAlipayService alipayService)
		{
			_alipayService = alipayService;
		}
	    #region 发起支付

	    public IActionResult Index()
	    {
		    return View();
	    }

	    public IActionResult Test()
	    {
			DefaultAopClient client = new DefaultAopClient("https://openapi.alipaydev.com/gateway.do", "2016090800466366", "MIIEogIBAAKCAQEAuCcfh+EJHfMwzXC6b8HfzjYDcH7y79dKAdh5Y5rXOO/4y9stKke+F4Pdp7BOzu6N8/jse7xxCiVLdyUO0L/ck5hOWna//fdQPcSbEyYBUQn+CQy7+uDbsZRniXeeaHJBnK5fot3oHzpHe4dScnvG5NKRJAYsaQW5vD8cVc/m31yuocbyPgqUllblK3h8Lg/nh64bKl8e0Hck1EgGm7+W0+IojM9rcMY1BrdIlUiuP25u8I3zPFH5Tjk4otCvm+xDsukeJiftmDrhtmW3cGtTpNFtr6DK+acfNVccyiaGeCidvLcT3ziO8V7Cy1oQVk+8j5qg83xzCslvF0kZwwrQXwIDAQABAoIBAErfbUx7zI8mz5LX4JWLyAk5oNBebTvi1q5Fa6V50UPPHeMUrBpirQE2liqV3pT4HTMy5EOy7GrpmvNIV+u79lz0MfdKDOmfYjqa80ony6U9YNIhIVTxk6Fx617TCc8BlXe01iGMA9KeiW3KKVVuygFDM8vnzqqsr0XAiy7ApuftP4ujFCJhDZ/PSaKB1/YNJQjz/2GRjqbYT5irre3Mjz7FgyN19CJmSnjshdixp0ItOTQ7QsHYE537gGZhEXs+RDFopF+w4y45kfUUh1IpBxD7jlD1zO3OHXJxBqSiJxz4iO8FS/OYdQUMoGpTtJc6bMnmZcLSwnfV9RTIB381BoECgYEA7+AcpfIpKWgc1zOJRKTjBdKe77LTjto30UlyeskpUofKVRqEYTgALL7POlH16L9gA2R8HvsSEmAPGw7P4ptYO+Kfi3TeiG6RwT5K+JSB8g0xqwvDEVOD3May+nfnaz5sMktWNirSFHbbymfh99mj+4E1drv3cOuDJR6v2qicTyECgYEAxIgcUkcG6CpHagxYfaboyvmglbq/9ZD+ruZSvqG877NT0NLqEPZ6ZOd6AS9m/GsC6pUABtFfXK256oZ7H8bqsfIFTfoUbjiy3fXNuniuvqtLTGL+2kyCazhV15kLtX24wCV6M32Yc1DvGPywAI40UG9D6D2MTCuu7KgO715or38CgYA9p8AVU0oLL4yCL+fvceY8X+ekOrWv+RzxuUwojT4GzYpPF5LBHlDFL4I6PkjTuyTlmlVg7S229WPMk6ERYNZsBhL2GGL+dFUYc3d3r0w7N/L1QP+xm2LAQ35LbLhZ02CiCMUvBRCcW/SgcNUfDJzej1Z7n1K5fn9l8h5HOKF8oQKBgBP7Hp4C2KHsAny2qpyDxrE0Ne5jITcPOcWAZzM4cGQSYFgfyWpWFNWDbzUFo7vQCWjeIzWOPdrHUtqUN5pgd+YFjCKEZWVbYFwOrI7jzChYc/xdKDn7g1rxTFDyH22hTZJDfSwU/dXjiZuJvT8hNhJjbY0EDOqFmnA3GeWePJhzAoGAXioZrM79oG/iN59XuAA5Arl5UjkCsKPa1rfALCSfEoJfvH1UAQ1SbuM0MMIAOGJc/yPdDjyv8Kdy7z867gxo6m0vuLdyJ6qL/KI//mNLJpW30I6/pYsWO1LIIxusxTkv459mvGufn1WH0LHpAf4b84/uq2EDszVat7MEt48NlSI=", "json", "2.0",
			    "RSA2", "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAyWWqIIJ0KKDUPk6fgbxXLxCDqVWTvV4mplocxRsjFPvZC2e4uHs6hq3n8/bc/ZnLcavDTCx56L+TsTKN8w4984pqYPgkYXjWl26hMvVJ2GizjRGw+FeM+1uqiWzrDflX3Cp/o8HKSeFqjA+HXpX+nFg94XJfYIAf8vtHKCusdJE6YRmUZ7lCYH01M59b3KQBeX2q304GwC27xgZ1ZOuqGdngNeeTo0WgTpfKOzgQkUVDNLjVJhxk6i71vzfJ5QUUVIEff1UNlLK1C/kCGDaSY8DGPXni+BfdQekVxdqO1aV5JvaZVW9KrxnXfyAf0FYelBbqym4qzJu98+8eee/oyQIDAQAB", "UTF-8", false);

		    AlipayOpenAuthTokenAppModel model = new AlipayOpenAuthTokenAppModel();

		    //如果使用app_auth_code换取token，则为authorization_code，如果使用refresh_token换取新的token，则为refresh_token
		    model.GrantType = "authorization_code";

		    //与refresh_token二选一，用户对应用授权后得到，即第一步中开发者获取到的app_auth_code值
		    model.Code = "ab64829e4d5b4e2aabd2ab6bae0dcB72";

		    //与code二选一，可为空，刷新令牌时使用
		    model.RefreshToken = "";

		    AlipayOpenAuthTokenAppRequest request = new AlipayOpenAuthTokenAppRequest();

		    request.SetBizModel(model);

		    request.SetReturnUrl("");

		    var response = client.Execute(request);

		    return new JsonResult(new { data = response });

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
//		    DefaultAopClient client = new DefaultAopClient(Config.Gatewayurl, Config.AppId, Config.PrivateKey, "json", "1.0",
//			    Config.SignType, Config.AlipayPublicKey, Config.CharSet, false);

		    // 组装业务参数model
		    AlipayTradePagePayModel model = new AlipayTradePagePayModel
		    {
			    Body = itemBody,
			    Subject = subject,
			    TotalAmount = totalAmout,
			    OutTradeNo = tradeno,
			    ProductCode = "FAST_INSTANT_TRADE_PAY"
		    };

		    AlipayTradePagePayRequest request = new AlipayTradePagePayRequest();
		    // 设置同步回调地址
		    request.SetReturnUrl("http://localhost:5000/Pay/Callback");
		    // 设置异步通知接收地址
		    request.SetNotifyUrl("");
		    // 将业务model载入到request
		    request.SetBizModel(model);

			var response = _alipayService.SdkExecute(request);
		    Console.WriteLine($"订单支付发起成功，订单号：{tradeno}");
			//跳转支付宝支付
		    Response.Redirect(Config.Gatewayurl + "?" + response.Body);
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
			    bool flag = _alipayService.RSACheckV1(sArray);
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
			    bool flag = _alipayService.RSACheckV1(sArray);
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
		    /*DefaultAopClient client = new DefaultAopClient(Config.Gatewayurl, Config.AppId, Config.PrivateKey, "json", "2.0",
			    Config.SignType, Config.AlipayPublicKey, Config.CharSet, false);*/
		    AlipayTradeQueryModel model = new AlipayTradeQueryModel
		    {
			    OutTradeNo = tradeno,
			    TradeNo = alipayTradeNo
		    };

		    AlipayTradeQueryRequest request = new AlipayTradeQueryRequest();
		    request.SetBizModel(model);

		    var response = _alipayService.Execute(request);
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

		/// <summary>
		/// 订单退款
		/// </summary>
		/// <param name="tradeno">商户订单号</param>
		/// <param name="alipayTradeNo">支付宝交易号</param>
		/// <param name="refundAmount">退款金额</param>
		/// <param name="refundReason">退款原因</param>
		/// <param name="refundNo">退款单号</param>
		/// <returns></returns>
		[HttpPost]
	    public JsonResult Refund(string tradeno,string alipayTradeNo,string refundAmount,string refundReason,string refundNo)
	    {
		    /*DefaultAopClient client = new DefaultAopClient(Config.Gatewayurl, Config.AppId, Config.PrivateKey, "json", "2.0",
			    Config.SignType, Config.AlipayPublicKey, Config.CharSet, false);*/

		    AlipayTradeRefundModel model = new AlipayTradeRefundModel();
		    model.OutTradeNo = tradeno;
		    model.TradeNo = alipayTradeNo;
		    model.RefundAmount = refundAmount;
		    model.RefundReason = refundReason;
		    model.OutRequestNo = refundNo;

		    AlipayTradeRefundRequest request = new AlipayTradeRefundRequest();
		    request.SetBizModel(model);

		    var response = _alipayService.Execute(request);
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

		/// <summary>
		/// 退款查询
		/// </summary>
		/// <param name="tradeno">商户订单号</param>
		/// <param name="alipayTradeNo">支付宝交易号</param>
		/// <param name="refundNo">退款单号</param>
		/// <returns></returns>
		[HttpPost]
	    public JsonResult RefundQuery(string tradeno,string alipayTradeNo,string refundNo)
	    {
		    /*DefaultAopClient client = new DefaultAopClient(Config.Gatewayurl, Config.AppId, Config.PrivateKey, "json", "2.0",
			    Config.SignType, Config.AlipayPublicKey, Config.CharSet, false);*/

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

		    var response = _alipayService.Execute(request);
		    return Json(response.Body);
	    }

	    #endregion

	    #region 订单关闭

	    public IActionResult OrderClose()
	    {
		    return View();
	    }

		/// <summary>
		/// 关闭订单
		/// </summary>
		/// <param name="tradeno">商户订单号</param>
		/// <param name="alipayTradeNo">支付宝交易号</param>
		/// <returns></returns>
	    [HttpPost]
	    public JsonResult OrderClose(string tradeno, string alipayTradeNo)
	    {
		    /*DefaultAopClient client = new DefaultAopClient(Config.Gatewayurl, Config.AppId, Config.PrivateKey, "json", "2.0",
			    Config.SignType, Config.AlipayPublicKey, Config.CharSet, false);*/

		    AlipayTradeCloseModel model = new AlipayTradeCloseModel();
		    model.OutTradeNo = tradeno;
		    model.TradeNo = alipayTradeNo;

		    AlipayTradeCloseRequest request = new AlipayTradeCloseRequest();
		    request.SetBizModel(model);

		    var response = _alipayService.Execute(request);
		    return Json(response.Body);
	    }

	    #endregion
	}
}
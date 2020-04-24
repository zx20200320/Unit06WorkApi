using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using System.Web.Http;
using System.Net.Http;
using System.Web.Http.Controllers;

namespace Unit06WorkApi.Models
{
    public class MyAuthizer:AuthorizeAttribute
    {
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            try
            {
                //获取用户身份
                string Auth = actionContext.Request.Headers.GetValues("MyAuth").FirstOrDefault();
                if(Auth!=null)
                { 
                    string user = new JWTHelper().GetPayload(Auth);     //解密
                    if(user!=null)
                    {
                        Console.WriteLine("身份合法，可以继续访问。");
                    }
                    else
                    {
                        actionContext.Response = new HttpResponseMessage(System.Net.HttpStatusCode.OK);
                        var x = new ApiResult<string> { Code=1,Msg="身份不合法，不允许访问。"};
                        actionContext.Response.Content = new StringContent(JsonConvert.SerializeObject(x),System.Text.Encoding.UTF8,"application/json");
                    }
                }
                else
                {
                    actionContext.Response=new HttpResponseMessage(System.Net.HttpStatusCode.OK);
                    var x = new ApiResult<string> {Code=1,Msg= "身份不合法，不允许访问。" };
                    actionContext.Response.Content = new StringContent(JsonConvert.SerializeObject(x),System.Text.Encoding.UTF8,"application/json");
                }
            }
            catch (Exception)
            {
                actionContext.Response = new HttpResponseMessage(System.Net.HttpStatusCode.OK);
                var x = new ApiResult<string> { Code = 1, Msg = "身份不合法，不允许访问。" };
                actionContext.Response.Content = new StringContent(JsonConvert.SerializeObject(x), System.Text.Encoding.UTF8, "application/json");
            }
        }
    }
}
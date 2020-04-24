using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Unit06WorkApi.Models;

namespace Unit06WorkApi.Controllers
{
    public class UsersController : ApiController
    {
        [HttpGet]
         public ApiResult<string> Login(string name,string pass)
        {
            ApiResult<string> result = new ApiResult<string>();
            string str = $"select * from Users where Uname='{name}' and Upwd='{pass}'";
            Users users = DBHelper<Users>.GetData(str).FirstOrDefault();
            if(users!=null)
            {
                Dictionary<string, object> dic = new Dictionary<string, object>();
                dic.Add("Uid",users.Uid);
                dic.Add("Uname",users.Uname);

                JWTHelper helper = new JWTHelper();
                string token = helper.GetToken(dic,1200);

                result.Code = 0;
                result.Msg = "成功";
                result.Result = token;
            }
            else
            {
                result.Code = 1;
                result.Msg = "用户名或密码错误！";
            }
            return result;
        }
        [HttpGet]
        public ApiResult<List<Student>> GetStudents()
        {
            string str = "select * from Student";           
            ApiResult<List<Student>> result = new ApiResult<List<Student>>();
            result.Code = 0;
            result.Result = DBHelper<Student>.GetData(str);
            return result;
        }
    }
}

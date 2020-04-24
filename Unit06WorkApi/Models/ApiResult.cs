using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Unit06WorkApi.Models
{
    public class ApiResult<T>
    {
        public int Code { get; set; }
        public string Msg { get; set; }
        public T Result { get; set; }
    }
}
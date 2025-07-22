using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eMart.Service.Core.Dtos.Common
{
    public class CommonResponse<T>
    {
        public int Code { get; set; }

        public T Data { get; set; }

        public string Message { get; set; }

        public string[] Error { get; set; }
    }
}

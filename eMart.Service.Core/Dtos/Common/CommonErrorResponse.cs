using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eMart.Service.Core.Dtos.Common
{
    public class CommonErrorResponse
    {
        public string Path { get; set; }
        public string Message { get; set; }
        public int Status { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eMart.Service.Core.Dtos.Category
{
    public class CategoryCreateRequestDto
    {
        [Required]
        public string Name { get; set; }
    }
}

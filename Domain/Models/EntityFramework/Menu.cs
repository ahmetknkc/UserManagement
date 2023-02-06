using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.EntityFramework
{
    public class Menu
    {
        [Key]
        public string? ID { get; set; }

        public string? MenuName { get; set; }
        public string? Title { get; set; }
        public string? Content { get; set; }
        public string? AuthorizeRole { get; set; }
    }
}

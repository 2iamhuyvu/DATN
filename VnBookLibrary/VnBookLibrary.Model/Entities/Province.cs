using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace VnBookLibrary.Model.Entities
{
    public class Province
    {
        [Key]
        public int ProvinceId { get; set; }
        public string ProvinceName { get; set; }
        public virtual ICollection<District> Districts { get; set; }
    }
}
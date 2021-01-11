using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Website.Models
{
    public class PhanQuyen
    {
        [Key]
        public int MaPQ { get; set; }
        public string LoaiQuyen { get; set; }
        public ICollection<User> Users { get; set; }

    }
}
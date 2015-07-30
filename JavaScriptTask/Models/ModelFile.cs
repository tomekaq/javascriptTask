using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ServerFile.Models
{
    public class ModelFile
    {
        [Required]
        public string MaxValue { get; set; }
        
        [Required]
        public string FileAmount { get; set; }

    }
}
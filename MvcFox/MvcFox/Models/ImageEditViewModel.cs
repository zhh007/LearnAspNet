using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcFox.Models
{
    public class ImageEditViewModel
    {
        public string ButtonId { get; set; }
        public string JSCallback { get; set; }
        public int ImageWidth { get; set; }
        public int ImageHeight { get; set; }
    }
}
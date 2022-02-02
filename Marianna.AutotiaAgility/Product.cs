using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Marianna.AutotiaAgility
{
    public class Product
    {

        public string Brand { get; set; }
        public string ImgUrl { get; set; }
        public string Index { get; set; }

        public string Extention 
        { 
            get 
            {
                var imgRes = ImgUrl.Split('.');
                return ImgUrl.Split('.')[imgRes.Length - 1];
            }
        }

    }
}

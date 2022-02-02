using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Marianna.RBS
{
    public class PhotoSaver
    {
        private string _photoLinq;
        private string _indexProduct;
        private string _dirName = @"D:\rbs\dodge\";
        public PhotoSaver(string photoLinq, string indexProduct)
        {
            _photoLinq = photoLinq;
            _indexProduct = indexProduct;
        }
        public void PhotoSave()
        {           
                var client = new WebClient();
                var fileName = _dirName + _indexProduct + ".jpg";
                client.DownloadFile(_photoLinq, fileName);
           
        }
    }
}

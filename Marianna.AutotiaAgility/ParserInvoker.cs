using HtmlAgilityPack;
using System;
using System.IO;
using System.Net;

namespace Marianna.AutotiaAgility
{
    public class ParserInvoker
    {
        public void Invoke() // !!!!!!!AHTUNG!!!!!!!!!!!!!!!!!! ЭТО МЕЙН МАРИАННА, МЕЕЕЕЙН!!!!!!!
        {
            Console.WriteLine("Start parser......");
            Parser parser = new Parser();
            parser.GetProduct();
        }
    }

    internal class Parser
    {
        private const string PARS_CATEGORY_NAME = @"D:/autoteile/";
        public void GetProduct()
        {
            var html = "https://www.ws-autoteile.com/en/searchresult.html?manufacturerId=146&manufacturerId=146&pg=1&sort=sales&pp=2358";

            var client = new WebClient();

            HtmlWeb web = new HtmlWeb();




            var htmlDoc = web.Load(html);

            var productCrator = new ProductCreator();

            var products = productCrator.LoadBrand(htmlDoc);
            for (int i = 0; i < products.Count; i++)
            {
                SaveProductToImage(products[i], client);
            }

            // var node = htmlDoc.DocumentNode.SelectNodes("//a[@class='image-fluid-box']/img");






        }

        public void SaveProductToImage(Product product, WebClient client)
        {
            Console.WriteLine($"{product.Index}");
            Console.WriteLine("Saving product img....");
            Directory.CreateDirectory(PARS_CATEGORY_NAME + product.Brand);

            var fileName = PARS_CATEGORY_NAME + product.Brand + @"\" + product.Index + $".{product.Extention}";

            var dirName = PARS_CATEGORY_NAME + product.Brand;
            var count = Directory.GetFiles(dirName, product.Index + '*').Length;

            if (File.Exists(fileName))
            {
                count++;
                fileName = PARS_CATEGORY_NAME + product.Brand + @"\" + product.Index + "_" + count + ".png";
                client.DownloadFile(product.ImgUrl, fileName);
            }
            else
            {
               
                    client.DownloadFile(product.ImgUrl, fileName);
            }


        }
    }
}

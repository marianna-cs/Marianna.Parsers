using HtmlAgilityPack;
using System;
using System.Collections.Generic;

namespace Marianna.AutotiaAgility
{
    class ProductCreator
    {
        private HtmlDocument _html;
        public List<Product> LoadBrand(HtmlDocument _html)
        {

            var brands = _html.DocumentNode.SelectNodes(@".//div[@class='listing-item row']");
            var resultBrand = new List<Product>();

            foreach (HtmlNode brand in brands)
            {
                var html = brand.InnerHtml;
                var nodeDoc = new HtmlDocument();
                nodeDoc.LoadHtml(html);
                var brandNode = nodeDoc.DocumentNode.SelectSingleNode(@".//div[@class='manufacturer-logo']/img");
                var imageNode = nodeDoc.DocumentNode.SelectSingleNode(@"//a[@class='image-fluid-box']/img");
                var indexNode = nodeDoc.DocumentNode.SelectSingleNode(@".//div[@class='item-description']");
                Console.WriteLine($"Creating product......");

                var imgUrl = imageNode.GetAttributeValue("src", "");
                if (string.IsNullOrEmpty(imgUrl))
                {
                    Console.WriteLine("Image URL is empty. Skipping.....");
                    continue;
                }
                resultBrand.Add(new Product()
                {
                    Brand = brandNode.GetAttributeValue("title", ""),
                    ImgUrl = imgUrl,
                    Index = LoadProductIndex(indexNode.InnerText)

                });
            }

            return resultBrand;
        }


        private string LoadProductIndex(string index)
        {


            var res = index.Replace("\n\n", "\n").Split("\n")[3].Split(':')[1].Trim().Replace("/", "").Replace("*", "");
            return res.Split(',')[0].Trim();
        }
    }
}

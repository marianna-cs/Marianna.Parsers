using HtmlAgilityPack;
using System;
using System.Net;

namespace Marianna.ASAM
{
    public class Parser
    {
        public void Pars()
        {
            var html = $"https://www.asamromania.ro/ru/products";

            var client = new WebClient();

            HtmlWeb web = new HtmlWeb();

            var htmlDoc = web.Load(html);

            var cars = htmlDoc.DocumentNode.SelectNodes("//ul[@class='nav nav-pills nav-stacked']/li/a");

            

           
            foreach (var car in cars)
            {
                try
                {
                    #region Pars crs
                    var carLinq = car.GetAttributeValue("href", "not found");

                    var carLoad = web.Load(carLinq);


                    var products = carLoad.DocumentNode.SelectNodes("//div[@class='product-container']");
                    #region Pars products
                    foreach (var product in products)
                    {

                        try
                        {
                            var nodeDoc = new HtmlDocument();

                            nodeDoc.LoadHtml(product.InnerHtml);

                            var index = nodeDoc.DocumentNode.SelectSingleNode("//span[@class='ean-value']").InnerText;

                            var productImage = nodeDoc.DocumentNode.SelectSingleNode("//a[@class='product-img-link']/img").GetAttributeValue("src", "not found");

                            if (productImage.Contains("no-image-available"))
                            {
                                continue;
                            }

                            Console.WriteLine($"{index}");

                            Console.WriteLine("Saving product img....");

                            var imgRes = productImage.Split('.');

                            var extention = productImage.Split('.')[imgRes.Length - 1];

                            var dirName = @"D:\ASAM\";

                            var fileName = dirName + @"\" + index + $".{extention}";

                            client.DownloadFile(productImage, fileName);
                        }
                        catch (WebException we)
                        {
                            if (we.Message.Contains("(404)"))
                            {
                                Console.WriteLine(we.Message);
                                continue;
                            }

                        }
                    }
                    #endregion
                    #endregion
                }
                catch (NullReferenceException exp)
                {
                    Console.WriteLine("У машины нет товаров");
                    Console.WriteLine(exp.Message);
                    continue;

                }
                
            }

        }


    }
}



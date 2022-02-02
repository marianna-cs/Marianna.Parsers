using HtmlAgilityPack;
using System;
using System.IO;
using System.Linq;
using System.Net;

namespace Marianna.AHWShop
{
    public class Parser
    {
        public void Pars()
        {
            for (var i = 1; i < 48; i++)
            {
                Console.WriteLine(i);
                var html = $"https://shop.ahw-shop.de/skoda-teile?p={i.ToString()}&o=3";

                var client = new WebClient();

                HtmlWeb web = new HtmlWeb();

                var htmlDoc = web.Load(html);

                var node = htmlDoc.DocumentNode.SelectNodes("//div[@class='product--info']/a[@class='product--title']");

                foreach (HtmlNode allNodes in node)
                {
                    var url = allNodes.GetAttributeValue("href", "not found");

                    var productPage = web.Load(url);

                    var productIndex = productPage.DocumentNode.SelectNodes("//li[@class='base-info--entry entry--sku']/span");

                    var photoName = productIndex[0].InnerText.Replace("\n", "");

                    var photoLinq = productPage.DocumentNode.SelectNodes("//div[@class='image--box image-slider--item']/span");


                    var nullPhoto = productPage.DocumentNode.SelectNodes("//div[@class='image--box image-slider--item']/span/span").ToList()[0].InnerHtml;

                    if (nullPhoto.Contains("no-picture"))
                    {
                        continue;
                    }
                    

                    foreach(HtmlNode photo in photoLinq)
                    {
                        var photoUrl = photo.GetAttributeValue("data-img-large", "not found");

                        Console.WriteLine($"{photoName}");

                        Console.WriteLine("Saving product img....");

                        var imgRes = photoUrl.Split('.');

                        var extention = photoUrl.Split('.')[imgRes.Length - 1];

                        var dirName = @"D:\AHW\skoda\";

                        var fileName = dirName + @"\" + photoName + $".{extention}";

                        var count = Directory.GetFiles(dirName, photoName.ToString() + '*').Length;

                        if (File.Exists(fileName))
                        {
                            count++;
                            fileName = dirName + photoName + "_" + count + ".png";
                            client.DownloadFile(photoUrl, fileName);
                        }
                        else
                        {

                            client.DownloadFile(photoUrl, fileName);
                        }
                    }
                }


            }

        }
    }
}

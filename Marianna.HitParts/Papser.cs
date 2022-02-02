using HtmlAgilityPack;
using System;
using System.IO;
using System.Net;

namespace Marianna.HitParts
{
    public class Parser
    {
        public void Pars()
        {
            var html = $"https://hitparts.pl/";

            var client = new WebClient();

            HtmlWeb web = new HtmlWeb();

            var htmlDoc = web.Load(html);

            var cars = htmlDoc.DocumentNode.SelectNodes("//div[@class='BoxKategorie BoxZawartosc']/ul/li/div/a");

            foreach (var car in cars)
            {
                try
                {
                    var brand = car.InnerText.Trim();

                    Console.WriteLine(brand);

                    Directory.CreateDirectory(@"D:\HitParts\"+brand);

                    var carLinq = car.GetAttributeValue("href", "not found");

                    carLinq=carLinq + "/producent=3,6,31";

                    var carPage = web.Load(carLinq);

                    if (carPage.Text.Contains("ListingNawigacja LiniaCala"))
                    {
                        var countPage1 = carPage.DocumentNode.SelectNodes("//nav[@class='ListingNawigacja LiniaCala']/div/a").Count;
                        var countPage = carPage.DocumentNode.SelectNodes("//nav[@class='ListingNawigacja LiniaCala']/div/a")[countPage1-1].InnerText.ToString();
                        var maxPage = Convert.ToInt32(countPage);
                        SavePics(maxPage, carLinq, brand);
                    }
                    else
                    {
                        var countPage = 1;
                        SavePics(countPage, carLinq, brand);
                    }
                }
                catch (Exception)
                {

                    continue;
                }
                
                
            }
        }

        public void SavePics(int maxPage, string carLinq, string brand)
        {
            var client = new WebClient();
            HtmlWeb web = new HtmlWeb();
            for (int i = 1; i<=maxPage; i++)
            {
                Console.WriteLine("page "+i);
                var productPage = carLinq + $"/s={i}";

                var loadPage = web.Load(productPage);

                var linqsPhoto = loadPage.DocumentNode.SelectNodes("//div[@class='Foto']/a/img");
                //var detailsPage = loadPage.DocumentNode.SelectNodes("//div[@class='ProdCena']/h3/a");

                foreach (var linqPhoto in linqsPhoto)
                {
                    try
                    {
                        var linq = linqPhoto.GetAttributeValue("data-src-original", "not found").Split("px_")[1];

                        var resLinq = $"https://hitparts.pl/images/"+linq;
                        /*var page = detailPage.GetAttributeValue("href", "not found");

                        var pageProduct = web.Load(page);


                        var index = pageProduct.DocumentNode.SelectSingleNode("//p[@id='NrKatalogowy']/strong").InnerText.Trim();

                        var productImage = pageProduct.DocumentNode.SelectSingleNode("//div[@id='ZdjeciaDuze']/a").GetAttributeValue("href", "");*/

                        Console.WriteLine($"{linq}");

                        Console.WriteLine("Saving product img....");

                        /*var imgRes = resLinq.Split('.');

                        var extention = resLinq.Split('.')[imgRes.Length - 1];*/

                        var dirName = @"D:\HitParts\"+brand+@"\";

                        var fileName = dirName + linq;

                        client.DownloadFile(resLinq, fileName);
                    }
                    catch (Exception)
                    {

                        continue;
                    }
                   
                }
            }
        }
    }
}

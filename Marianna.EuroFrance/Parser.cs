using HtmlAgilityPack;
using OpenQA.Selenium.Chrome;
using System;
using System.Net;
using System.Text.RegularExpressions;

namespace Marianna.EuroFrance
{
    public class Parser
    {

        public void Pars()
        {
            using (var driver = new ChromeDriver(@"C:\Users\User10\source\repos\ParserNewIC\ParserNewIC\bin\Debug\netcoreapp2.1\drivers"))
            {
                var linq = @"https://eurofrance24.com/car-parts/filters/renault.html?p=2&product_list_order=name";

                var getHtml = new SeleniumParser(linq, driver);

                var content = getHtml.GetHtml();

                var nodeDoc = new HtmlDocument();

                nodeDoc.LoadHtml(content);

                var details = nodeDoc.DocumentNode.SelectNodes("//li[@class='item product product-item']");

                var client = new WebClient();

                HtmlWeb web = new HtmlWeb();

                var marka = "peugeot"; //renault //peugeot

                foreach (var detail in details)
                {
                    var nodeDetail = new HtmlDocument();
                    nodeDetail.LoadHtml(detail.InnerHtml);

                    var values = nodeDoc.DocumentNode.SelectSingleNode("//div[@class='product featured-attributes']").InnerText;

                    var value = values.Trim().Replace("\n", "");

                    if (!value.Contains("Numer części:"))
                    {
                        continue;
                    }

                    value = Regex.Replace(value, @"\s+", " ").Split("Numer części:")[1].Split("Numer z katalogu producenta pojazdu")[0].Trim();

                    var imgUrl = nodeDoc.DocumentNode.SelectSingleNode("//img[@class='product-image-photo']").GetAttributeValue("src", "not found");

                    if (imgUrl.Contains("LOGO"))
                    {
                        continue;
                    }

                    imgUrl = imgUrl.Replace("0360c4e5ab9fbc4cdff26aa07e53c342", "e730d56669a6b225923c6f3363621cbc");

                    Console.WriteLine($"{value}");

                    Console.WriteLine("Saving product img....");

                    var imgRes = imgUrl.Split('.');

                    var extention = imgUrl.Split('.')[imgRes.Length - 1];

                    var dirName = @"D:\EuroFrance\" + marka + @"\";

                    var fileName = dirName + value + $".{extention}";

                    client.DownloadFile(imgUrl, fileName);
                }
            }

            

            

        }

    }
}

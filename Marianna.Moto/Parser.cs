using HtmlAgilityPack;
using System.Net;

namespace Marianna.Moto
{
    public class Parser
    {
        private string _dirName = @"D:\MotoDynamic\peuge\";
        public void Pars()
        {
            var client = new WebClient();

            HtmlWeb web = new HtmlWeb();

            var html = $"https://moto-dynamic.com/";


            var car = "https://moto-dynamic.com/products/peugeot,2,21802";

            var carPage = web.Load(car);

            var countPage = carPage.DocumentNode.SelectSingleNode("//div[@class='pager']/span[@class='d']").InnerText.Split("of")[1].Trim();

            for (int j = 149; j <= int.Parse(countPage); j++)
            {
                System.Console.WriteLine(j);

                var htmlPage = car + $"?pageId={j.ToString()}";

                var pageDoc = web.Load(htmlPage);

                var pagesProduct = pageDoc.DocumentNode.SelectNodes("//p[@class='pTxt']/a");

                foreach (var pageProduct in pagesProduct)
                {
                    try
                    {
                        var page = pageProduct.GetAttributeValue("href", "not found");

                        var htmlPageProduct = web.Load($"https://moto-dynamic.com/" + page);

                        var indexProduct = htmlPageProduct.DocumentNode.SelectSingleNode("//div[@class='page-header']/h3").InnerText.Split(":")[1].Trim();

                        var photoLinq = htmlPageProduct.DocumentNode.SelectSingleNode("//div[@id='prodFoto']/a");

                        string imgLinq;

                        if (photoLinq.InnerHtml.Contains("img/large/-1/nophoto.jpg"))
                        {
                            continue;
                        }
                        else
                        {
                            imgLinq=photoLinq.GetAttributeValue("href", "not found");
                        }
                        System.Console.WriteLine(indexProduct);

                        var baseUrl = $"https://moto-dynamic.com";

                        var fileName = _dirName + indexProduct + ".jpg";

                        var prodLinq = baseUrl+ imgLinq;

                        client.DownloadFile(prodLinq, fileName);
                    }
                    catch (WebException)
                    {
                        System.Console.WriteLine("нету картинки...");
                        continue;
                    }
                }



            }



        }
    }
}

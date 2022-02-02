using HtmlAgilityPack;
using System.IO;
using System.Net;

namespace Marianna.Myparto
{
    public class Parser
    {
        private string _dirName = @"D:\Myparto\";
        public void Pars()
        {
            var client = new WebClient();

            HtmlWeb web = new HtmlWeb();

            var html = $"https://www.myparto.com/de/ersatzteile?sort=sales&pg=1&pp=2000&articleManufacturer=OPEL";

            var htmlDoc = web.Load(html);

            var countPage = htmlDoc.DocumentNode.SelectSingleNode("//div[@class='pagechanger-total']").InnerText.Split("von")[1].Trim();

            for (var i = 1; i <= int.Parse(countPage); i++)
            {
                System.Console.WriteLine(i);
                var htmlPage = $"https://www.myparto.com/de/ersatzteile?sort=sales&pg=1&pp=2000&articleManufacturer=OPEL";

                var pageDoc = web.Load(htmlPage);

                var pageProducts = pageDoc.DocumentNode.SelectNodes("//div[@class='listing-item row']");

                foreach (var pageProduct in pageProducts)
                {
                    try
                    {
                        var nodeDoc = new HtmlDocument();

                        nodeDoc.LoadHtml(pageProduct.InnerHtml);

                        var photoLinq = nodeDoc.DocumentNode.SelectSingleNode("//a[@class='image-fluid-box']/img").GetAttributeValue("src", "not found");

                        var brand = nodeDoc.DocumentNode.SelectSingleNode("//meta[@itemprop='brand']").GetAttributeValue("content", "not founf");

                        Directory.CreateDirectory(_dirName + brand);

                        var indexProduct = nodeDoc.DocumentNode.SelectSingleNode("//meta[@itemprop='mpn']").GetAttributeValue("content", "not founf");

                        if (indexProduct.Contains(","))
                        {
                            indexProduct = indexProduct.Trim().Split(",")[0];
                        }

                        System.Console.WriteLine(indexProduct);

                        var fileName = _dirName +brand +"/" + indexProduct + ".jpg";

                        client.DownloadFile(photoLinq, fileName);
                    }
                    catch (WebException)
                    {
                        System.Console.WriteLine("нету картинки...");
                        continue;
                    }
                    catch (System.ArgumentException ex)
                    {
                        System.Console.WriteLine(ex.Message);
                        continue;
                    }
                }
            }









        }
    }
}

using HtmlAgilityPack;
using System.Net;

namespace Marianna.Ucando
{
    public class Parser
    {
        private string _dirName = @"D:\Ucando\POLMO\";
        public void Pars()
        {
            var siteName = "https://www.ucando.pl";

            var client = new WebClient();

            HtmlWeb web = new HtmlWeb();

            var html = $"https://www.ucando.pl/b/polmo/TD-PM-4873?sort=NAME_ASC&page=1";

            var htmlDoc = web.Load(html);

            var countPage = htmlDoc.DocumentNode.SelectSingleNode("//p[@class='c-pagination__pageText']").InnerText.Split(";")[2].Trim();

           for (var i = 1; i <= int.Parse(countPage); i++)
            {
                System.Console.WriteLine(i);
                var htmlPage = $"https://www.ucando.pl/b/polmo/TD-PM-4873?sort=NAME_ASC&page={i.ToString()}";

                var pageDoc = web.Load(htmlPage);

                var pageProducts = pageDoc.DocumentNode.SelectNodes("//div[@class='c-product-item__content']/h3[@class='c-product-item__name']/a");

                foreach (var pageProduct in pageProducts)
                {
                    try
                    {
                        //var nodeDoc = new HtmlDocument();

                        var pageLinq = pageProduct.GetAttributeValue("href", "not found");
                        pageLinq = siteName + pageLinq;
                        var nodeDoc = web.Load(pageLinq);
                       //var page = web.Load(pageLinq);

                        var picLinq = nodeDoc.DocumentNode.SelectSingleNode("//meta[@property='og:image']").GetAttributeValue("content", "not found");

                        var namePic = nodeDoc.DocumentNode.SelectNodes("//span[@class='product-details__value']")[0].InnerText.Trim().Replace("/","");

                        System.Console.WriteLine(namePic);

                        var fileName = _dirName + namePic + ".jpg";

                        client.DownloadFile(picLinq, fileName);
                    }
                    catch (WebException ex)
                    {
                        System.Console.WriteLine(ex.Message);
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

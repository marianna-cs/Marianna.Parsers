using HtmlAgilityPack;
using System;

namespace Marianna.RBS
{
    public class HandlerPages
    {

        public void LoadPage(int countPage, string linqPage)
        {
            var siteName = "https://eshop.rbs-handel.de";

            HtmlWeb web = new HtmlWeb();
            Console.WriteLine(linqPage);
            for (var i = 1; i <= countPage; i++)
            {
                try
                {
                    var page = linqPage + "?page=" + i.ToString();
                    var htmlPage = web.Load(page);
                    var productBlocks = htmlPage.DocumentNode.SelectNodes("//div[@class='col-lg-3 col-md-4 col-sm-6 col-xs-6']");

                    if (productBlocks == null)
                    {
                        continue;
                    }

                    foreach (var productBlock in productBlocks)
                    {
                        try
                        {
                            var nodeDoc = new HtmlDocument();
                            nodeDoc.LoadHtml(productBlock.OuterHtml);
                            var productIndex = nodeDoc.DocumentNode.SelectSingleNode("//a[@class='itemlink center-block']/h6").InnerText.Trim();
                            var photoLinq = nodeDoc.DocumentNode.SelectSingleNode("//img[@class='img-responsive center-block lazy']");
                            if (photoLinq == null)
                            {
                                continue;
                            }
                            var photo = photoLinq.GetAttributeValue("data-src", "notfound").Replace("/310/", "/800/");
                            /*string singlTwoChar = productIndex[0].ToString() + productIndex[1].ToString();
                            var linqPhoto = "https://photos.rbs-handel.de/catalogue/" + singlTwoChar + "/800/" + productIndex + ".jpg";*/
                            Console.WriteLine(productIndex);
                            var photoSaver = new PhotoSaver(photo, productIndex);
                            photoSaver.PhotoSave();
                        }
                        catch (System.NullReferenceException ex)
                        {
                            Console.WriteLine(ex.Message);
                            continue;
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                            continue;
                        }
                    }
                }
                catch(NullReferenceException ex)
                {
                    Console.WriteLine(ex.Message);
                    continue;
                }

            }
        }
    }
}

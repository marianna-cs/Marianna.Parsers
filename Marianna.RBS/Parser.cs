using HtmlAgilityPack;
using System;
using System.Net;

namespace Marianna.RBS
{
    public class Parser
    {
        public void Pars()
        {
            var siteName = "https://eshop.rbs-handel.de";

            var client = new WebClient();

            HtmlWeb web = new HtmlWeb();

            var html = $"https://eshop.rbs-handel.de/en/catalog/dodge/";

            var htmlDoc = web.Load(html);

            var models = htmlDoc.DocumentNode.SelectNodes("//a[@class='my-model-column']");

            for (int i = 0; i<models.Count; i++)
            {
                var model = models[i];
                var linqModel = model.GetAttributeValue("href", "not found");
                var htmlModel = web.Load(siteName+linqModel);
                var engins = htmlModel.DocumentNode.SelectNodes("//td[@class='engine']/a");

                foreach(var engine in engins)
                {
                    var linqEngin = engine.GetAttributeValue("href", "not found");
                    var htmlEngine = web.Load(siteName + linqEngin);
                    var categories = htmlEngine.DocumentNode.SelectNodes("//li[@class='list-group-item']/a");

                    foreach( var category in categories)
                    {
                        var linqCategory = category.GetAttributeValue("href", "not found");
                        if(linqCategory.Contains("#"))
                        {
                            continue;
                        }
                        
                        var htmlCategory=web.Load(siteName+linqCategory);
                        var handlerPages = new HandlerPages();

                        if(htmlCategory.Text.Contains("<ul class='pagination'><li class='active'>"))
                        {
                            var countPage = htmlCategory.DocumentNode.SelectNodes("//ul[@class='pagination']/li").Count - 2;
                            handlerPages.LoadPage(countPage, siteName + linqCategory);
                        }
                        else
                            handlerPages.LoadPage(1, siteName + linqCategory);
                        
                    }
                }
            }
        }
    }
}

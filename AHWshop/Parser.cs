using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace AHWshop
{
    public class Parser
    {
        public void Pars()
        {
            for(var i = 0; i < 108; i++)
            {
                var html = $"https://shop.ahw-shop.de/audi-teile?p={i.ToString()}&o=3";

                var client = new WebClient();

                HtmlWeb web = new HtmlWeb();

                var htmlDoc = web.Load(html);
            }

        }
    }
}

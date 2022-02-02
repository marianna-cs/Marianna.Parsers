using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Text;

namespace Marianna.EuroFrance
{
    public class SeleniumParser
    {
        private string _linq;

        private IWebDriver _driver;

        public SeleniumParser(string linq, IWebDriver driver)
        {
            _linq = linq;
            _driver = driver;
        }
        public string GetHtml()
        {

            _driver.Url = _linq;

            WaitHelpers.WaitUntilElementClickable(_driver, By.ClassName(@"product-item-info"), 40);

            _driver.FindElement(By.XPath(".//select[@class='sorter-options']")).Click();

            _driver.FindElement(By.XPath(".//option[@value='sku']")).Click();

            WaitHelpers.WaitUntilElementClickable(_driver, By.ClassName(@"product-item-info"), 40);

            var content = _driver.PageSource;

            return content;
        }

    }
}

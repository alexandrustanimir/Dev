﻿using FluentSharp.CoreLib;
using NUnit.Framework;

namespace TeamMentor.UnitTests.CoreLib.ExtensionMethods
{
    [TestFixture]
    public class Extra_HtmlAgilityPack_ExtensionMethods
    {
        public string MessyHtml      { get; set; }
        public string ExpectedResult { get; set; }

        public Extra_HtmlAgilityPack_ExtensionMethods()
        {
            MessyHtml      = "<html><ul><li>an item</li></ul></html>";
            ExpectedResult = "<ul>\r\n    <li>an item</li>\r\n  </ul>";
        }
        [Test]
        public void tidyHtml_String()
        {            
            Assert.AreEqual(MessyHtml.tidyHtml(), ExpectedResult);
        }

        [Test]
        public void tidyHtml_HtmlDocument()
        {         
            var htmlDocument = MessyHtml.htmlDocument();

            Assert.NotNull(htmlDocument);
            Assert.NotNull(htmlDocument.tidyHtml(), ExpectedResult);

            Assert.IsNull((null as HtmlAgilityPack.HtmlDocument).tidyHtml(), ExpectedResult);           
        }
    }
}

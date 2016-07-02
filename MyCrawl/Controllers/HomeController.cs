using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mime;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;


namespace MyCrawl.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CrawlProductImage()
        {
            string url = "https://en.yurtbay.com.tr/urunler";
           
            var  links=GetAllProductImages(url);
            int i = 0;
            foreach (var link in links)
            {
                string realLink = "http:" + link;
                WebClient wc= new WebClient();
                wc.DownloadFile(realLink, @"c:\Product Images\product"+i+".jpg");
                i++;
            }
            TempData["Msg"] = @"Product images are Successfully crawled.And Save images in C:\Product Images directory";
            return RedirectToAction("Index");
        }


    public List<string> GetAllProductImages(string baseUrl) {

        // Declaring a new WebClient() method
        WebClient webClient = new WebClient();

        // Setting the URL, then downloading the data from the URL.
        string source = webClient.DownloadString(baseUrl);

        // Declaring 'document' as new HtmlAgilityPack() method
        HtmlAgilityPack.HtmlDocument document = new HtmlAgilityPack.HtmlDocument();

        // Loading document's source via HtmlAgilityPack
        document.LoadHtml(source);

       // For every tag in the HTML containing the node img.
       var urls = document.DocumentNode.Descendants("img")
                                            .Select(e => e.GetAttributeValue("src", null))
                                           .Where(s => !String.IsNullOrEmpty(s));

        List<string> links=new List<string>();
        foreach (var url in urls)
        {
            if (url.Contains("lh3.googleusercontent"))
            {
                links.Add(url);
            }
        }
        return links;
    }

    }
}
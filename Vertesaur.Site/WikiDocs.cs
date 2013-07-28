using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.Xml;
using MarkdownSharp;

namespace Vertesaur.Site
{

    public class WikiDocsItem
    {
        public string RequestSlug { get; set; }
        public string Title { get; set; }
        public HtmlString Body { get; set; }
    }

    public class WikiDocs
    {

        public static WikiDocsItem GetWikiContent(string slug) {
            if (String.IsNullOrWhiteSpace(slug))
                slug = "_Sidebar";

            var directory = new DirectoryInfo(HostingEnvironment.MapPath("~/docs/wiki/Content"));
            var fileName = slug;
            if (!fileName.EndsWith(".md", StringComparison.OrdinalIgnoreCase))
                fileName += ".md";

            var fileInfo = directory.GetFiles(fileName).FirstOrDefault(x => String.Equals(x.Name, fileName, StringComparison.OrdinalIgnoreCase));
            if (fileInfo == null)
                return null;

            var fullMarkdownPath = fileInfo.FullName;
            var markdown = new Markdown();
            var html = markdown.Transform(File.ReadAllText(fullMarkdownPath));

            var response = new WikiDocsItem();
            response.RequestSlug = slug;
            response.Body = new HtmlString(html);
            response.Title = Path.GetFileNameWithoutExtension(fileInfo.Name).Replace('-', ' ');
            if (response.Title == "_Sidebar")
                response.Title = "Documentation";

            return response;
        }

        private readonly Lazy<WikiDocsItem> _sidebarRaw;

        public WikiDocs() {
            _sidebarRaw = new Lazy<WikiDocsItem>(GetSidebar);
        }

        private static WikiDocsItem GetSidebar() {
            return GetWikiContent("_Sidebar");
        }

        private static IList<XmlElement> ExtractTopListItems(WikiDocsItem item) {
            if (item == null)
                return null;

            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml("<root>" + item.Body + "</root>");
            var root = xmlDoc.FirstChild;
            var firstUl = root.ChildNodes
                .OfType<XmlElement>()
                .First(x => String.Equals(x.Name, "UL", StringComparison.OrdinalIgnoreCase));

            return new ReadOnlyCollection<XmlElement>(firstUl.ChildNodes.OfType<XmlElement>().ToArray());
        }

        public IList<XmlElement> ExtractTopListItems() {
            return ExtractTopListItems(_sidebarRaw.Value);
        }

    }
}
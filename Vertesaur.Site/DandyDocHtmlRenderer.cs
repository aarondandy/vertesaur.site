using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using DandyDoc.CRef;
using DandyDoc.CodeDoc;
using ServiceStack.Html;
using ServiceStack.Razor;

namespace Vertesaur.Site
{

    public static class DandyDocHtmlRenderer
    {

        private static readonly HtmlString EmptyHtmlString = new HtmlString(String.Empty);
        private static readonly Regex CodeNameSplitRegex = new Regex(@"(?<=[.,;\<\(\[])");

        public static HtmlString BreakIntoCodeNamePartElements(string name, string tagName = "span") {
            var htmlBuilder = new StringBuilder();
            var openTag = '<' + tagName + '>';
            var closeTag = "</" + tagName + '>';
            foreach (var part in CodeNameSplitRegex.Split(name)) {
                htmlBuilder.Append(openTag);
                htmlBuilder.Append(HttpUtility.HtmlEncode(part));
                htmlBuilder.Append(closeTag);
            }
            return new HtmlString(htmlBuilder.ToString());
        }

        private static string GetLocalMemberUri<T>(this ViewPageBase<T> viewPage, CRefIdentifier cRef) where T : class {
            var cRefAppUri = "~/Docs/Api?cRef=" + Uri.EscapeDataString(cRef.FullCRef);
            return viewPage.Url.Content(cRefAppUri);
        }

        private static string GetMemberUri<T>(this ViewPageBase<T> viewPage, ICodeDocMember member) where T : class {
            Contract.Requires(member != null);

            var uri = member.Uri;
            if (uri != null) {
                if ("HTTP".Equals(uri.Scheme, StringComparison.OrdinalIgnoreCase) || "HTTPS".Equals(uri.Scheme, StringComparison.OrdinalIgnoreCase))
                    return uri.ToString();

                // while it is less efficient the Uri may contain a more accurate linking CRef than the CRef itself due to byref and arrays
                CRefIdentifier cRef;
                if (CRefIdentifier.TryParse(uri, out cRef))
                    return viewPage.GetLocalMemberUri(cRef);

                if (!uri.IsAbsoluteUri)
                    return uri.ToString();
            }

            return viewPage.GetLocalMemberUri(member.CRef);
        }

        public static HtmlString GetMemberTextFull(ICodeDocMember member) {
            Contract.Requires(member != null);
            Contract.Ensures(Contract.Result<HtmlString>() != null);
            var rawText = member.FullName;
            if (member is CodeDocSimpleAssembly) {
                var assemblyFileName = ((CodeDocSimpleAssembly)member).AssemblyFileName;
                if (!String.IsNullOrEmpty(assemblyFileName)) {
                    rawText += " (" + assemblyFileName + ')';
                }
            }
            return new HtmlString(HttpUtility.HtmlEncode(rawText));
        }

        public static HtmlString GetMemberTextShort(ICodeDocMember member) {
            Contract.Requires(member != null);
            Contract.Ensures(Contract.Result<HtmlString>() != null);
            return new HtmlString(HttpUtility.HtmlEncode(member.ShortName));
        }

        public static HtmlString ActionLink<T>(this ViewPageBase<T> viewPage, ICodeDocMember member, HtmlString linkText = null) where T : class {
            Contract.Requires(member != null);
            Contract.Ensures(Contract.Result<HtmlString>() != null);

            var uri = viewPage.GetMemberUri(member);
            if (linkText == null || String.IsNullOrEmpty(linkText.ToString()))
                linkText = GetMemberTextShort(member);

            var linkBuilder = new TagBuilder("a");
            linkBuilder.MergeAttribute("href", uri);
            linkBuilder.InnerHtml = linkText.ToString();
            return new HtmlString(linkBuilder.ToString());
        }

    }

}
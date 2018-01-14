using MACAWeb.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Security.Principal;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace MACAWeb
{
    public static class Auxiliaries
    {
        public static string[] ValidImageTypes = new string[] { "image/gif", "image/jpeg", "image/pjpeg", "image/png" };

        public static string Action(this UrlHelper helper, string actionName, string controllerName, RouteValueDictionary routeValues, string protocol, string hostName, bool defaultPort)
        {
            if (!defaultPort)
            {
                return helper.Action(actionName, controllerName, routeValues, protocol, hostName);
            }

            string port = "80";
            if (protocol.Equals("https", StringComparison.OrdinalIgnoreCase))
            {
                port = "443";
            }

            Uri requestUrl = helper.RequestContext.HttpContext.Request.Url;
            string defaultPortRequestUrl = Regex.Replace(requestUrl.ToString(), @"(?<=:)\d+?(?=/)", port);
            Uri url = new Uri(new Uri(defaultPortRequestUrl, UriKind.Absolute), requestUrl.PathAndQuery);

            var requestContext = GetRequestContext(url);
            var urlHelper = new UrlHelper(requestContext, helper.RouteCollection);

            var values = new RouteValueDictionary(routeValues);
            values.Add("controller", controllerName);
            values.Add("action", actionName);

            return urlHelper.RouteUrl(null, values, protocol, hostName);
        }

        public static string Action(this UrlHelper helper, string actionName, string controllerName, RouteValueDictionary routeValues, string protocol, bool defaultPort)
        {
            return Action(helper, actionName, controllerName, routeValues, protocol, null, defaultPort);
        }

        private static RequestContext GetRequestContext(Uri uri)
        {
            // Create a TextWriter with null stream as a backing stream 
            // which doesn't consume resources
            using (var writer = new StreamWriter(Stream.Null))
            {
                var request = new HttpRequest(
                    filename: string.Empty,
                    url: uri.ToString(),
                    queryString: string.IsNullOrEmpty(uri.Query) ? string.Empty : uri.Query.Substring(1));
                var response = new HttpResponse(writer);
                var httpContext = new HttpContext(request, response);
                var httpContextBase = new HttpContextWrapper(httpContext);
                return new RequestContext(httpContextBase, new RouteData());
            }
        }

        public static void AddEvent(EventDbContext dbEvents, EventTypeDbContext dbEventTypes, string eventTypeCode, string dns, string ip, System.Security.Principal.IIdentity identity)
        {
            // Add event
            /*Event evt = new Event();
            evt.EventID = Guid.NewGuid();
            evt.EventTypeID = dbEventTypes.EventTypes.Where(et => et.Code.Equals(eventTypeCode)).First().EventTypeID;
            evt.Description = "DNS: " + dns + "; IP: " + ip;
            evt.DateCreated = DateTime.Now;
            if (!identity.IsAuthenticated)
                evt.UserCreatedID = null;
            else
                evt.UserCreatedID = new Guid(identity.GetUserId());
            dbEvents.Events.Add(evt);
            dbEvents.SaveChanges();*/
        }

        /*
         * Returns the Curent user Id as a Guid. 
         */
        public static Guid GetUserId(IPrincipal user)
        {
            return Guid.Parse(user.Identity.GetUserId());
        }
    }

    public class MemoryPostedFile : HttpPostedFileBase
    {
        private readonly byte[] fileBytes;

        public MemoryPostedFile(byte[] fileBytes, string fileName = null)
        {
            this.fileBytes = fileBytes;
            this.FileName = fileName;
        }

        public override int ContentLength => fileBytes.Length;

        public override string FileName { get; }        
    }

}
﻿using MACAWeb.Models;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Reflection;
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

        /*public static void AddEvent(MACADbContext dbEvents, MACADbContext dbEventTypes, string eventTypeCode, string dns, string ip, System.Security.Principal.IIdentity identity)
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
            dbEvents.SaveChanges();
        }

        
         * Returns the Curent user Id as a Guid. 
         */
        public static Guid GetUserId(IPrincipal user)
        {
            return Guid.Parse(user.Identity.GetUserId());
        }

        public static Guid GetConnectedPerson(string LoggedUser)
        {
            Guid PersonID = new Guid();
            MACADbContext db = new MACADbContext();
            var selectedUser = db.PersonUsers.Where(x => x.UserID == LoggedUser).FirstOrDefault()?.PersonID.ToString();
            if (selectedUser != null) { PersonID = Guid.Parse(selectedUser); }
            return PersonID;
        }
        public static string GetUserAcc(string UserID)
        {
            MACADbContext db = new MACADbContext();
            ApplicationDbContext dbApplication = new ApplicationDbContext();
            string selectedUser = db.PersonUsers.Where(x => x.UserID == UserID).FirstOrDefault()?.UserID.ToString();
            if (selectedUser != null)
            {
                ApplicationUser user = dbApplication.Users.Where(u => u.Id.Equals(selectedUser)).FirstOrDefault();
                return user.UserName;
            }
            else
            {
                return null;
            }
            
        }

        public static byte[] CreateThumbnail(byte[] image,int thumbWidth)
        {
            MemoryStream msImage = new MemoryStream(image);
            Image fullsizeImage = Image.FromStream(msImage);

            if (thumbWidth == 0)
            {
                thumbWidth = int.Parse(ConfigurationManager.AppSettings["thumbnailWidth"]);
            }
            int thumbHeight = (int)(fullsizeImage.Height * thumbWidth / (double)fullsizeImage.Width);

            var thumbnailBitmap = new Bitmap(thumbWidth, thumbHeight);
            Graphics thumbnailGraph = Graphics.FromImage(thumbnailBitmap);
            thumbnailGraph.CompositingQuality = CompositingQuality.HighQuality;
            thumbnailGraph.SmoothingMode = SmoothingMode.HighQuality;
            thumbnailGraph.InterpolationMode = InterpolationMode.HighQualityBicubic;

            var imageRectangle = new Rectangle(0, 0, thumbWidth, thumbHeight);
            thumbnailGraph.DrawImage(fullsizeImage, imageRectangle);

            MemoryStream msThumb = new MemoryStream();
            thumbnailBitmap.Save(msThumb, fullsizeImage.RawFormat);

            fullsizeImage.Dispose();
            thumbnailGraph.Dispose();

            return msThumb.ToArray();
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
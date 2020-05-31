using System;
using System.Reflection;
using Flannounce.Configuration;
using Flannounce.Domain.Filter;
using Flannounce.Domain.Services.Implementation;
using Microsoft.AspNetCore.WebUtilities;

namespace Flannounce.Domain.Services
{
    public class UriService : IUriService
    {
        private readonly string _baseUri;
        
        public UriService(string baseUri)
        {
            _baseUri = baseUri;
        }
        
        public Uri GetAnnounceUri(string announceId)
        {
            return new Uri(_baseUri + ApiRoutes.Announce.Get.Replace("{announceId}", announceId));
        }

        public Uri GetAllAnnouncesUri(string path = "", PaginationQuery pagination = null, GetAllAnnouncesFilter announcesFilter = null)
        {
            var uriWithPath = $"{_baseUri}{path}/";
            
            var uri = new Uri(uriWithPath);

            if (pagination == null)
            {
                return uri;
            }


            var modifiedUri = QueryHelpers.AddQueryString(uriWithPath, "pageNumber", pagination.PageNumber.ToString());
            modifiedUri = QueryHelpers.AddQueryString(modifiedUri, "pageSize", pagination.PageSize.ToString());
            AddFilterParameters(announcesFilter, ref modifiedUri);
            return new Uri(modifiedUri);
        }

        private static void AddFilterParameters(GetAllAnnouncesFilter announcesFilter, ref string modifiedUri)
        {
            PropertyInfo[] properties = typeof(GetAllAnnouncesFilter).GetProperties();
            foreach (PropertyInfo property in properties)
            {
                var propValue = property.GetValue(announcesFilter);
                if (propValue != null)
                {
                    modifiedUri = QueryHelpers.AddQueryString(modifiedUri, property.Name, propValue.ToString());
                }
            }
        }
    }
}
using System;
using Flannounce.Configuration;
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

        public Uri GetAllAnnouncesUri( string path = "", PaginationQuery pagination = null)
        {
            var uriWithPath = $"{_baseUri}{path}/";
            
            var uri = new Uri(uriWithPath);

            if (pagination == null)
            {
                return uri;
            }

            var modifiedUri = QueryHelpers.AddQueryString(uriWithPath, "pageNumber", pagination.PageNumber.ToString());
            modifiedUri = QueryHelpers.AddQueryString(modifiedUri, "pageSize", pagination.PageSize.ToString());
            
            return new Uri(modifiedUri);
        }
    }
}
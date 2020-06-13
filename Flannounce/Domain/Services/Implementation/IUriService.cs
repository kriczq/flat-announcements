using System;
using Flannounce.Domain.Filter;

namespace Flannounce.Domain.Services.Implementation
{
    public interface IUriService
    {
        Uri GetAnnounceUri(string announceId);

        Uri GetAllAnnouncesUri(string path = "", PaginationQuery pagination = null, GetAllAnnouncesFilter announcesFilter = null);
    }
}
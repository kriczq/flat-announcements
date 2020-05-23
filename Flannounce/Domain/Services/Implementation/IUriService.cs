using System;

namespace Flannounce.Domain.Services.Implementation
{
    public interface IUriService
    {
        Uri GetAnnounceUri(string announceId);

        Uri GetAllAnnouncesUri(string path = "", PaginationQuery pagination = null);
    }
}
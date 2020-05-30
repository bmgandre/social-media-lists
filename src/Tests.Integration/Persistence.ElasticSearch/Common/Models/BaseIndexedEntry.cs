using System;

namespace SocialMediaLists.Tests.Integration.Persistence.ElasticSearch.Common.Models
{
    internal class BaseIndexedEntry<T>
    {
        public Guid id { get; set; }
        public T source { get; set; }
    }
}
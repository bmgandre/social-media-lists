using Nest;
using SocialMediaLists.Application.Contracts.Posts.Models;
using SocialMediaLists.Domain.Posts;

namespace SocialMediaLists.Persistence.ElasticSearch.Posts.Repositories
{
    internal static class QueryContainerExtensions
    {
        public static QueryContainer CreateQuery(this PostSearchRequest filter)
        {
            return new QueryContainer()
                .AddTextualFilterIfNotEmpty(filter)
                .AddNetworkIfNotEmpty(filter)
                .AddDateIfAvailable(filter);
        }

        private static QueryContainer AddTextualFilterIfNotEmpty(this QueryContainer query, PostSearchRequest filter)
        {
            if (!string.IsNullOrWhiteSpace(filter.Text))
            {
                return query && Query<Post>.Match(p => p.Field(f => f.Content).Query(filter.Text));
            }

            return query;
        }

        private static QueryContainer AddNetworkIfNotEmpty(this QueryContainer query, PostSearchRequest filter)
        {
            if (!string.IsNullOrWhiteSpace(filter.Network))
            {
                return query && Query<Post>.Term(p => p.Network, filter.Network);
            }

            return query;
        }

        private static QueryContainer AddDateIfAvailable(this QueryContainer query, PostSearchRequest filter)
        {
            return query.AddBeginingDateIfAvailable(filter).AddEndingDateIfAvailable(filter);
        }

        private static QueryContainer AddBeginingDateIfAvailable(this QueryContainer query, PostSearchRequest filter)
        {
            if (filter.DateRange?.Begin.HasValue == true)
            {
                return query && Query<Post>.DateRange(c => c.Field(p => p.Date)
                    .GreaterThanOrEquals(filter.DateRange?.Begin.Value)
                );
            }

            return query;
        }

        private static QueryContainer AddEndingDateIfAvailable(this QueryContainer query, PostSearchRequest filter)
        {
            if (filter.DateRange?.End.HasValue == true)
            {
                return query && Query<Post>.DateRange(c => c.Field(p => p.Date)
                    .LessThanOrEquals(filter.DateRange?.End.Value)
                );
            }

            return query;
        }
    }
}
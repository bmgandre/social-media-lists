using Nest;
using SocialMediaLists.Application.Contracts.Posts.Models;
using SocialMediaLists.Domain;

namespace SocialMediaLists.Persistence.ElasticSearch.Posts.Repositories
{
    public static class QueryContainerExtensions
    {
        public static QueryContainer CreateQuery(this PostFilter filter)
        {
            return new QueryContainer()
                .AddTextualFilterIfNotEmpty(filter)
                .AddNetworkIfNotEmpty(filter)
                .AddDateIfAvailable(filter);
        }

        private static QueryContainer AddTextualFilterIfNotEmpty(this QueryContainer query, PostFilter filter)
        {
            if (!string.IsNullOrWhiteSpace(filter.Text))
            {
                return query && Query<Post>.Match(p => p.Field(f => f.Content).Query(filter.Text));
            }

            return query;
        }

        private static QueryContainer AddNetworkIfNotEmpty(this QueryContainer query, PostFilter filter)
        {
            if (!string.IsNullOrWhiteSpace(filter.Network))
            {
                return query && Query<Post>.Term(p => p.Network, filter.Network);
            }

            return query;
        }

        private static QueryContainer AddDateIfAvailable(this QueryContainer query, PostFilter filter)
        {
            return query.AddBeginingDateIfAvailable(filter).AddEndingDateIfAvailable(filter);
        }

        private static QueryContainer AddBeginingDateIfAvailable(this QueryContainer query, PostFilter filter)
        {
            if (filter.DateRange?.Begin.HasValue == true)
            {
                return query && Query<Post>.DateRange(c => c.Field(p => p.Date)
                    .GreaterThanOrEquals(filter.DateRange?.Begin.Value)
                );
            }

            return query;
        }

        private static QueryContainer AddEndingDateIfAvailable(this QueryContainer query, PostFilter filter)
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
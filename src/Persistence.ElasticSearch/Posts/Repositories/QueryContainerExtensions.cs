using Nest;
using SocialMediaLists.Application.Contracts.Posts.Models;
using SocialMediaLists.Domain.Posts;
using System.Linq;

namespace SocialMediaLists.Persistence.ElasticSearch.Posts.Repositories
{
    internal static class QueryContainerExtensions
    {
        public static QueryContainer CreateQuery(this PostFilter filter)
        {
            return Query<Post>.MatchAll()
                .AddTextualFilterIfNotEmpty(filter)
                .AddNetworkIfNotEmpty(filter)
                .AddAuthorsIfNotEmpty(filter)
                .AddListsIfNotEmpty(filter)
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

        private static QueryContainer AddAuthorsIfNotEmpty(this QueryContainer query, PostFilter postFilter)
        {
            if (postFilter.Authors != null && postFilter.Authors.Count() > 0)
            {
                return query && Query<Post>.Terms(t => t.Field(f => f.Author).Terms(postFilter.Authors));
            }

            return query;
        }

        private static QueryContainer AddListsIfNotEmpty(this QueryContainer query, PostFilter postFilter)
        {
            if (postFilter.Authors != null && postFilter.Authors.Count() > 0)
            {
                return query && Query<Post>.Terms(t => t.Field(f => f.Lists).Terms(postFilter.Lists));
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
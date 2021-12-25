using CarsCrawler.Domain.Model;

namespace CarsCrawler.Domain.Events
{
    public class SearchStartedDomainEvent
    {
        private SearchModel Search { get; }

        public SearchStartedDomainEvent(SearchModel search)
        {
            this.Search = search;
        }
    }
}
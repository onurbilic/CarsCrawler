using CarsCrawler.Domain.Model;
using MassTransit.Topology;

namespace CarsCrawler.SharedBusiness.Commands
{
    [EntityName(Domain.Model.Consts.SearchCarsCommand)]
    public interface ISearchCarsCommand 
    {
        
    }
    public class SearchCarsCommand : SearchModel, ISearchCarsCommand
    {
        
    }
}
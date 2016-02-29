using System.Collections.Generic;

namespace DependencyAnalyser.Services
{
    public interface IComponentsService
    {
        IEnumerable<Models.Component> GetList();
        Models.Component GetItem(int id);
        Models.Component Add(Models.Component component);
        Models.Component Update(Models.Component component);
        bool Delete(Models.Component component);
    }
}
using System.Collections.Generic;
using DependencySummary.Models;

namespace DependencySummary.Services
{
    public interface IComponentService
    {
        IEnumerable<Component> GetList();
        Component GetItem(int id);
        Component GetItem(string teamCityBuildId);
        bool Update(ViewModels.Component component);
        int Add(ViewModels.Component component);
        void Delete(int id);
    }
}
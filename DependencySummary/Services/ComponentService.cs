﻿using System.Collections.Generic;
using System.Linq;
using DependencySummary.Models;

namespace DependencySummary.Services
{
    public class ComponentService : IComponentService
    {
        public IEnumerable<Component> GetList()
        {
            using (var db = new PackageContext())
            {
                return db.Components.ToList();
            }
        }

        public Component GetItem(int id)
        {
            using (var db = new PackageContext())
            {
                return db.Components.SingleOrDefault(c => c.Id == id);
            }
        }

        public Component GetItem(string teamCityBuildId)
        {
            using (var db = new PackageContext())
            {
                return db.Components.SingleOrDefault(c => c.TeamCityBuildId == teamCityBuildId);
            }
        }

        public bool Update(ViewModels.Component component)
        {
            using (var db = new PackageContext())
            {
                var existingComponent = db.Components.SingleOrDefault(c => c.Id == component.Id);

                if (existingComponent == null)
                {
                    return false;
                }

                existingComponent.Name = component.Name;
                existingComponent.TeamCityBuildId = component.TeamCityBuildId;

                db.SaveChanges();

                return true;
            }
        }

        public int Add(ViewModels.Component component)
        {
            using (var db = new PackageContext())
            {
                var newComponent = new Component
                {
                    Name = component.Name,
                    TeamCityBuildId = component.TeamCityBuildId
                };

                db.Components.Add(newComponent);

                db.SaveChanges();

                return newComponent.Id;
            }
        }

        public void Delete(int id)
        {
            using (var db = new PackageContext())
            {
                db.Components.Remove(db.Components.Single(c => c.Id == id));

                db.SaveChanges();
            }
        }
    }
}
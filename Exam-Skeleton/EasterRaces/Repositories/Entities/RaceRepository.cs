using EasterRaces.Models.Races.Contracts;
using EasterRaces.Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;

namespace EasterRaces.Repositories.Entities
{
    public class RaceRepository : IRepository<IRace>
    {
        private readonly List<IRace> races;

        public RaceRepository()
        {
            races = new List<IRace>();
        }
        public void Add(IRace model)
        {
            races.Add(model);
        }

        public IReadOnlyCollection<IRace> GetAll()
        {
            return races.ToArray();
        }

        public IRace GetByName(string name)
        {
            return races.FirstOrDefault(r => r.Name == name);
        }

        public bool Remove(IRace model)
        {
            return races.Remove(model);
        }
    }


}

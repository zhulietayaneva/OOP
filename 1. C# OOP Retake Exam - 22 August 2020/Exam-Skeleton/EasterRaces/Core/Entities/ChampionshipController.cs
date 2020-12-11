using EasterRaces.Core.Contracts;
using EasterRaces.Models.Cars.Contracts;
using EasterRaces.Models.Cars.Entities;
using EasterRaces.Models.Drivers.Contracts;
using EasterRaces.Models.Drivers.Entities;
using EasterRaces.Models.Races.Contracts;
using EasterRaces.Models.Races.Entities;
using EasterRaces.Repositories.Contracts;
using EasterRaces.Repositories.Entities;
using EasterRaces.Utilities.Messages;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace EasterRaces.Core.Entities
{
    public class ChampionshipController : IChampionshipController
    {
        private readonly IRepository<IDriver> driverRep;
        private readonly IRepository<ICar> carRep;
        private readonly IRepository<IRace> raceRep;

        public ChampionshipController()
        {
            this.driverRep = new DriverRepository();
            this.carRep = new CarRepository();
            this.raceRep = new RaceRepository();
        }

        public string AddCarToDriver(string driverName, string carModel)
        {
            if (driverRep.GetByName(driverName)==null)
            {
                throw new InvalidOperationException(String.Format(ExceptionMessages.DriverNotFound, driverName));
            }

            if (carRep.GetByName(carModel) == null)
            {
                throw new InvalidOperationException(String.Format(ExceptionMessages.CarNotFound, carModel));
            }

            var car = carRep.GetByName(carModel);
            var driver = driverRep.GetByName(driverName);
            driver.AddCar(car);
            return String.Format(OutputMessages.CarAdded, driverName, carModel);
        }

        public string AddDriverToRace(string raceName, string driverName)
        {
            if (driverRep.GetByName(driverName) == null)
            {
                throw new InvalidOperationException(String.Format(ExceptionMessages.DriverNotFound, driverName));
            }

            if (raceRep.GetByName(raceName) == null)
            {
                throw new InvalidOperationException(String.Format(ExceptionMessages.RaceNotFound, raceName));
            }

            var race = raceRep.GetByName(raceName);
            var driver = driverRep.GetByName(driverName);
            race.AddDriver(driver);
            return String.Format(OutputMessages.DriverAdded, driverName, raceName);
        }

        public string CreateCar(string type, string model, int horsePower)
        {
            ICar car = null;
            switch (type)
            {
                case "Sports":
                    car = new SportsCar(model, horsePower);
                    break;
                case "Muscle":
                    car = new MuscleCar(model, horsePower);
                    break;             
            }

            if (carRep.GetByName(model)!=null)
            {
                throw new ArgumentException(String.Format(ExceptionMessages.CarExists, model));
            }
            carRep.Add(car);
            return String.Format(OutputMessages.CarCreated,car.GetType().Name, model);

        }

        public string CreateDriver(string driverName)
        {
            IDriver driver = new Driver(driverName);

            if (driverRep.GetByName(driverName) != null)
            {
                throw new ArgumentException(String.Format(ExceptionMessages.DriversExists,driverName));
            }
            driverRep.Add(driver);
            
            return String.Format(OutputMessages.DriverCreated, driverName);
        }

        public string CreateRace(string name, int laps)
        {
            IRace race = new Race(name, laps);

            if (raceRep.GetByName(name)!=null)
            {
                throw new InvalidOperationException(String.Format(ExceptionMessages.RaceExists,name));
            }
            raceRep.Add(race);
            return String.Format(OutputMessages.RaceCreated, name);
        }

        public string StartRace(string raceName)
        {
            var raceCurr = raceRep.GetByName(raceName);

            if (raceRep.GetByName(raceName)==null)
            {
                throw new InvalidOperationException(String.Format(ExceptionMessages.RaceNotFound, raceName));
            }

            if (raceCurr.Drivers.Count<3)
            {
                throw new InvalidOperationException(String.Format(ExceptionMessages.RaceInvalid, raceName, 3));

            }

            var raceToPrint = raceCurr.Drivers.OrderByDescending(d => d.Car.CalculateRacePoints(raceCurr.Laps)).ToList();

            StringBuilder sb = new StringBuilder();
            sb.Append($"Driver {raceToPrint[0].Name} wins { raceName} race."+Environment.NewLine);
            sb.Append($"Driver {raceToPrint[1].Name} is second in { raceName} race."+Environment.NewLine);
            sb.Append($"Driver {raceToPrint[2].Name} is third in { raceName} race."+Environment.NewLine);

            return sb.ToString().Trim();
        }
    }
}

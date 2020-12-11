using EasterRaces.Models.Cars.Contracts;
using EasterRaces.Models.Drivers.Contracts;
using EasterRaces.Utilities.Messages;
using System;
using System.ComponentModel.DataAnnotations;

namespace EasterRaces.Models.Drivers.Entities
{
    public class Driver : IDriver
    {
        private string name;

        public Driver(string name)
        {
            Name = name;
            CanParticipate = false;
        }
        public string Name
        {
            get { return name; }
           private set
            {
                if (String.IsNullOrEmpty(value) || value.Length<5)
                {
                    throw new ArgumentException(String.Format(ExceptionMessages.InvalidName, value, 5));
                }
                name = value;
            }
        }

        public ICar Car { get; private set; }

        public int NumberOfWins { get; private set; }

        public bool CanParticipate 
        { 
            get
            {
                return Car != null ? true : false;
            }
            private set { }
        }

        public void AddCar(ICar car)
        {
            if (car!=null)
            {
                Car = car;
            }
            else
            {
                throw new ArgumentNullException(ExceptionMessages.CarInvalid);
            }
        }

        public void WinRace()
        {
            NumberOfWins++;
        }
    }
}

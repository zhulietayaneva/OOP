using EasterRaces.Models.Cars.Contracts;
using EasterRaces.Utilities.Messages;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net.Http.Headers;
using System.Text;

namespace EasterRaces.Models.Cars.Entities
{
    public abstract class Car : ICar
    {
        private string model;
        private int horsepower;
      
        private int MinHorsePower;
        private int MaxHorsePower;

        protected Car(string model, int horsePower, double cubicCentimeters, int minHorsePower, int maxHorsePower)
        { 
            MinHorsePower = minHorsePower;
            MaxHorsePower = maxHorsePower;
            Model = model;
            HorsePower = horsePower;
            CubicCentimeters = cubicCentimeters;
           

        }
        public string Model
        {
            get { return this.model; }

            private set
            {
                if (String.IsNullOrWhiteSpace(value) || value.Length<4)
                {
                    throw new ArgumentException(String.Format(ExceptionMessages.InvalidModel,value,4));
                }
                model = value;
            }

        }

        public  int HorsePower
        {
            get { return this.horsepower; }
            private set
            {
                if (value<MinHorsePower || value >MaxHorsePower)
                {
                    throw new ArgumentException(String.Format(ExceptionMessages.InvalidHorsePower,value));
                }
                horsepower = value;
            }
        }

        public double CubicCentimeters
        {
            get;
        }


        public double CalculateRacePoints(int laps)
        {
            return CubicCentimeters / HorsePower * laps;
        }
    }
}

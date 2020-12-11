using CounterStrike.Core.Contracts;
using CounterStrike.Models.Guns;
using CounterStrike.Models.Guns.Contracts;
using CounterStrike.Models.Maps;
using CounterStrike.Models.Maps.Contracts;
using CounterStrike.Models.Players;
using CounterStrike.Models.Players.Contracts;
using CounterStrike.Repositories;
using CounterStrike.Utilities.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CounterStrike.Core
{
    public class Controller : IController
    {
        private GunRepository gunRepository;
        private PlayerRepository playerRepository;
        private IMap map;

        
        public Controller()
        {
            gunRepository = new GunRepository();
            playerRepository = new PlayerRepository();
            map = new Map();

        }
        public string AddGun(string type, string name, int bulletsCount)
        {
            IGun gun = null;

            switch (type)
            {
                case "Pistol":
                    gun = new Pistol(name, bulletsCount);
                    break;
                case "Rifle":
                    gun = new Rifle(name, bulletsCount);
                    break;
                default:
                    throw new ArgumentException(ExceptionMessages.InvalidGunType);
                    // exc message-a se razlichava s tova ot faila
                    
            }

            
            gunRepository.Add(gun);
            return String.Format(OutputMessages.SuccessfullyAddedGun, name);

            
        }

        public string AddPlayer(string type, string username, int health, int armor, string gunName)
        {
            IPlayer Player = null;
            IGun gun = null;

            gun = gunRepository.FindByName(gunName);
            if (gun==null)
            {
                throw new ArgumentException(ExceptionMessages.GunCannotBeFound);
            }

            switch (type)
            {
                case "Terrorist":
                    Player = new Terrorist(username,health,armor,gun);
                    break;
                case "CounterTerrorist":
                    Player = new CounterTerrorist(username, health, armor, gun);
                    break;
                default:
                    throw new ArgumentException(ExceptionMessages.InvalidPlayerType);
                    // exc message-a se razlichava s tova ot faila

            }

            playerRepository.Add(Player);
            return String.Format(OutputMessages.SuccessfullyAddedPlayer, username);


        }

        public string Report()
        {
            var toPrint = playerRepository.Models.OrderBy(p=>p.GetType().Name).ThenByDescending(p => p.Health).ThenBy(p => p.Username);
          

            StringBuilder sb = new StringBuilder();

            foreach (var pl in toPrint)
            {
                sb.Append($"{pl.GetType().Name}: {pl.Username}" + Environment.NewLine);
                sb.Append($"--Health: {pl.Health}"+ Environment.NewLine);
                sb.Append($"--Armor: {pl.Armor}"+ Environment.NewLine);
                sb.Append($"--Gun: {pl.Gun.Name}"+ Environment.NewLine);
                
            }
            return sb.ToString().TrimEnd();
        }

        public string StartGame()
        {

            return map.Start(playerRepository.Models.ToList());
        }
    }
}

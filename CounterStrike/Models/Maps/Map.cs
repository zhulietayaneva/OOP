using CounterStrike.Models.Maps.Contracts;
using CounterStrike.Models.Players.Contracts;
using CounterStrike.Utilities.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CounterStrike.Models.Maps
{
    public class Map : IMap
    {
        public string Start(ICollection<IPlayer> players)
        {
            List<IPlayer> counter = new List<IPlayer>();
            List<IPlayer> terr = new List<IPlayer>();
            foreach (var player in players)
            {
                if (player.GetType().Name == "Terrorist")
                {
                    terr.Add(player);
                }
                else
                {
                    counter.Add(player);
                }
            }
           
            while (terr.Any(p => p.IsAlive) && counter.Any(p => p.IsAlive))
            {
               
                AttackTeam(terr, counter);
                AttackTeam(counter, terr);
                              

            }
                       

            if (counter.Sum(p=>p.Health) >terr.Sum(p=>p.Health))
            {
                return "Counter Terrorist wins!";
            }
            return "Terrorist wins!";
        }


        private void AttackTeam(List<IPlayer> attTeam, List<IPlayer> deffTeam)
        {
            foreach (var att in attTeam)
            {
                if (att.IsAlive )
                {
                    foreach (var def in deffTeam)
                    {
                        if(def.IsAlive) def.TakeDamage(att.Gun.Fire());
                      
                    }

                }

            }


        }




    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace MyAirport.Pim.Models
{
    public static class Factory
    {
        private static AbstractDefinition singleton = null;
        public static AbstractDefinition Model
        {
            get
            {
                if (singleton == null)
                {
                    switch (ConfigurationManager.AppSettings["Factory"])
                    {
                        case "Sql":
                             singleton = new MyAirport.Pim.Models.Sql();
                            break;
                        case "Natif":
                            singleton = new MyAirport.Pim.Models.Natif();
                            break;
                        default:
                            singleton = new MyAirport.Pim.Models.Natif();
                            break;
                    }
                }
                return singleton;
            }
        }
    }
}

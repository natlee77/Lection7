using System;
using System.Collections.Generic;
using System.Text;

namespace SharedLibrary.Models
{
    public class TemperatureModel
    {
        public double Temperature { get; set; }

        public double Humidity { get; set; }

        public DateTime Timestamp { get; set; } //när temp  Datum


        // att för enclar mer : skapa 2 ctor
        public TemperatureModel()
        {
                  
        }

        //model på -vad  för värde vi kan sätta in och hur vi vill de värderna ska vara
        public TemperatureModel(double temperature, double humidity)
        {
            Temperature = temperature;
            Humidity = humidity;
            Timestamp = DateTime.Now;  //sätt med hjälp ctor
        }
    }

}

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SharedLibrary.Models;

namespace WorkerService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> log; //�ndra _logger =log som i Azure(GetTemp.)

         private HttpClient client;    // 1 .  BARA DEKLARERA

        public Worker(ILogger<Worker> logger)
        {
            log = logger;
        }
        // on jag vill g�ra n�n kan skapa i class WORKER ---GENERATE OVVERRIDES 
        public override Task StartAsync(CancellationToken cancellationToken)
        {
            client = new HttpClient(); // 2.N�R STARTAR--D� KOMMER ATT G�RA INCTANSE-  AV HTTPCLIENT OBJECTER-- RESERVERA RAMMINNE

            log.LogInformation("Worker STARTED at: {time}", DateTimeOffset.Now);
            return base.StartAsync(cancellationToken);
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {

            client.Dispose();  //3.N�R TRYCKER P� STOP --TA BORT HTTPCLIENT OBJECT

            log.LogInformation("Worker STOPPED at: {time}", DateTimeOffset.Now);
            return base.StopAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {// N�R G�R SIT ARBETA , VILL ATT DET H�MTA FR�N API(1.2.3)


                //f�r att se fel g�ra try-catch:

                try
                {
                    // H�MTA RESULTAT:
                    var response = await client.GetAsync("https://api.openweathermap.org/data/2.5/onecall?lat=59.23797&lon=14.43077&units=metric&exclude=current&appid=594a02d31f7827d13d00eb499ee71ef4");//
                                                                                                                                                                                                          //vi kan ha m�nhga olika response s� vi skapa IF SATS:
                    if (response.IsSuccessStatusCode)
                    {

                        var result = await response.Content.ReadAsStringAsync();// result ser ut som mdl-text
                                                                                // vi packa ut mdl-text till Temperature objeckt 
                        var data = JsonConvert.DeserializeObject<TemperatureModel>(result);// kanh�mta fr�n result alt och l�gga in result

                        //nu kan skriva ut info ---data. tar fr�m model
                        log.LogInformation($"Temperature: {data.Temperature}, Humidity: {data.Humidity} , Time {data.Timestamp}");
                    }
                    else
                    { 
                        log.LogInformation("Failed to get data f�m API: {time}", DateTimeOffset.Now);
                    }
                }

                catch
                {
                    log.LogInformation("Failed to get data f�m API: {time}", DateTimeOffset.Now);
                }

                log.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                await Task.Delay(360000, stoppingToken);
            }
        }
    }
}

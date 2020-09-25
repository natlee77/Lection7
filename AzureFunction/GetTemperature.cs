using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SharedLibrary.Models;
using Microsoft.AspNetCore.Authorization;

namespace AzureFunction
{
    public static class GetTemperature
    {
        //vi ska slumpa nummer:
        //var rnd = new Random(); /inne f.
        private static Random rnd = new Random(); //utanf�r f.


           [FunctionName("GetTemperature")]

        //AuthorizationLevel.Anonymous- beh�vs inte key

        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
            ILogger log)


        {  // vi ta n�n som inte async  och skapa till async (Task.Run)
            return new OkObjectResult(await Task.Run(() =>
            {

                //skapa json formaterad mdl--
                return JsonConvert.SerializeObject(new TemperatureModel
                {
                    Temperature = rnd.Next(20, 30),
                    Humidity = rnd.Next(30, 40),
                    Timestamp = DateTime.Now
                });

            }));
        }
        
    }
}
/* n�sta �ndringar : fr�n var1
{
    var result = await Task.Run(() =>
    {
     var data = new TemperatureModel()
     {
         Temperature = rnd.Next(20, 30),
         Humidity = rnd.Next(30, 40),
         Tamestamp=DateTime.Now
     };

     var json = JsonConvert.SerializeObject(data);//
     return json;
    });
    return new OkObjectResult(result);
}
 */

/* F�sta Variant mer tydlig
 * 
 * //alla det  �r sync --- returera ingenting till __RUN
    m�ste ha  --await Task.Run ///
   //eller skapa object inst�llet 2 st variable

   var data = new TemperatureModel()
   {
       Temperature = rnd.Next(20, 30),
       Humidity = rnd.Next(30, 40)

   };
   // 2 st  variable
   var temperature = rnd.Next(20,30);// next best�mmer -t gnereras mellan 20-30
   var humidity = rnd.Next(30, 40);


   // ta object TemperatureModel() och convectera i json-text f�r att skicka iv�g

   var json = JsonConvert.SerializeObject(data);//


   return new OkObjectResult(json);// returera i v�g json-text*/
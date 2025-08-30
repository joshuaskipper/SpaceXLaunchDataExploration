using System;
using System.Text.Json;

namespace SpaceXLaunchDataExploration 
{
    class Program 
    {
        static  async Task Main(string[] args) 
        {

            //DateTime date = 8/29/2025
            //var year = DateTime.Now.Year;


            string url = "https://api.spacexdata.com/v4/launches?utm_source=chatgpt.com";

            HttpClient client = new HttpClient();
            //Console.WriteLine(year);

            try 
            {
                var response = await client.GetAsync(url);

                if (response.IsSuccessStatusCode) 
                {
                    var option = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true,
                    };

                    var json = await response.Content.ReadAsStringAsync();

                    var launchJson = JsonSerializer.Deserialize<List<Root>>(json);

                    /*
                    foreach (var item in launchJson)
                    {
                        Console.WriteLine($"\nItem#:{item.launchpad} - Flight#:{item.flight_number} - Flight Name:{item.name} - Flight Success:{item.success} - Date:{item.date_utc} ");
                        foreach(var fail in item.failures) 
                        {
                            Console.WriteLine($"Failure Reason:{fail.reason}");
                        }

                    }
                    */

                    var successLaunchesIn2022 = from item in launchJson
                                                where item.date_utc.Year == 2022 &
                                                item.success == true
                                              select item;

                    foreach (var item in successLaunchesIn2022)
                    {
                        Console.WriteLine($"{item.name} - {item.date_utc}");

                    }
                    

                   
                    
                }

            
            }
            catch(Exception ex) 
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
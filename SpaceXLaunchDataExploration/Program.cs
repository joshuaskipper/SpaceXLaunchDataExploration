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

                    var successLaunchesIn2022 = (from item in launchJson
                                                where item.date_utc.Year == 2022 &
                                                item.success == true
                                                orderby item.date_utc descending
                                              select item).Take(5);

                    Console.WriteLine("TOP 5 MOST RECENT SUCCESSFUL SPACEX LAUNCHES OF 2022");
                    Console.WriteLine($"----------------------------------------------------------\n");
                    foreach (var item in successLaunchesIn2022)
                    {
                        Console.WriteLine($"{item.name} - {item.date_utc}");

                    }

                    Console.WriteLine($"\nWould you like to see all the data from spaceX launches? (yes/no)");

                    if (Console.ReadLine().Trim().ToLower() == "yes") 
                    {
                        foreach (var item in launchJson)
                        {
                            Console.WriteLine($"\nItem#:{item.launchpad} - Flight#:{item.flight_number} - Flight Name:{item.name} - Flight Success:{item.success} - Date:{item.date_utc} ");
                            foreach (var fail in item.failures)
                            {
                                Console.WriteLine($"Failure Reason:{fail.reason}");
                            }

                        }
                    }
                    else
                    {
                        Console.WriteLine("Exiting Program...");
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
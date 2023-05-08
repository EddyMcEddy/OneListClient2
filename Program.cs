using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using ConsoleTables;

namespace OneListClient2
{
    class Program
    {
        static async Task AddOneItem(string token, Item newItem)
        {
            var client = new HttpClient();

            // Generate a URL specifically referencing the endpoint for adding a todo item
            var url = $"https://one-list-api.herokuapp.com/items?access_token={token}";

            // Take the `newItem` and serialize it into JSON
            var jsonBody = JsonSerializer.Serialize(newItem);

            // We turn this into a StringContent object and indicate we are using JSON
            // by ensuring there is a media type header of `application/json`
            var jsonBodyAsContent = new StringContent(jsonBody);
            jsonBodyAsContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            // Send the POST request to the URL and supply the JSON body
            var response = await client.PostAsync(url, jsonBodyAsContent);

            // Get the response as a stream.
            var responseJson = await response.Content.ReadAsStreamAsync();

            // Supply that *stream of data* to a Deserialize that will interpret it as a *SINGLE* `Item`
            var item = await JsonSerializer.DeserializeAsync<Item>(responseJson);

            // Make a table to output our new item.
            var table = new ConsoleTable("ID", "Description", "Created At", "Updated At", "Completed");

            // Add one row to our table
            table.AddRow(item.id, item.text, item.created_at, item.updated_at, item.CompletedStatus);

            // Write the table
            table.Write(Format.Minimal);
        }

        static async Task GetOneItem(string token, int id)
        {

            try
            {
                //HttpClient is a Library that will send and receive information to API over HTTP
                var client = new HttpClient();

                //creating a variable to get the response from the Http or Website
                var responseAsStream = await client.GetStreamAsync($"https://one-list-api.herokuapp.com/items/{id}?access_token={token}");

                //Take the List off unlike ShowAllItems where the List<> must be added
                var items = await JsonSerializer.DeserializeAsync<Item>(responseAsStream);

                //Creating a table so it shows us in Terminal better 
                var table = new ConsoleTable("Description", "Created At", "Completed");

                table.AddRow(items.text, items.created_at, items.CompletedStatus);
                table.Write();

            }
            catch (HttpRequestException)
            {

                Console.WriteLine("Could not Find the Item");
            }
        }

        static async Task ShowAllItems(string token)
        {

            //HttpClient is a Library that will send and receive information to API over HTTP
            var client = new HttpClient();

            //creating a variable to get the response from the Http or Website
            var responseAsStream = await client.GetStreamAsync($"https://one-list-api.herokuapp.com/items?access_token={token}");

            // Telling the website or HTTP that we want the information set up like our Class Item
            var items = await JsonSerializer.DeserializeAsync<List<Item>>(responseAsStream);

            //Creating a table so it shows us in Terminal better 
            var table = new ConsoleTable("Description", "Created At", "Completed");

            // For each item in our deserialized List of Item
            foreach (var item in items)
            {
                // Output some details on that item adn adding a Row into our Table 
                table.AddRow(item.text, item.created_at, item.CompletedStatus);
            }
            table.Write();
        }
        static async Task Main(string[] args)
        {

            //This token lets The code ude 
            var token = "";

            if (args.Length == 0)
            {
                Console.Write("What list would you like? ");
                token = Console.ReadLine();
            }
            else
            {
                token = args[0];
            }






            var keepGoing = true;
            while (keepGoing)
            {
                Console.Clear();
                Console.Write("Get (A)ll Items,(C)Create Item,(O)ne Item or (Q)uit: ");
                var choice = Console.ReadLine().ToUpper();
                switch (choice)
                {
                    case "Q":
                        keepGoing = false;
                        break;
                    case "C":
                        Console.Write("Enter the description of your new todo: ");
                        var Text = Console.ReadLine();

                        var newItem = new Item
                        {
                            text = Text
                        };

                        await AddOneItem(token, newItem);

                        Console.WriteLine("Press ENTER to continue");
                        Console.ReadLine();
                        break;
                    case "O":
                        Console.WriteLine("Enter the ID?: ");
                        var id = int.Parse(Console.ReadLine());
                        await GetOneItem(token, id);

                        Console.WriteLine("Press ENTER to Continue");
                        Console.ReadLine();
                        break;
                    case "A":
                        await ShowAllItems(token);
                        Console.WriteLine("Press ENTER to continue");
                        Console.ReadLine();
                        break;
                    default:
                        break;
                }










            }
        }
    }
}

using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using TB3.ConsoleAppConnectWithApi.Dtos;
using static System.Net.Mime.MediaTypeNames;

namespace TB3.ConsoleAppConnectWithApi;

public class RestClientService
{
    private readonly string _baseUrl = "https://localhost:7258";
    public async Task Read()
    {
        RestClient client = new RestClient();
        RestRequest request = new RestRequest(_baseUrl + "/api/Product", Method.Get);
        var response = await client.ExecuteAsync(request);

        if (response.IsSuccessStatusCode)
        {
            var lst = JsonConvert.DeserializeObject<List<ProductDto>>(response.Content!);
            //Console.WriteLine(json);

            Console.WriteLine("Product List:");
            Console.WriteLine("----------------------------");
            foreach (ProductDto item in lst)
            {
                Console.WriteLine($"Product Name    : {item.ProductName}");
                Console.WriteLine($"Product Price   : {item.Price.ToString("n2")}");
                Console.WriteLine($"Product Quantity: {item.Quantity.ToString("n0")}");
                Console.WriteLine("----------------------------");
            }
        }
    }

    public async Task Create()
    {
        Console.Write("Please enter product name: ");
        string productName = Console.ReadLine();

        Console.Write("Please enter price: ");
        decimal price = Convert.ToDecimal(Console.ReadLine());

        Console.Write("Please enter quantity: ");
        int quantity = Convert.ToInt32(Console.ReadLine());

        ProductCreateRequestDto requestDto = new ProductCreateRequestDto()
        {
            Price = price,
            ProductName = productName,
            Quantity = quantity
        }; // object to json

        RestClient client = new RestClient();
        RestRequest request = new RestRequest(_baseUrl + "/api/Product", Method.Post);
        request.AddJsonBody(requestDto);
        var response = await client.ExecuteAsync(request);
        var message = response.Content!;
        Console.WriteLine(message);
    }

    public async Task Update()
    {
        Console.Write("Please enter product id: ");
        int productId = Convert.ToInt32(Console.ReadLine());

        Console.Write("Please enter product name: ");
        string productName = Console.ReadLine();

        Console.Write("Please enter price: ");
        decimal price = Convert.ToDecimal(Console.ReadLine());

        Console.Write("Please enter quantity: ");
        int quantity = Convert.ToInt32(Console.ReadLine());

        ProductUpdateRequestDto requestDto = new ProductUpdateRequestDto
        {
            ProductName = productName,
            Quantity = quantity,
            Price = price
        };

        RestClient client = new RestClient();
        RestRequest request = new RestRequest($"{_baseUrl}/api/Product/{productId}", Method.Put);
        request.AddJsonBody(requestDto);
        var response = await client.ExecuteAsync(request);
        var message = response.Content!;
        Console.WriteLine(message);
    }

    public async Task Patch()
    {
        Console.Write("Please enter product id: ");
        int productId = Convert.ToInt32(Console.ReadLine());

        Console.Write("Please enter product name: ");
        string productName = Console.ReadLine();


        Console.Write("Please enter price: ");
        string priceStr = Console.ReadLine();
        decimal price = string.IsNullOrEmpty(priceStr) ? 0 : Convert.ToDecimal(priceStr);

        Console.Write("Please enter quantity: ");
        string qtyStr = Console.ReadLine();
        int quantity = string.IsNullOrEmpty(qtyStr) ? 0 : Convert.ToInt32(qtyStr);

        ProductUpdateRequestDto requestDto = new ProductUpdateRequestDto
        {
            ProductName = productName,
            Quantity = quantity,
            Price = price
        };

        RestClient client = new RestClient();
        RestRequest request = new RestRequest($"{_baseUrl}/api/Product/{productId}", Method.Patch);
        request.AddJsonBody(requestDto);
        var response = await client.ExecuteAsync(request);
        var message = response.Content!;
        Console.WriteLine(message);
    }

    public async Task Delete()
    {
        Console.Write("Please enter product id: ");
        int productId = Convert.ToInt32(Console.ReadLine());

        RestClient client = new RestClient();
        RestRequest request = new RestRequest($"{_baseUrl}/api/Product/{productId}", Method.Delete);
        var response = await client.ExecuteAsync(request);
        string message =  response.Content!;
        Console.WriteLine(message);
    }
}

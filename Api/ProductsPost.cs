using System.Collections.Generic;
using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using Data;

namespace Api;

public class ProductsPost
{
	private readonly ILogger _logger;
	
    private readonly IProductData productData;

    public ProductsPost(IProductData productData, ILoggerFactory loggerFactory)
    {
		_logger = loggerFactory.CreateLogger<ProductsPost>();
        this.productData = productData;
    }

    [Function("ProductsPost")]
    public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "products")] HttpRequestData req, ILogger log)
    {
        _logger.LogInformation("Post route processed a request.");

        var body = await new StreamReader(req.Body).ReadToEndAsync();

        var product = JsonSerializer.Deserialize<Product>(body, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });

        var newProduct = await productData.AddProduct(product);
		
		var response = req.CreateResponse(HttpStatusCode.OK);
        response.Headers.Add("Content-Type", "text/plain; charset=utf-8");

        response.WriteString("Post performed");

        return response;
    }
}
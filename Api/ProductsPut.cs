using System.Collections.Generic;
using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using Data;

namespace Api;

public class ProductsPut
{
    private readonly ILogger _logger;
	
    private readonly IProductData productData;

    public ProductsPut(IProductData productData, ILoggerFactory loggerFactory)
    {
		_logger = loggerFactory.CreateLogger<ProductsPut>();
        this.productData = productData;
    }

    [Function("ProductsPut")]
    public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "products")] HttpRequestData req, ILogger log)
    {
        var body = await new StreamReader(req.Body).ReadToEndAsync();
        var product = JsonSerializer.Deserialize<Product>(body, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });

        var updatedProduct = await productData.UpdateProduct(product);
        
		var response = req.CreateResponse(HttpStatusCode.OK);
        response.Headers.Add("Content-Type", "text/plain; charset=utf-8");

        response.WriteString("Post performed");

        return response;
    }
}

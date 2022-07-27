using System.Collections.Generic;
using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Data;

namespace Api;

public class ProductsDelete
{
    private readonly ILogger _logger;
	
    private readonly IProductData productData;

    public ProductsDelete(IProductData productData, ILoggerFactory loggerFactory)
    {
		_logger = loggerFactory.CreateLogger<ProductsDelete>();
        this.productData = productData;
    }

    [Function("ProductsDelete")]
    public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "products/{productId:int}")] HttpRequestData req, int productId, ILogger log)
    {
        _logger.LogInformation("Deletion route processed a request.");

        var result = await productData.DeleteProduct(productId);

        var response = req.CreateResponse(HttpStatusCode.OK);
        response.Headers.Add("Content-Type", "text/plain; charset=utf-8");

        response.WriteString("Delete performed");
		
		return response;
    }
}

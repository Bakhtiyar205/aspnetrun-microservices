using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shopping.Aggregator.Models;
using Shopping.Aggregator.Services;

namespace Shopping.Aggregator.Controllers;
[ApiController]
[Route("api/v1/[controller]")]
public class ShoppingController : ControllerBase
{
    private readonly IOrderService _orderService;
    private readonly IBasketService _basketService;
    private readonly ICatalogService _catalogService;

    public ShoppingController(IOrderService orderService, IBasketService basketService, ICatalogService catalogService)
    {
        _orderService = orderService ?? throw new ArgumentNullException(nameof(orderService));
        _basketService = basketService ?? throw new ArgumentNullException(nameof(basketService));
        _catalogService = catalogService ?? throw new ArgumentNullException(nameof(catalogService));
    }

    [HttpGet("{userName}", Name = "GetShopping")]
    [ProducesResponseType(typeof(ShoppingModel), StatusCodes.Status200OK)]
    public async Task<ActionResult<ShoppingModel>> GetShopping(string userName)
    {
        // get basket with username
        // iterate basket items and consume products with basket item productId member
        // map product related members into basketitem dto with extended columns
        // consume ordering microservices and retrieve order list
        // return root ShoppinModel dto class which including all responses
        var basket = await _basketService.GetBasket(userName);
        foreach (var item in basket.Items)
        {
            var product = await _catalogService.GetCatalog(item.ProductId);
            // set additional product fields onto basket item
            item.ProductName = product.Name;
            item.Category = product.Category;
            item.Summary = product.Summary;
            item.Description = product.Description;
            item.ImageFile = product.ImageFile;
        }

        var orders = await _orderService.GetOrdersByUserName(userName);

        var shoppingModel = new ShoppingModel
        {
            UserName = userName,
            BasketWithProducts = basket,
            Orders = orders
        };

        return Ok(shoppingModel);
    }
}

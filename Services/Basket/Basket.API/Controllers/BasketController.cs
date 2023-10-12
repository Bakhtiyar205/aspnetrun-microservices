using AutoMapper;
using Basket.API.Entities;
using Basket.API.GrpcServices;
using Basket.API.Repositories;
using EventBus.Messages.Events;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Basket.API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class BasketController : ControllerBase
{
   private readonly IBasketRepository _basketRepository;
   private readonly DiscountGrpcService _discoubtGrpcService;
   private readonly IMapper _mapper;

    public BasketController(IBasketRepository basketRepository,
        DiscountGrpcService discoubtGrpcService,
        IMapper mapper)
    {
        _basketRepository = basketRepository ?? throw new ArgumentNullException(nameof(basketRepository));
        _discoubtGrpcService = discoubtGrpcService ?? throw new ArgumentNullException(nameof(discoubtGrpcService));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }


    [HttpGet("[action]/{userName}", Name ="GetBasket")]
    [ProducesResponseType(typeof(ShoppingCart), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<ShoppingCart>> GetBasket(string userName)
    {
        var basket = await _basketRepository.GetBasketAsync(userName); 
        return Ok(basket ?? new ShoppingCart(userName));
    }

    [HttpPost]
    [ProducesResponseType(typeof(ShoppingCart), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<ShoppingCart>> UpdateBasket([FromBody] ShoppingCart shoppingCart)
    {
        foreach (var item in shoppingCart.Items)
        {
            var coupon = await _discoubtGrpcService.GetDiscount(item.ProductName);
            if (coupon is null) continue;
            item.Price -= coupon.Amount;
        }
        return Ok(await _basketRepository.UpdateBasketAsync(shoppingCart));
    }

    [HttpDelete("{userName}", Name ="DeleteBasket")]
    [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> DeleteBasket(string userName)
    {
        await _basketRepository.DeleteBasketAsync(userName);
        return Ok();
    }

    [Route("[action]")]
    [HttpPost]
    [ProducesResponseType((int)HttpStatusCode.Accepted)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> Checkout([FromBody] BasketCheckout basketCheckout)
    {
        // get existing basket with total price 
        // Create basketCheckoutEvent -- Set TotalPrice on basketCheckout eventMessage
        // send checkout event to rabbitmq
        // remove the basket
        var basket = await _basketRepository.GetBasketAsync(basketCheckout.UserName);
        if (basket is null) return BadRequest();

        //send checkout event to rabbitmq
        var eventMessage = _mapper.Map<BasketCheckoutEvent>(basketCheckout);
        //_eventBus.PublishBaketCheckout
        //eventMessage.TotalPrice = basket.TotalPrice;

        //remove the basket
        await _basketRepository.DeleteBasketAsync(basket.UserName);

        return Accepted();
    }
}

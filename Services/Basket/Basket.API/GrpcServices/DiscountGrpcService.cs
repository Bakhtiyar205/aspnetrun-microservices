using Discount.Grpc.Protos;

namespace Basket.API.GrpcServices;

public class DiscountGrpcService
{
    private readonly DiscountProtoService.DiscountProtoServiceClient _discountProtoServiceClient;
    private readonly ILogger<DiscountGrpcService> _logger;

    public DiscountGrpcService(DiscountProtoService.DiscountProtoServiceClient discountProtoServiceClient, ILogger<DiscountGrpcService> logger)
    {
        _discountProtoServiceClient = discountProtoServiceClient;
        _logger = logger;
    }

    public async Task<CouponModel> GetDiscount(string? productName)
    {
        try
        {
            if (productName == null) return null;
            var discountRequest = new GetDiscountRequest { ProductName = productName };
            return await _discountProtoServiceClient.GetDiscountAsync(discountRequest);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Grpc Error: {ex.Message}");
            _logger.LogError($"Grpc Error: {ex.Message}");
            throw;
        }
        
    }
}

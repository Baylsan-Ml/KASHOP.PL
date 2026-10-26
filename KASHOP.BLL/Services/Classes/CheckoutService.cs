using KASHOP.BLL.Services.Interfaces;
using KASHOP.DAL.DTO;
using KASHOP.DAL.Models;
using KASHOP.DAL.Repository;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Stripe.Checkout;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KASHOP.BLL.Services.Classes
{
    public class CheckoutService : ICheckoutService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;

        public CheckoutService(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }
        public async Task<Result<OrderResponse>> Checkout(string userId, CheckoutRequest request)
        {
            var cartItems = await _unitOfWork.CartRepository.GetAllAsync(
                filter: c => c.UserId == userId,
                includes: new string[]
                {  nameof(CartItem.Product),
                   "Product.Translations" }
            );

            if(!cartItems.Any())
            {
                return Result<OrderResponse>.Fail("Cart is empty");
            }
            foreach (var item in cartItems)
            {
                if (item.Product.Quantity < item.Count)
                {
                    return Result<OrderResponse>.Fail($"not enough stock for Product {item.ProductId}");
                }
            }
            var user = await _userManager.FindByIdAsync(userId);
            if(user is null)
            {
                return Result<OrderResponse>.Fail("User not found");
            }
            var city = request.City ?? user.City;
            var street = request.Street ?? user.Street;
            var phoneNumber = request.PhoneNumber ?? user.PhoneNumber;

            if(string.IsNullOrEmpty(city) || string.IsNullOrEmpty(street) || string.IsNullOrEmpty(phoneNumber))
                return Result<OrderResponse>.Fail("Shipping Info Is Required");

            var order = new Order
            {
                UserId = userId,
                PaymentMethod = request.PaymentMethod,
                Status = DAL.Enums.OrderStatusEnum.Pending,
                PaymentStatus = DAL.Enums.PaymentStatus.Pending,
                City = city,
                Street = street,
                PhoneNumber = phoneNumber,
                OrderItems = new List<OrderItem>()
            };

            decimal totalAmount = 0;
            foreach(var item in cartItems)
            {
                var orderItem = new OrderItem
                {
                    ProductId = item.ProductId,
                    Count = item.Count,
                    PriceAtPurchase = item.Product.Price
                };
                order.OrderItems.Add(orderItem);
                totalAmount += orderItem.PriceAtPurchase * orderItem.Count;
            }
            order.TotalAmount = totalAmount;
            return request.PaymentMethod == DAL.Enums.PaymentMethodEnum.Cash ?
                await PlaceCashOrderAsync(order, cartItems)
                :
                await PlaceVisaOrderAsync(order, cartItems);
        }

        private async Task<Result<OrderResponse>> PlaceVisaOrderAsync(Order order, List<CartItem> cartItems)
        {
            var options = new SessionCreateOptions
            {
                Mode = "payment",
                LineItems = cartItems.Select(Item => new SessionLineItemOptions
                {
                    Quantity = Item.Count,
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = Item.Product.Translations.FirstOrDefault(t => t.Language == "en")?.Name ?? "Unknown Product",
                        },
                        UnitAmount = (long)(Item.Product.Price * 100),
                        Currency = "usd",
                    },
                }).ToList(),

                SuccessUrl = $"/checkout/success",
                CancelUrl = $"/checkout/cancel",
            };
            
            var service = new SessionService();
            var session = service.Create(options);
            order.StripeSessionId = session.Id;
            await _unitOfWork.OrderRepository.CreateAsync(order);
            await _unitOfWork.CompleteAsync();

            var response = order.Adapt<OrderResponse>();
            response.PaymentUrl = session.Url;

            return Result<OrderResponse>.Ok(response, "Redirecting to payment page...");
        }

        private async Task<Result<OrderResponse>> PlaceCashOrderAsync(Order order, List<CartItem> cartItems)
        {
          foreach(var item in cartItems)
            {
                item.Product.Quantity -= item.Count;
                _unitOfWork.CartRepository.Delete(item);
            }
            await _unitOfWork.OrderRepository.CreateAsync(order);
            await _unitOfWork.CompleteAsync();
            return Result<OrderResponse>.Ok(order.Adapt<OrderResponse>(), "Order Created Successfully");
        }
    }
}

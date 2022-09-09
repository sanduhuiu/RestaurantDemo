using Microsoft.AspNetCore.Mvc;
using Restaurant.Services.CouponAPI.Models.Dto;
using Restaurant.Services.CouponAPI.Repository;

namespace Restaurant.Services.CouponAPI.Controllers
{
    [ApiController]
    [Route("api/coupon")]
    public class CouponAPIController : Controller
    {
        private readonly ICouponRepository _couponRepository;
        protected ResponseDto _response;
        public CouponAPIController(ICouponRepository couponRepository)
        {
            _couponRepository = couponRepository;
            this._response = new ResponseDto();
        }

        [HttpGet("{couponCode}")]
        public async Task<object> GetDiscountForCode(string couponCode)
        {
            try
            {
                var coupon = await _couponRepository.GetCouponByCode(couponCode);
                _response.Result = coupon;

            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }

            return _response;
        }
    }
}

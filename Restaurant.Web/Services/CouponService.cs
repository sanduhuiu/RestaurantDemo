﻿using Restaurant.Web.Models;
using Restaurant.Web.Services.IServices;

namespace Restaurant.Web.Services
{
    public class CouponService : BaseService, ICouponService
    {
        private readonly IHttpClientFactory _clientFactory;
        public CouponService(IHttpClientFactory clientFactory) : base(clientFactory)
        {
            _clientFactory = clientFactory;
        }
        public async Task<T> GetCoupon<T>(string couponCode, string token = null)
        {
            var url = SD.CouponAPIBase + "/api/coupon/" + couponCode;
            return await this.SendAsync<T>(new ApiRequest()
            {
                ApiType = SD.ApiType.GET,
                Url = url,
                AccessToken = token
            });
        }
    }
}

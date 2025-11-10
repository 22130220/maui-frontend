using MauiFrontend.Constants;
using MauiFrontend.Http;
using MauiFrontend.Models;
using MauiFrontend.Models.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MauiFrontend.Services
{
    public class ProductReviewService : BaseService
    {
        public ProductReviewService(Https https) : base(https)
        {
        }

        public async Task<ApiResponse<DataPaging<ProductReview>>> GetListReview(string param)
        {
            var list = await this.GetSingleNoSeperatorAsync<ApiResponse<DataPaging<ProductReview>>>(APICONSTANT.PRODUCT_REVIEW.GET_LIST, param);
            return list;
        }

        public async Task<ApiResponse<SummaryReview>> GetSummaryReviewAsync(string param)
        {
            var summary = await this.GetSingleNoSeperatorAsync<ApiResponse<SummaryReview>>(APICONSTANT.PRODUCT_REVIEW.SUMMARY_REVIEW, param);
            return summary;
        }

        public async Task<ApiResponse<Object>> PostReviewAsync(ProductReviewCreateRequest request)
        {
            try
            {
                var response = await _https.PostAsync<ProductReviewCreateRequest, ApiResponse<Object>>(
                   APICONSTANT.PRODUCT_REVIEW.POST_COMMENT,
                   request
                );

                return response;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

    }
}

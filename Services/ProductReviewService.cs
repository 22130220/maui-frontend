using MauiFrontend.Constants;
using MauiFrontend.Http;
using MauiFrontend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            
    }
}

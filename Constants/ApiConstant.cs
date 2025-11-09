using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiFrontend.Constants
{
    public static class APICONSTANT
    {
        public static class PRODUCT
        {
            public const string GET_LIST = "products/get-list";
            public const string CREATE = "product/";
            public const string DETAIL = "products/detail/";
            public const string SEARCH = "products/search";
        }

        public static class USER
        {
            public const string LOGIN = "http://localhost:8080/users/login";
            public const string LOGOUT = "users/logout";
        }

        public static class PRODUCT_REVIEW
        {
            public const string GET_LIST = "product-reviews/get-list";
            public const string SUMMARY_REVIEW = "product-reviews/summary-review";
        }

    }
}

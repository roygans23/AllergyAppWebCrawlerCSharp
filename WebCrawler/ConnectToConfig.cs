using System.Configuration;

namespace WebCrawler
{
    public static class ConnectToConfig
    {
        private const string MAX_PAGES_TO_SEARCH = "MAX_PAGES_TO_SEARCH";
        private const string USER_AGENT_BROWSER = "USER_AGENT_BROWSER";
        private const string FIRST_LINK_TO_CRAWL = "FIRST_LINK_TO_CRAWL";
        private const string SCOPE_LINK = "SCOPE_LINK";
        private const string PRODUCT_PAGE_POSTFIX = "PRODUCT_PAGE_POSTFIX";
        private const string PRODUCT_LINK_ATTRIBUTE = "PRODUCT_LINK_ATTRIBUTE";

        
        private const string PRODUCT_BARCODE_REGEX = "PRODUCT_BARCODE_REGEX";
        private const string PRODUCT_NAME_REGEX = "PRODUCT_NAME_REGEX";
        private const string PRODUCT_INGREDIENTS_REGEX = "PRODUCT_INGREDIENTS_REGEX";
        private const string PRODUCT_ALLERGY_REGEX = "PRODUCT_ALLERGY_REGEX";

        private const string PRODUCT_DB_CONNECTION_STRING = "ProductDbConnectionString";

        public static int MaxPagesToSearch
        {
            get
            {
                if (!int.TryParse(ConfigurationManager.AppSettings[MAX_PAGES_TO_SEARCH], out int maxPagesToSearch))
                {
                    return 0;
                };
                return maxPagesToSearch;
            }
        }

        public static string UserAgentBrowser
        {
            get
            {
                return ConfigurationManager.AppSettings[USER_AGENT_BROWSER];
            }
        }

        public static string FirstLinkToCrawl
        {
            get
            {
                return ConfigurationManager.AppSettings[FIRST_LINK_TO_CRAWL];
            }
        }

        public static string ScopeLink
        {
            get
            {
                return ConfigurationManager.AppSettings[SCOPE_LINK];
            }
        }

        public static string ProductPagePostfix
        {
            get
            {
                return ConfigurationManager.AppSettings[PRODUCT_PAGE_POSTFIX];
            }
        }

        public static string ProductLinkAttribute
        {
            get
            {
                return ConfigurationManager.AppSettings[PRODUCT_LINK_ATTRIBUTE];
            }
        }

        public static string BarcodeRegexName
        {
            get
            {
                return ConfigurationManager.AppSettings[PRODUCT_BARCODE_REGEX];
            }
        }

        public static string ProductNameRegex
        {
            get
            {
                return ConfigurationManager.AppSettings[PRODUCT_NAME_REGEX];
            }
        }

        public static string ProductIngredientsRegex
        {
            get
            {
                return ConfigurationManager.AppSettings[PRODUCT_INGREDIENTS_REGEX];
            }
        }

        public static string ProductAllergyRegex
        {
            get
            {
                return ConfigurationManager.AppSettings[PRODUCT_ALLERGY_REGEX];
            }
        }

        public static string ProductDbConnectionString
        {
            get
            {
                return ConfigurationManager.ConnectionStrings[PRODUCT_DB_CONNECTION_STRING].ConnectionString;
            }
        }
    }
}

using System.Configuration;

namespace WebCrawler
{
    public static class ConnectToConfig
    {
        private const string MAX_PAGES_TO_SEARCH = "MAX_PAGES_TO_SEARCH";
        private const string FIRST_LINK_TO_CRAWL = "FIRST_LINK_TO_CRAWL";
        private const string SCOPE_LINK = "SCOPE_LINK";


        private const string RECIPE_JSON_OBJECT_REGEX = "RECIPE_JSON_OBJECT_REGEX";
        private const string START_INDEX_JSON_OBJECT_REGEX = "START_INDEX_JSON_OBJECT_REGEX";
        private const string END_INDEX_JSON_OBJECT_REGEX = "END_INDEX_JSON_OBJECT_REGEX";

        private const string RECIPE_MAIN_CATEGORY_REGEX = "RECIPE_MAIN_CATEGORY_REGEX";
        private const string START_INDEX_MAIN_CATEGORY_REGEX = "START_INDEX_MAIN_CATEGORY_REGEX";
        private const string START_POSITION_PREFIX_MAIN_CATEGORY_REGEX = "START_POSITION_PREFIX_MAIN_CATEGORY_REGEX";
        private const string END_POSITION_SUFFIX_MAIN_CATEGORY_REGEX = "END_POSITION_SUFFIX_MAIN_CATEGORY_REGEX";

        private const string RECIPES_DB_INSERT_SQL_QUERY_PREFIX = "RECIPES_DB_INSERT_SQL_QUERY_PREFIX";
        private const string RECIPES_DB_UPDATE_SQL_QUERY_PREFIX = "RECIPES_DB_UPDATE_SQL_QUERY_PREFIX";
        private const string RECIPES_DB_COUNT_SQL_QUERY_PREFIX = "RECIPES_DB_COUNT_SQL_QUERY_PREFIX";
        
        private const string RECIPES_DB_CONNECTION_STRING = "RecipesDbConnectionString";

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

        public static string RecipeJsonObjectRegex
        {
            get
            {
                return ConfigurationManager.AppSettings[RECIPE_JSON_OBJECT_REGEX];
            }
        }

        public static int StartIndexJsonObjectRegex
        {
            get
            {
                if (!int.TryParse(ConfigurationManager.AppSettings[START_INDEX_JSON_OBJECT_REGEX], out int startIndexRegex))
                {
                    return 0;
                };
                return startIndexRegex;
            }
        }

        public static int EndIndexJsonObjectRegex
        {
            get
            {
                if (!int.TryParse(ConfigurationManager.AppSettings[END_INDEX_JSON_OBJECT_REGEX], out int endIndexRegex))
                {
                    return 0;
                };
                return endIndexRegex;
            }
        }

        public static string RecipeMainCategoryRegex
        {
            get
            {
                return ConfigurationManager.AppSettings[RECIPE_MAIN_CATEGORY_REGEX];
            }
        }

        public static int StartIndexMainCategoryRegex
        {
            get
            {
                if (!int.TryParse(ConfigurationManager.AppSettings[START_INDEX_MAIN_CATEGORY_REGEX], out int startIndexRegex))
                {
                    return 0;
                };
                return startIndexRegex;
            }
        }

        public static string StartPositionPrefixMainCategoryRegex
        {
            get
            {
                return ConfigurationManager.AppSettings[START_POSITION_PREFIX_MAIN_CATEGORY_REGEX];
            }
        }

        public static string EndPositionSuffixMainCategoryRegex
        {
            get
            {
                return ConfigurationManager.AppSettings[END_POSITION_SUFFIX_MAIN_CATEGORY_REGEX];
            }
        }

        public static string RecipesDbInsertSqlQueryPrefix
        {
            get
            {
                return ConfigurationManager.AppSettings[RECIPES_DB_INSERT_SQL_QUERY_PREFIX];
            }
        }

        public static string RecipesDbUpdateSqlQueryPrefix
        {
            get
            {
                return ConfigurationManager.AppSettings[RECIPES_DB_UPDATE_SQL_QUERY_PREFIX];
            }
        }

        public static string RecipesDbCountSqlQueryPrefix
        {
            get
            {
                return ConfigurationManager.AppSettings[RECIPES_DB_COUNT_SQL_QUERY_PREFIX];
            }
        }
        

        public static string ProductDbConnectionString
        {
            get
            {
                return ConfigurationManager.ConnectionStrings[RECIPES_DB_CONNECTION_STRING].ConnectionString;
            }
        }
    }
}

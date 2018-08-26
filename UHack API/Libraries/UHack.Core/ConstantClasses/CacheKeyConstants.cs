using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UHack.Core.ConstantClasses
{
    public partial class CacheKeyConstants
    {
        /// <summary>
        /// <para>0 - Category Id</para>
        /// <para>1 - SubCategory Id</para>
        /// <para>2 - Latitude</para>
        /// <para>3 - Longitude</para>
        /// </summary>
        public const string BUSINESS_BY_CATEGORY_KEY = "UHack.SERVICES.BUSINESS_BY_CATEGORY.{0}.{1}.{2}.{3}";

        public const string BUSINESS_BY_CATEGORY_STATE_KEY = "UHack.SERVICES.BUSINESS_BY_CATEGORY.{0}.{1}.{2}.{3}.{4}";

        /// <summary>
        /// <para>0 - OriginLat</para>
        /// <para>1 - OriginLng</para>
        /// <para>2 - Limit</para>
        /// <para>3 - ClubId</para>
        /// </summary>
        public const string NEW_BUSINESSES_KEY = "UHack.SERVICES.NEW_BUSINESSES.{0}.{1}.{2}.{3}";

        /// <summary>
        /// <para>0 - OriginLat</para>
        /// <para>1 - OriginLng</para>
        /// <para>2 - Limit</para>
        /// <para>3 - ClubId</para>
        /// <para>4 - StateId</para>
        /// </summary>
        public const string NEW_BUSINESSES_BY_STATE_KEY = "UHack.SERVICES.NEW_BUSINESSES.{0}.{1}.{2}.{3}.{4}";

        /// <summary>
        /// <para>0 - OriginLat</para>
        /// <para>1 - OriginLng</para>
        /// </summary>
        public const string ACTIVE_BUSINESSES_BY_LOCATION_KEY = "UHack.SERVICES.ACTIVE_BUSINESSES_BY_LOCATION.{0}.{1}";
    }
}

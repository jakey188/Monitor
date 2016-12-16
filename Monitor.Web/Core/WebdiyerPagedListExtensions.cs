using System.Collections.Generic;

namespace Monitor.Web.Core
{
    public static class WebdiyerPagedListExtensions
    {
        /// <summary>
        /// 转换为分页PagedList
        /// </summary>
        /// <typeparam name="T">当前对象</typeparam>
        /// <param name="allItems">当前集合</param>
        /// <param name="allcount">集合数量</param>
        /// <param name="pageIndex">当前页码</param>
        /// <param name="pageSize">每页数量</param>
        /// <returns>PagedList</returns>
        public static Webdiyer.WebControls.Mvc.PagedList<T> ToPagedList<T>(this IEnumerable<T> allItems, int allcount, int pageIndex, int pageSize)
        {
            pageIndex = pageIndex < 1 ? 1 : pageIndex;
            var itemIndex = (pageIndex - 1) * pageSize;
            var pageOfItems = allItems;
            var totalItemCount = allcount;
            return new Webdiyer.WebControls.Mvc.PagedList<T>(pageOfItems, pageIndex, pageSize, totalItemCount);
        }
    }
}

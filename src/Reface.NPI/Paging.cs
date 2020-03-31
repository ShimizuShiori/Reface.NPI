namespace Reface.NPI
{
    public class Paging
    {
        /// <summary>
        /// 分页大小，即每页显示数量
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// 页数，0表示第一页
        /// </summary>
        public int PageIndex { get; set; }
    }
}

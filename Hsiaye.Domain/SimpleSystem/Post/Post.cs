using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hsiaye.Domain.SimpleSystem.Post
{
    // 帖子
    public class Post
    {
        public int Id { get; set; }
        public int OrganizationUnitId { get; set; }
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 作者Id
        /// </summary>
        public int MemberId { get; set; }
        /// <summary>
        /// 作者姓名
        /// </summary>
        public string MemberName { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string Cover { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string Tags { get; set; }//标签，以半角逗号隔开
        public PostState State { get; set; }
        public int CommentCount { get; set; }//评论量
        public int ViewCount { get; set; }//浏览量
    }
    public enum PostState : byte
    {
        Draft = 0,//草稿
        Published = 1//已发布
    }
}

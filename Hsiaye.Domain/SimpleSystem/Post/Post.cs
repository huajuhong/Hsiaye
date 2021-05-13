using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hsiaye.Domain
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
        //page.comments	留言是否开启
        //page.excerpt 页面摘要
        //page.link 文章的外部链接（用于链接文章）
    }
    public enum PostState : byte
    {
        草稿 = 0,
        待审 = 1,
        已发布 = 2
    }
}

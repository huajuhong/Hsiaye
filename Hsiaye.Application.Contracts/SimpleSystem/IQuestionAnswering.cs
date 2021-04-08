using Hsiaye.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Hsiaye.Application.Contracts
{
    public interface IQuestionAnswering
    {
        //创建问题
        void CreateQuestion(string title, string description);
        //获取问题列表
        List<Question> GetQuestions(string keywords, int page, int limit, Expression<Func<Question, object>> sort, ref int total);
        //获取问题详情
        GetQuestionOutput GetQuestion(int id);
        //对问题投票，表示遇到该问题且支持
        void Vote(int questionId);
        //创建答案
        void CreateAnswer(int questionId, string description);
        //接受答案
        void AcceptedAnswer(int questionId, int answerId);
    }
}

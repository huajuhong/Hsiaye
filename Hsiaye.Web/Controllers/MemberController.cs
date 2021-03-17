using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hsiaye.Web.Controllers
{
    public class MemberController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Login()
        {
            return null;
        }

        //1.当前用户列表（包含拥有的角色数组）
        //  当前用户可管理的角色

        //2.当前角色列表（包含拥有的权限数组）
        //  
    }
}

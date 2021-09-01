{
  "Code": 200,
  "Message": "",
  "Data": [
    {
      "title": "主页",
      "icon": "layui-icon-home",
      "list": [
        {
          "title": "控制台",
          "jump": "/"
        },
        {
          "name": "homepage1",
          "title": "主页一",
          "jump": "home/homepage1"
        },
        {
          "name": "homepage2",
          "title": "主页二",
          "jump": "home/homepage2"
        }
      ]
    },
    {
      "name": "template",
      "title": "页面",
      "icon": "layui-icon-template",
      "list": [
        {
          "name": "personalpage",
          "title": "个人主页",
          "jump": "template/personalpage"
        },
        {
          "name": "addresslist",
          "title": "通讯录",
          "jump": "template/addresslist"
        },
        {
          "name": "caller",
          "title": "客户列表",
          "jump": "template/caller"
        },
        {
          "name": "goodslist",
          "title": "商品列表",
          "jump": "template/goodslist"
        },
        {
          "name": "msgboard",
          "title": "留言板",
          "jump": "template/msgboard"
        },
        {
          "name": "search",
          "title": "搜索结果",
          "jump": "template/search"
        },
        {
          "name": "reg",
          "title": "注册",
          "jump": "user/reg"
        },
        {
          "name": "login",
          "title": "登入",
          "jump": "user/login"
        },
        {
          "name": "forget",
          "title": "忘记密码",
          "jump": "user/forget"
        },
        {
          "name": "404",
          "title": "404",
          "jump": "template/tips/404"
        },
        {
          "name": "error",
          "title": "错误提示",
          "jump": "template/tips/error"
        },
        {
          "name": "",
          "title": "内嵌页面",
          "spread": true,
          "list": [
            {
              "name": "",
              "title": "百度一下",
              "jump": "/iframe/link/baidu"
            }
          ]
        }
      ]
    },
    {
      "name": "app",
      "title": "应用",
      "icon": "layui-icon-app",
      "list": [
        {
          "name": "SelfStudyRoom",
          "title": "自习室系统",
          "list": [
            {
              "name": "SeatSubject",
              "title": "自习学科",
              "jump": "app/SelfStudyRoom/SeatSubject/list"
            },
            {
              "name": "SeatCategory",
              "title": "座位分类",
              "jump": "app/SelfStudyRoom/SeatCategory/list"
            },
            {
              "name": "Seat",
              "title": "现有座位",
              "jump": "app/SelfStudyRoom/Seat/list"
            },
            {
              "name": "SeatReservation",
              "title": "座位预约",
              "jump": "app/SelfStudyRoom/SeatReservation/list"
            }
          ]
        },
        {
          "name": "content",
          "title": "内容系统",
          "list": [
            {
              "name": "list",
              "title": "文章列表"
            },
            {
              "name": "tags",
              "title": "分类管理"
            },
            {
              "name": "comment",
              "title": "评论管理"
            }
          ]
        },
        {
          "name": "forum",
          "title": "社区系统",
          "list": [
            {
              "name": "list",
              "title": "帖子列表"
            },
            {
              "name": "replys",
              "title": "回帖列表"
            }
          ]
        },
        {
          "name": "message",
          "title": "消息中心"
        },
        {
          "name": "workorder",
          "title": "工单系统",
          "jump": "app/workorder/list"
        }
      ]
    },
    {
      "name": "get",
      "title": "关于",
      "icon": "layui-icon-auz",
      "jump": "system/about"
    }
  ]
}
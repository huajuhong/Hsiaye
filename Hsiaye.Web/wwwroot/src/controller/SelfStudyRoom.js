layui.define(['table', 'form'], function (exports) {
  var $ = layui.$
    , admin = layui.admin
    , view = layui.view
    , table = layui.table
    , form = layui.form;

  //座位分类
  table.render({
    elem: '#LAY-app-SeatCategory-list'
    , url: '/api/SelfStudyRoom/SeatCategory_List'
    , cols: [[
      { type: 'checkbox', fixed: 'left' }
      , { field: 'Id', width: 80, title: 'ID', sort: true }
      , { field: 'CreateTime', title: '时间' }
      , { field: 'Name', title: '名称' }
      , { field: 'BeginTime', title: '开始时段', width: 100 }
      , { field: 'EndTime', title: '结束时段', width: 100 }
      , { field: 'Price', title: '价格', sort: true }
      , { field: 'Description', title: '说明', minWidth: 80, align: 'center' }
      , { field: 'Normal', title: '状态', templet: '#buttonTpl-Normal' }
      , { title: '操作', width: 150, align: 'center', fixed: 'right', toolbar: '#table-SeatCategory-list' }
    ]]
    , page: true
    , limit: 10
    , limits: [10, 15, 20, 25, 30]
    , text: '对不起，加载出现异常！'
  });

  //工具条
  table.on('tool(LAY-app-SeatCategory-list)', function (obj) {
    var data = obj.data;
    if (obj.event === 'del') {
      layer.confirm('确定删除此条帖子？', function (index) {
        obj.del();
        layer.close(index);
      });
    } else if (obj.event === 'edit') {
      admin.popup({
        title: '编辑帖子'
        , area: ['550px', '450px']
        , id: 'LAY-popup-SeatCategory-edit'
        , resize: false
        , success: function (layero, index) {
          view(this.id).render('app/SeatCategory/listform', data).done(function () {
            form.render(null, 'layuiadmin-form-list');

            //提交
            form.on('submit(layuiadmin-app-SeatCategory-submit)', function (data) {
              var field = data.field; //获取提交的字段

              //提交 Ajax 成功后，关闭当前弹层并重载表格
              //$.ajax({});
              layui.table.reload('LAY-app-SeatCategory-list'); //重载表格
              layer.close(index); //执行关闭 
            });
          });
        }
      });
    }
  });



  exports('SelfStudyRoom', {})
});
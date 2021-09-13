layui.define(['table', 'form', 'laydate'], function (exports) {
  var $ = layui.$
    , admin = layui.admin
    , view = layui.view
    , table = layui.table
    , form = layui.form
    , laydate = layui.laydate;

  //自习学科
  table.render({
    elem: '#LAY-app-list'
    , url: '/api/SelfStudyRoom/SeatSubject_List'
    , cols: [[
      { field: 'Id', width: 80, title: 'ID', sort: true }
      , { field: 'Name', title: '名称' }
      , { field: 'Description', title: '说明', minWidth: 80, align: 'center' }
      , { field: 'Normal', title: '状态', templet: '#buttonTpl-Normal' }
      , { field: 'CreateTime', title: '创建时间' }
      , { title: '操作', width: 150, align: 'center', fixed: 'right', toolbar: '#table-list' }
    ]]
    , page: true
    , limit: 10
    , limits: [10, 15, 20, 25, 30]
    , text: '对不起，加载出现异常！'
  });

  //工具条
  table.on('tool(LAY-app-list)', function (obj) {
    var data = obj.data;
    if (obj.event === 'del') {
      layer.confirm('确定删除此条记录？', function (index) {
        //提交 Ajax 成功后，关闭当前弹层并重载表格
        admin.req({
          url: '/api/SelfStudyRoom/SeatSubject_Delete?id=' + data.Id
          , type: 'post'
          , done: function (res) {
            obj.del();
            layui.table.reload('LAY-app-list'); //重载表格
            layer.close(index); //执行关闭 
          }
        });
      });
    } else if (obj.event === 'edit') {
      admin.popup({
        title: '编辑'
        , area: ['550px', '550px']
        , id: 'LAY-popup-edit'
        , resize: false
        , success: function (layero, index) {
          view(this.id).render('app/SelfStudyRoom/SeatSubject/listform', data).done(function () {
            form.val('layuiadmin-form-list', data);
            form.render(null, 'layuiadmin-form-list');
            //提交
            form.on('submit(layuiadmin-app-submit)', function (data) {
              var field = data.field;
              field.Id = obj.data.Id;
              //提交 Ajax 成功后，关闭当前弹层并重载表格
              admin.req({
                url: '/api/SelfStudyRoom/SeatSubject_Update'
                , type: 'post'
                , contentType: 'application/json'
                , data: field
                , done: function (res) {
                  layui.table.reload('LAY-app-list'); //重载表格
                  layer.close(index); //执行关闭 
                }
              });
            });
          });
        }
      });
    }
  });

  //座位分类
  table.render({
    elem: '#LAY-app-SeatCategory-list'
    , url: '/api/SelfStudyRoom/SeatCategory_List'
    , cols: [[
      { field: 'Id', width: 80, title: 'ID', sort: true }
      , { field: 'Name', title: '名称' }
      , { field: 'Description', title: '说明', minWidth: 80, align: 'center' }
      , { field: 'Normal', title: '状态', templet: '#buttonTpl-Normal' }
      , { field: 'CreateTime', title: '创建时间' }
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
      layer.confirm('确定删除此条记录？', function (index) {
        //提交 Ajax 成功后，关闭当前弹层并重载表格
        admin.req({
          url: '/api/SelfStudyRoom/SeatCategory_Delete?id=' + data.Id
          , type: 'post'
          , done: function (res) {
            obj.del();
            layui.table.reload('LAY-app-SeatCategory-list'); //重载表格
            layer.close(index); //执行关闭 
          }
        });
      });
    } else if (obj.event === 'edit') {
      admin.popup({
        title: '编辑'
        , area: ['550px', '550px']
        , id: 'LAY-popup-SeatCategory-edit'
        , resize: false
        , success: function (layero, index) {
          view(this.id).render('app/SelfStudyRoom/SeatCategory/listform', data).done(function () {
            form.val('layuiadmin-form-list', data);
            form.render(null, 'layuiadmin-form-list');
            //提交
            form.on('submit(layuiadmin-app-SeatCategory-submit)', function (data) {
              var field = data.field;
              field.Id = obj.data.Id;
              //提交 Ajax 成功后，关闭当前弹层并重载表格
              admin.req({
                url: '/api/SelfStudyRoom/SeatCategory_Update'
                , type: 'post'
                , contentType: 'application/json'
                , data: field
                , done: function (res) {
                  layui.table.reload('LAY-app-SeatCategory-list'); //重载表格
                  layer.close(index); //执行关闭 
                }
              });
            });
          });
        }
      });
    }
  });


  //座位
  table.render({
    elem: '#LAY-app-Seat-list'
    , url: '/api/SelfStudyRoom/Seat_List'
    , cols: [[
      { field: 'Id', width: 80, title: 'ID', sort: true }
      , { field: 'SeatCategoryId', width: 160, title: '分类', templet: '#textTpl-SeatCategory' }
      , { field: 'Name', width: 160, title: '名称' }
      , { field: 'BeginTime', title: '开始时段', width: 100 }
      , { field: 'EndTime', title: '结束时段', width: 100 }
      , { field: 'Price', width: 120, title: '价格（元）', sort: true }
      , { field: 'Description', title: '说明' }
      , { field: 'Normal', width: 80, title: '状态', templet: '#buttonTpl-Normal' }
      , { field: 'CreateTime', width: 160, title: '创建时间' }
      , { title: '操作', width: 150, align: 'center', fixed: 'right', toolbar: '#table-Seat-list' }
    ]]
    , page: true
    , limit: 10
    , limits: [10, 15, 20, 25, 30]
    , text: '对不起，加载出现异常！'
  });

  //工具条
  table.on('tool(LAY-app-Seat-list)', function (obj) {
    var data = obj.data;
    if (obj.event === 'del') {
      layer.confirm('确定删除此条记录？', function (index) {
        //提交 Ajax 成功后，关闭当前弹层并重载表格
        admin.req({
          url: '/api/SelfStudyRoom/Seat_Delete?id=' + data.Id
          , type: 'post'
          , done: function (res) {
            obj.del();
            layui.table.reload('LAY-app-Seat-list'); //重载表格
            layer.close(index); //执行关闭 
          }
        });
      });
    } else if (obj.event === 'edit') {
      admin.popup({
        title: '编辑'
        , area: ['600px', '600px']
        , id: 'LAY-popup-Seat-edit'
        , resize: false
        , success: function (layero, index) {
          view(this.id).render('app/SelfStudyRoom/Seat/listform', data).done(function () {
            form.val('layuiadmin-form-list', data);
            form.render(null, 'layuiadmin-form-list');
            //提交
            form.on('submit(layuiadmin-app-Seat-submit)', function (data) {
              var field = data.field;
              field.Id = obj.data.Id;
              //提交 Ajax 成功后，关闭当前弹层并重载表格
              admin.req({
                url: '/api/SelfStudyRoom/Seat_Update'
                , type: 'post'
                , contentType: 'application/json'
                , data: field
                , done: function (res) {
                  layui.table.reload('LAY-app-Seat-list'); //重载表格
                  layer.close(index); //执行关闭 
                }
              });
            });
          });
        }
      });
    }
  });

  //预约
  table.render({
    elem: '#LAY-app-SeatReservation-list'
    , url: '/api/SelfStudyRoom/SeatReservation_List'
    , cols: [[
      // { type: 'checkbox', fixed: 'left' }
      // , { field: 'Id', width: 80, title: 'ID', sort: true }
      { field: 'Name', width: 170, title: '姓名-电话', templet: function (d) { return d.Name + '-' + d.Phone; } }
      , { field: 'Begin', width: 190, title: '开始-结束日期', templet: function (d) { return d.Begin.substring(0, 10) + ' ~ ' + d.End.substring(0, 10); } }
      , { field: 'SeatId', title: '座位', templet: '#textTpl-Seat' }
      , { field: 'SeatSubjectId', width: 100, title: '科目', templet: function (d) { return d.SeatSubject.Name; } }
      , { field: 'Description', title: '说明' }
      // , { field: 'OperatorId', width: 80, title: '操作者ID' }
      // , { field: 'OperatorRemark', title: '操作者备注' }
      , { field: 'Normal', width: 70, title: '状态', templet: '#buttonTpl-Normal' }
      , { field: 'Reported', width: 110, title: '签到', templet: '#buttonTpl-Reported' }
      , { field: 'Paid', width: 110, title: '付款', templet: '#buttonTpl-Paid' }
      , { field: 'CreateTime', width: 160, title: '创建时间' }
      , { title: '操作', width: 150, align: 'center', fixed: 'right', toolbar: '#table-SeatReservation-list' }
    ]]
    , page: true
    , limit: 10
    , limits: [10, 15, 20, 25, 30]
    , text: '对不起，加载出现异常！'
  });

  //工具条
  table.on('tool(LAY-app-SeatReservation-list)', function (obj) {
    var data = obj.data;
    if (obj.event === 'del') {
      layer.confirm('确定删除此条记录？', function (index) {
        //提交 Ajax 成功后，关闭当前弹层并重载表格
        admin.req({
          url: '/api/SelfStudyRoom/SeatReservation_Delete?id=' + data.Id
          , type: 'post'
          , done: function (res) {
            obj.del();
            layui.table.reload('LAY-app-SeatReservation-list'); //重载表格
            layer.close(index); //执行关闭 
          }
        });
      });
    } else if (obj.event === 'edit') {
      admin.popup({
        title: '编辑'
        , area: ['650px', '850px']
        , id: 'LAY-popup-SeatReservation-edit'
        , resize: false
        , success: function (layero, index) {
          view(this.id).render('app/SelfStudyRoom/SeatReservation/listform', data).done(function () {
            data.Begin = data.Begin.substring(0, 10);
            data.End = data.End.substring(0, 10);
            form.val('layuiadmin-form-list', data);
            form.render(null, 'layuiadmin-form-list');
            //提交
            form.on('submit(layuiadmin-app-SeatReservation-submit)', function (data) {
              var field = data.field;
              field.Id = obj.data.Id;
              //提交 Ajax 成功后，关闭当前弹层并重载表格
              admin.req({
                url: '/api/SelfStudyRoom/SeatReservation_Update'
                , type: 'post'
                , contentType: 'application/json'
                , data: field
                , done: function (res) {
                  layui.table.reload('LAY-app-SeatReservation-list'); //重载表格
                  layer.close(index); //执行关闭 
                }
              });
            });
          });
        }
      });
    }
  });

  //监听签到操作
  form.on('checkbox(table-Reported)', function (obj) {
    var value = this.value;
    var checked = obj.elem.checked;
    //提交 Ajax 成功后，关闭当前弹层并重载表格
    admin.req({
      url: '/api/SelfStudyRoom/SeatReservation_Reported?id=' + value + '&value=' + checked
      , type: 'post'
      , done: function (res) {
        layui.table.reload('LAY-app-SeatReservation-list'); //重载表格
        layer.msg(checked ? '签到成功' : '等待签到', { icon: checked ? 1 : 0 });
      }
    });
  });

  //监听付款操作
  form.on('checkbox(table-Paid)', function (obj) {
    var value = this.value;
    var checked = obj.elem.checked;
    //提交 Ajax 成功后，关闭当前弹层并重载表格
    admin.req({
      url: '/api/SelfStudyRoom/SeatReservation_Paid?id=' + value + '&value=' + checked
      , type: 'post'
      , done: function (res) {
        layui.table.reload('LAY-app-SeatReservation-list'); //重载表格
        layer.msg(checked ? '付款成功' : '等待付款', { icon: checked ? 1 : 0 });
      }
    });
  });


  exports('SelfStudyRoom', {})
});
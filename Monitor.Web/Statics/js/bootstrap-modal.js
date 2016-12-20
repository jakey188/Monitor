var modal = {
    show: function (options) {
        var html = '<div class="modal fade" id="appModal" tabindex="-1" role="dialog" aria-labelledby="myModalLabel">' +
                '  <div class="modal-dialog">' +
                '    <div class="modal-content">' +
                '      <div class="modal-header">' +
                '        <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>' +
                '        <h4 class="modal-title" id="myModalLabel">模态框</h4>' +
                '      </div>' +
                '      <div class="modal-body">' +
                '      </div>' +
                '      <div class="modal-footer">' +
                '      </div>' +
                '    </div>' +
                '  </div>' +
                '</div>';
        $('#appModal').remove();
        $('body').append(html);
        var title = options.title || ''; //模态框的名称，显示在 #myModalLabel 处
        var width = options.width || ''; //模态框的宽度
        var buttons = options.buttons || []; //模态框的按钮
        var content = options.content || ''; //模态框的内容，显示在 .modal-body 处
        var url = options.url; //以Jquery.load(url)的方式去加载模态框的内容，若设置了这个选项，content 选项将无效
        var modal = $('#appModal');
        if (title.length === 0) {
            modal.find('.modal-header').remove();
        } else {
            modal.find('.modal-title').text(title);
        }
        modal.find('.modal-dialog').width(width);
        if (url) {
            modal.find('.modal-body').load(url);
        } else {
            modal.find('.modal-body').html(content);
        }
        if (buttons.length > 0) {
            var footer = '';
            for (var i in buttons) {
                var btnClass = buttons[i].class || 'btn btn-default';
                footer += '<button class="' + btnClass + '">' + buttons[i].text + '</button>';
            }
            modal.find('.modal-footer').html(footer);
            modal.find('.modal-footer button').each(function (i, btn) {
                $(btn).on('click', buttons[i].click);

            });
        } else {
            modal.find('.modal-footer').remove();
        }
        $('#appModal').modal("show");
    }
}

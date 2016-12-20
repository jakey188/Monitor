function getTotalPage(total, pageSize) {
    return (total + pageSize - 1) / pageSize;
}

function loading(value) {
    if (typeof (value) !== 'undefined' && !value) {
        setTimeout(function () {
            $(".showbox").stop(true).animate({ 'margin-top': '250px', 'opacity': '0' }, 400);
            $(".overlay").css({ 'display': 'none', 'opacity': '0' });
            console.log('loading close');
        }, 400);
    } else {
        var h = $(document).height();
        $(".overlay").css({ "height": h });
        $(".overlay").css({ 'display': 'block', 'opacity': '0.8' });
        $(".showbox").stop(true).animate({ 'margin-top': '300px', 'opacity': '1' }, 200);
        console.log('loading start');
    }
}

var dialog = {
    alert: function (content) {
        modal.show({
            title: '提示',
            content: content,
            buttons: [{
                text: '确认',
                class: 'btn btn-primary',
                click: function () {
                    $('#appModal').modal('hide');
                }
            }]
        });
    },
    confirm: function (content, callback, failCallback) {
        modal.show({
            title: '提示',
            content: content,
            buttons: [{
                text: '确认',
                class: 'btn btn-primary',
                click: function () {
                    callback && callback();
                    $('#appModal').modal('hide');
                }
            }, {
                text: '取消',
                class: 'btn btn-danger',
                click: function () {
                    failCallback && failCallback();
                    $('#appModal').modal('hide');
                }
            }]
        });
    }
}


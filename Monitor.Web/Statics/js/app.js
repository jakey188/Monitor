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

var app = {
    alert: function (content,callback) {
        BootstrapDialog.alert({
            title: '提示',
            closable: true, // <-- Default value is false
            draggable: true, // <-- Default value is false
            message: content,
            size: BootstrapDialog.SIZE_SMALL,
            type: BootstrapDialog.TYPE_INFO,
            callback: callback
        })
    },
    confirm: function (content, callback) {
        BootstrapDialog.confirm({
            title: '提示',
            message: content,
            btnOKLabel: '确认',
            btnCancelLabel:'取消',
            type: BootstrapDialog.TYPE_WARNING,
            callback: callback
        });
    },
    tips: function (content, callback) {
        var dialog = new BootstrapDialog({
            title: '提示',
            size: BootstrapDialog.SIZE_SMALL,
            message: content
        });
        dialog.open();
        setTimeout(function () {
            dialog.close();
            callback();
        }, 2000);
    },
    dialog: function (options) {
        var dialog = new BootstrapDialog(options);
        dialog.open();
        return dialog;
    }
}


﻿$.validator.setDefaults({
    highlight: function (element) {
        $(element).closest('.form-group').addClass('has-error');
    },
    unhighlight: function (element) {
        $(element).closest('.form-group').removeClass('has-error');
    },
    //highlight: function (element, errorClass, validClass) {
    //    if (element.type === "radio") {
    //        this.findByName(element.name).addClass(errorClass).removeClass(validClass);
    //    } else {
    //        $(element).closest('.form-group').removeClass('has-success has-feedback').addClass('has-error has-feedback');
    //        $(element).closest('.form-group').find('span.glyphicon').remove();
    //        $(element).closest('.form-group').append('<span class="glyphicon glyphicon-remove form-control-feedback" aria-hidden="true"></span>');
    //    }
    //},
    //unhighlight: function (element, errorClass, validClass) {
    //    if (element.type === "radio") {
    //        this.findByName(element.name).removeClass(errorClass).addClass(validClass);
    //    } else {
    //        $(element).closest('.form-group').removeClass('has-error has-feedback').addClass('has-success has-feedback');
    //        $(element).closest('.form-group').find('span.glyphicon').remove();
    //        $(element).closest('.form-group').append('<span class="glyphicon glyphicon-ok form-control-feedback" aria-hidden="true"></span>');
    //    }
    //},
    errorElement: 'span',
    errorClass: 'help-block',
    errorPlacement: function (error, element) {
        if (element.parent('.input-group').length || element.prop('type') === 'checkbox' || element.prop('type') === 'radio') {
            error.insertAfter(element.parent());
        } else {
            error.insertAfter(element);
        }
    }
});
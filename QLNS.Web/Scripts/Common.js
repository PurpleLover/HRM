function NotiSuccess(title, message) {
    $.gritter.add({
        // (string | mandatory) the heading of the notification
        title: title,
        time: 2000,
        sticky: false,
        // (string | mandatory) the text inside the notification
        text: message,
        class_name: 'gritter-success'
    });
}
function NotiError(title, message) {
    $.gritter.add({
        // (string | mandatory) the heading of the notification
        title: title,
        time: 2000,
        sticky: false,
        // (string | mandatory) the text inside the notification
        text: message,
        class_name: 'gritter-error'
    });
}

function AfterSussessActionAjaxform() {
    location.reload();
}

function AjaxSearchSuccess(rs) {
    location.reload();
}

function AjaxFormSuccess(rs) {
    if (rs.Status) {

        $("#MasterModal").modal("hide");
        $("#MasterModal").empty();
        NotiSuccess("Thành công", "Cập nhật dữ liệu thành công");
        AfterSussessActionAjaxform();
    } else {
        NotiError("Lỗi xử lý", rs.Message);
    }
}
function AjaxFormError() {
    NotiError("Có lỗi xảy ra", "Vui lòng kiểm tra lại thông tin");
}

function AjaxCall(url, type, data, callback, callbackError) {
    var isfunction = callback && typeof (callback) == "function";
    if (!isfunction) {
        callback = function () {
            console.log("Chưa cài đặt sự kiện thành công");
        }
    }
    var isfunction = callbackError && typeof (callbackError) == "function";
    if (!isfunction) {
        callbackError = function () {
            NotiError("Thao tác không thể thực hiện");
        }
    }
    $.ajax({
        url: url,
        type: type,
        data: data,
        success: callback,
        error: callbackError
    })

}
function EditAction(url) {
    AjaxCall(url, 'get', null, function (rs) {
        $("#MasterModal").html(rs);
        $("#MasterModal").modal("show");

    })
}
function CreateAction(url) {
    AjaxCall(url, 'get', null, function (rs) {
        $("#MasterModal").html(rs);
        $("#MasterModal").modal("show");

    })
}

function OpenModalAction(url) {
    AjaxCall(url, 'get', null, function (rs) {
        $("#MasterModal").html(rs);
        $("#MasterModal").modal("show");

    })
}

function DeleteAction(url, mesage) {
    if (mesage == null || mesage == '') {
        mesage = "Bạn xác nhận thực hiện thao tác này ?";
    }
    bootbox.confirm(mesage, function (result) {
        if (result) {
            AjaxCall(url, 'get', null, function (rs) {
                if (rs.Status) {
                    NotiSuccess("Thành công", rs.Message);
                    AfterSussessActionAjaxform();
                } else {
                    NotiError("Lỗi xử lý", rs.Message);
                }
            })
        }
    });
}

function ConfirmAction(url,type,data, mesage) {
    if (mesage == null || mesage == '') {
        mesage = "Bạn xác nhận thực hiện thao tác này ?";
    }
    bootbox.confirm(mesage, function (result) {
        if (result) {
            AjaxCall(url, type, data, function (rs) {
                if (rs.Status) {
                    NotiSuccess("Thành công", rs.Message);
                    AfterSussessActionAjaxform();
                } else {
                    NotiError("Lỗi xử lý", rs.Message);
                }
            })
        }
    });
}

/**
 * @author:duynn
 * @create_date: 19/04/2019
 * @param {any} url
 * @param {any} mesage
 */
function onDelete(url, mesage) {
    if (mesage == null || mesage == '') {
        mesage = "Bạn xác nhận thực hiện thao tác này ?";
    }
    bootbox.confirm(mesage, function (result) {
        if (result) {
            AjaxCall(url, 'delete', null, function (result) {
                if (result.Status) {
                    NotiSuccess("Thành công", result.Message);
                    AfterSussessActionAjaxform();
                } else {
                    NotiError("Lỗi xử lý", result.Message);
                }
            })
        }
    });
}


function ToDate(obj) {
    if (obj == null) {
        return "";
    } else {

        if (obj.indexOf('Date') >= 0) {
            var dateint = parseInt(obj.match(/\d+/)[0]);
           
            obj = new Date(dateint);
        } else {
            obj = new Date(obj);
        }
        var mon = '';
        if ((obj.getMonth() + 1) < 10) {
            mon = "0" + (obj.getMonth() + 1);
        } else {
            mon = (obj.getMonth() + 1);
        }
        var day = "";
        if (obj.getDate() < 10) {
            day = '0' + obj.getDate();
        } else {
            day = obj.getDate();
        }
        var date_string = day + "/" + mon + "/" + obj.getFullYear();
        return date_string;

    }
}


function ToDateTime(obj) {
    if (obj == null) {
        return "";
    } else {

        if (obj.indexOf('Date') >= 0) {
            var dateint = parseInt(obj.match(/\d+/)[0]);
            obj = new Date(dateint);
        } else {
            obj = new Date(obj);
        }
        var mon = '';
        if ((obj.getMonth() + 1) < 10) {
            mon = "0" + (obj.getMonth() + 1);
        } else {
            mon = (obj.getMonth() + 1);
        }
        var day = "";
        if (obj.getDate() < 10) {
            day = '0' + obj.getDate();
        } else {
            day = obj.getDate();
        }

        var hour = obj.getHours();
        if (hour < 10) {
            hour = "0" + hour;
        }
        var minute = obj.getMinutes()
        if (minute < 10) {
            minute = "0" + minute;
        }
        var date_string = day + "/" + mon + "/" + obj.getFullYear() + " " + hour + ":" + minute;
        return date_string;

    }
}


function onAddNewRow(element) {
    var parent = $(element).closest('table');
    if (!parent) {
        return;
    }
    var request = $(element).data('request');
    if (!request) {
        return;
    }

    $.get(request,function (result) {
        parent.find('tbody').append(result);
    })
}

function onRemoveRow(element) {
    var parent = $(element).closest('tr');
    if (!parent) {
        return;
    }
    $(parent).remove();
}

function validateRequired(container) {
    var isValid = true;
    $("#" + container + " .required").each(function () {
        var parent = $(this).parents(".form-group").first();
        if (parent.length == 0) {
            parent = $(this).parent();
        }
        var errorText = parent.find(".error");
        if ($(this).val() == null || $(this).val().length == 0 || $(this).val().toString().trim() == "") {
            errorText.addClass('error-required');
            isValid = false;
        } else {
            errorText.removeClass('error-required');
        }
    });
    return isValid;
}

function validateDate(container) {
    var isValid = true;
    var pattern = /^[0-3][0-9]\/[01][0-9]\/[12][0-9][0-9][0-9]$/;
    var inputs = $("#" + container + " .checkDateValid");

    inputs.each(function () {
        var parent = $(this).parents(".form-group").first();
        var errorText = parent.find(".error");
        if ($(this).val() != null &&
            $(this).val().length != 0 &&
            $(this).val().toString().trim() != "") {
            if (!pattern.test($(this).val())) {
                errorText.addClass("error-date-format");
                isValid = false;
            }
            else {
                errorText.removeClass("error-date-format");
            }
        }
    })
    return isValid;
}

function validateSelectOption(container) {
    var isValid = true;
    var inputs = $("#" + container + " select.requiredDropDownList");
    inputs.each(function () {
        var parent = $(this).parents(" .form-group").first();
        var errorText = parent.find(".error");
        if ($(this).val() == null || $(this).val().length == 0) {
            errorText.addClass('error-required-dropdown');
            isValid = false;
        } else {
            errorText.removeClass('error-required-dropdown');
        }
    })
    return isValid;
}

function validateTextArea(container) {
    var isValid = true;
    $("#" + container + " .requiredTextArea").each(function () {
        var parent = $(this).parents(" .form-group").first();
        var errorText = parent.find(".error");
        if ($(this).html() == null || $(this).html().trim() == "") {
            errText.addClass('error-required');
            isValid = false;
        } else {
            errorText.removeClass('error-required');
        }
    });
    return isValid;
}

function validateNumber(container) {
    var isValid = true;
    var inputs = $('#' + container + ' .checkIsNumeric');
    if (inputs.length > 0) {
        inputs.each(function () {
            var parent = $(this).parents('.form-group').first();
            var errorDOM = parent.find('.error');

            if ($(this).val() != null &&
                $(this).val().length != 0 &&
                $(this).val().toString().trim() != "") {

                if (!$.isNumeric($(this).val())) {
                    errorDOM.addClass("error-number-format");
                    isValid = false;
                } else {
                    errorDOM.removeClass("error-number-format");
                }
            }
        })
    }
    return isValid;
}

function validateHTMLInjection(container) {
    var isValid = true;
    var pattern = /<[a-z][\s\S]*>/i;
    var inputs = $('#' + container + ' .checkHTMLInjection');
    if (inputs.length > 0) {
        inputs.each(function () {
            var parent = $(this).parents('.form-group').first();
            var errorDOM = parent.find('.error');

            if ($(this).val() != null &&
                $(this).val().length != 0 &&
                $(this).val().toString().trim() != "") {

                if (pattern.test($(this).val())) {
                    errorDOM.addClass('error-html-format');
                    isValid = false;
                }
                else {
                    errorDOM.removeClass('error-html-format');
                }
            }
        })
    }
    return isValid;
}

function validateSpecialCharacter(container) {
    var isValid = true;
    var pattern = /[!@#$%^&*()_+\-=\[\]{};':"\\|,.<>\/?]/;
    var inputs = $('#' + container + ' .checkSpecialCharacter');
    if (inputs.length > 0) {
        inputs.each(function () {
            var parent = $(this).parents('.form-group').first();
            var errorDOM = parent.find('.error');

            if ($(this).val() != null &&
                $(this).val().length != 0 &&
                $(this).val().toString().trim() != "") {

                if (pattern.test($(this).val())) {
                    errorDOM.addClass('error-special-character');
                    isValid = false;
                }
                else {
                    errorDOM.removeClass('error-special-character');
                }
            }
        })
    }
    return isValid;
}

function validateForm(formId) {
    var error = 0;
    var isValid = true;
    error += this.validateRequired(formId) ? 0 : 1;
    error += this.validateDate(formId) ? 0 : 1;
    error += this.validateSelectOption(formId) ? 0 : 1;
    error += this.validateTextArea(formId) ? 0 : 1;
    error += this.validateNumber(formId) ? 0 : 1;
    error += this.validateHTMLInjection(formId) ? 0 : 1;
    error += this.validateSpecialCharacter(formId) ? 0 : 1;

    $('#' + formId).find('.error').each(function () {
        var message = '';
        if ($(this).hasClass('error-required')) {
            message = 'Vui lòng nhập thông tin';
        } else if ($(this).hasClass('error-date-format')) {
            message = 'Vui lòng nhập theo định dạng "ngày/tháng/năm"';
        } else if ($(this).hasClass('error-number-format')) {
            message = 'Vui lòng đúng định dạng số';
        } else if ($(this).hasClass('error-html-format')) {
            message = 'Vui lòng không nhập ký tự HTML';
        } else if ($(this).hasClass('error-required-dropdown')) {
            message = 'Vui lòng chọn thông tin';
        } else if ($(this).hasClass('error-special-character')) {
            message = 'Vui lòng không nhập ký tự đặc biệt';
        }

        if (message !== '') {
            $(this).html(message);
            $(this).css('display', 'inline');
        } else {
            $(this).css('display', 'none');
        }
    });

    if (error > 0) {
        isValid = false;
    } else {
        isValid = true;
    }
    return isValid;
}
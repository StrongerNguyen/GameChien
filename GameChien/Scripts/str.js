var Loading = {
    Show: function (element = "body") {
        $(element).append("<div class='loading'><div class='ripple'><div></div><div></div></div></div>");
    },
    Hide: function (element = "body") {
        $(element).find(".loading").remove();
    }
}
var Popup = {
    Show: function (title, body) {
        let element = '<div id="popup">';
        element += '<div class="popup-content">';
        if (title != '' && title != undefined) {
            element += '<div class="popup-head">';
            element += '<div class="popup-title">' + title + '</div>';
            element += '<div class="popup-button-close" onclick="document.getElementById(\'popup\').remove()"><i class="fa fa-remove"></i></div>';
            element += '</div>';
        }
        else {
            element += '<div class="popup-button-close" onclick="document.getElementById(\'popup\').remove()"><i class="fa fa-remove"></i></div>';
        }
        element += '<div class="popup-body">';
        element += body;
        element += '</div>';
        element += '</div>';
        element += '</div>';
        document.getElementsByTagName('body')[0].insertAdjacentHTML('afterbegin', element);
    },
    Hide: function () {
        var popup = document.getElementById("popup");
        if (popup != null) popup.remove();
    },
    AjaxShow: function (parentElement, title, ajaxUrl, ajaxData) {
        Loading.Show(parentElement);
        $.ajax({
            url: ajaxUrl,
            method: "GET",
            data: ajaxData,
            error: function (ex) {
                toastr.error(ex);
                Loading.Hide(parentElement);
            },
            success: function (rs) {
                Loading.Hide(parentElement);
                if (rs != undefined && rs != null) {
                    if (rs.success != undefined && rs.success != null) {
                        if (rs.success) {
                            Alert.Success(rs.message)
                        }
                        else {
                            Alert.Error(rs.message);
                        }
                    }
                    else {
                        $(parentElement).append(Popup.Show(title, rs));
                    }
                }
            }
        });
    }
}
var Alert = {
    Show: function (parentElement, isSuccess = null, message = null, func = null) {
        let element = '<div id="alert">';
        element += '<div class="alert-content">';

        element += '<div class="alert-button-close" onclick="document.getElementById(\'alert\').remove()"><i class="fa fa-remove"></i></div>';
        element += '<div class="alert-body">';
        if (isSuccess != undefined && isSuccess != null) {
            if (isSuccess) {
                //success
                element += '<div class="alert_icon text-success"><i class="fa fa-check" aria-hidden="true"></i></div>';
            }
            else {
                //info
                element += '<div class="alert_icon text-danger"><i class="fa fa-exclamation-triangle" aria-hidden="true"></i></div>';
            }
        }
        else {
            //info
            element += '<div class="alert_icon text-info"><i class="fa fa-info" aria-hidden="true"></i></div>';
        }
        element += message;
        element += '</div>';
        element += '</div>';
        element += '</div>';
        $(parentElement).append(element);
    },
    Hide: function () {
        var alert = document.getElementById("alert");
        if (alert != null) alert.remove();
    }
}
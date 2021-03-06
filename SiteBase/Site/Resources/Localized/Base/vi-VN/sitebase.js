// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

$.sb.localization = {
    culture: 'vi-VN',
    closeLabel: 'Đóng',
    messageHeading: 'Tin Nhắn',
    errorHeading: 'Cái gì bị sai...',
    errorText: 'Có lỗi khi hoàn thành yêu cầu này.',
    loadingText: 'Sự bỏ vào',
    noDataText: 'Không có sẵn nội dung',
    confirmText: 'Bạn có chắc chắn?',
    formAlreadySubmitted: 'Hình thức này đã được gửi. Xin vui lòng làm mới trang này để cố gắng yêu cầu này một lần nữa.'
};
modalboxLocalizedStrings = {
    messageCloseWindow: $.sb.localization.closeLabel,
    messageAjaxLoader: $.sb.localization.loadingText,
    errorMessageIfNoDataAvailable: $.sb.localization.noDataText,
    errorMessageXMLHttpRequest: $.sb.localization.errorText,
    errorMessageTextStatusError: $.sb.localization.errorText
};
$.digitalbeacon.modalBox.defaults.loadingText = $.sb.localization.loadingText;
$.digitalbeacon.modalBox.defaults.noDataText = $.sb.localization.noDataText;
if ($.fn.tGrid) {
    $.fn.tGrid.defaults.localization.displayingItems = 'Hiển thị các mục {0} - {1} của {2}';
}
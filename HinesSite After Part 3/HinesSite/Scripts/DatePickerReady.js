$(function() {
    (function () {
        var elem = document.createElement('input');
        elem.setAttribute('type', 'date');

        if (elem.type === 'text') {
            $(".datefield").datepicker(
            {
                autoclose: true,

                gotoCurrent: true,

                maxDate: -0,
                minDate: -0,
                
                showAnim: 'clip',

                selectOtherMonths: true,
                showOtherMonths: true,

                todayHighlight: true
                
            }).change(function () {
                var date = new Date($(".datefield").val());
                var now = new Date();
                if (date != now) {
                    alert("Created On field must be today's date.");
                }
            });
        }
    })();



    
});
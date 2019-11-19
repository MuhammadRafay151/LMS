function ModelOnclick(row) {
    var text = document.getElementById('t1').rows[row.rowIndex].cells[0].innerHTML;
    document.getElementById('edittxt').value = text;

}
function ModelOnclick(row, id) {
    var text = document.getElementById('t1').rows[row.rowIndex].cells[0].innerHTML;
    document.getElementById('edittxt').value = text;
    document.getElementById('id').value = id;
    console.log(id);
}
function set_active(btn) {
    //this function is used to set active current page number in pagination bar
    //currently not attach with any element
    var x = $(btn).parent().children();

    for (var i = 0; i < x.length; i++) {
        $(x[i]).removeClass('active');
    }
    x = $(btn).addClass('active');

}
function GetCurrentPage() {
    var x = $('#paged').children();
    for (var i = 0; i < x.length; i++) {
        if ($(x[i]).hasClass('active')) {

            return parseInt(x[i].firstElementChild.innerHTML);
        }
    }

}

function Next(href) {
    //href eg /ViewApplications/All?PageNo=
    $("#next").attr("href", href + (GetCurrentPage() + 1));
}
function Pre(href) {
    if (GetCurrentPage() - 1 > 0) {
        $("#pre").attr("href", href + (GetCurrentPage() - 1));
    } else {
        $("#pre").attr("href", href + GetCurrentPage());
    }

}
function StateChange(ToggleBtn,id) {
   
    $.ajax({
        type: "post",
        url: "/Admin/RequestableStateChange",
        data: { LeaveTypeID: id, IsRequestable:ToggleBtn.checked }
      
    })
    //window.location.href = "/Admin/RequestableStateChange?LeaveTypeID=" + id + "&IsRequestable="+ToggleBtn.checked;
   
}
function GetBalance(Empid, row) {
   
   
    $.ajax({
        type: "post",
        url: "/ViewApplications/FacultyLeaveBalance?id=" + Empid,
        data: Empid,
        success: function (html) {
            $('#bal1').html(html);
            $('#EmpName').html(document.getElementById('t1').rows[row.rowIndex].cells[0].innerHTML);
        }

    })
   
}
function GetLeaveBalance() {
    $.ajax({
        type: "post",
        url: "/ViewApplications/LeaveBalance",
        success: function (html) {
            $('#bal1').html(html);
            $('#EmpNameHead').remove();
            $('#EmpName').remove();
        }

    })
}
function EmployeeStateChange(ToggleBtn, id) {
    var form = $(ToggleBtn).parent().parent();
    $.ajax({
        type: "post",
        url: "/Admin/EmployeeStateChange",
        data: { EmployeeID: id, IsActive: ToggleBtn.checked }

    })


}
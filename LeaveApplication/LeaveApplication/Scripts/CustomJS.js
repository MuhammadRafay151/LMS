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
    var x = $(btn).parent().children();

    for (var i = 0; i < x.length; i++) {
        $(x[i]).removeClass('active');
    }
    x = $(btn).addClass('active');

}
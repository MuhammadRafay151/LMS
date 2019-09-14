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

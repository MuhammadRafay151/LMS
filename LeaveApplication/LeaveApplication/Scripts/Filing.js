function GetFileExtension(InputId) {
    var x = document.getElementById(InputId);
    var name = x.files[0].name;
    var ext = name.split('.').pop();
    return ext;
}
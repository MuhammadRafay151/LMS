function IsValidImage(file) {
    var ImgFormat = ["jpg", "jpeg", "png"];
    var check = false;
    var fname = file.files[0].name;
    var array = fname.split(".");

    for (var x = 0; x < ImgFormat.length; x++) {
        if (array[array.length - 1] == ImgFormat[x]) {
            return true;
        }
    }
    return false;
}
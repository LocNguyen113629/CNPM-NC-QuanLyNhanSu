function ShowImagePreview(imageUploader, previewImage) 
{
    if (imageUploader.files && imageUploader.files[0]) {
        var reader = new FileReader();
        reader.onload = function (e) {
            $(previewImage).attr('src', e.tartget.result);
        }
        reader.readAsDataURL(imageUploader.files[0]);
    }
}
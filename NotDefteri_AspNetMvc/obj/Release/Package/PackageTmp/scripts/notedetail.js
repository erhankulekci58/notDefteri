$(function () {
    $('#model_notedetail').on('show.bs.modal', function (e) {

        var btn = $(e.relatedTarget);
        noteid = btn.data("note-id");

        $("#model_notedetail_body").load("/Note/GetNoteText/" + noteid);
    })
})
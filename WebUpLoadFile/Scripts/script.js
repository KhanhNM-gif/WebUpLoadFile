function readURL(input) {
  if (input.files && input.files[0]) {                      // if input is file, files has content
    var inputFileData = input.files[0];                     // shortcut
    var reader = new FileReader();                          // FileReader() : init
    reader.onload = function(e) {                           /* FileReader : set up ************** */
      console.log('e',e) 
        $('.file-upload-placeholder').hide();                 // call for action element : hide
        if (e.target.result.includes("data:image/jpeg")) {
            $('.file-upload-image').attr('src', e.target.result); // image element : set src data.
        }
        else {
            $('.file-upload-image').attr('src', "https://upload.wikimedia.org/wikipedia/commons/thumb/0/0c/File_alt_font_awesome.svg/512px-File_alt_font_awesome.svg.png"); // image element : set src data.
        }
      $('.file-upload-preview').show();                     // image element's container : show
      $('.image-title').html(inputFileData.name);           // set image's title
    };
    console.log('input.files[0]',input.files[0])
    reader.readAsDataURL(inputFileData);     // reads target inputFileData, launch `.onload` actions
  } else { removeUpload(); }
}

function removeUpload() {
  var $clone = $('.file-upload-input').val('').clone(true); // create empty clone
  $('.file-upload-input').replaceWith($clone);              // reset input: replaced by empty clone
  $('.file-upload-placeholder').show();                     // show placeholder
  $('.file-upload-preview').hide();                         // hide preview
}

// Style when drag-over
$('.file-upload-placeholder').bind('dragover', function () {
  $('.file-upload-placeholder').addClass('image-dropping');
});
$('.file-upload-placeholder').bind('dragleave', function () {
  $('.file-upload-placeholder').removeClass('image-dropping');
});

const input = document.getElementById('fileinput');


function generateAllServerHTML(data) {
    var list = $('.table .body-table');
    var theTemplateScript = $("#filelist-template").html();
    var theTemplate = Handlebars.compile(theTemplateScript);
    list.append(theTemplate(data));
}

// This will upload the file after having read it
const upload = (file) => {
    var formData = new FormData();
    formData.append("file", file);

    $.ajax({
        url: '/api/ApiUploadFile/UploadFile',
        type: 'POST',
        data: formData,
        cache: false,
        contentType: false,
        processData: false,
        success: function (response) {
            if (response.Status) {
                generateAllServerHTML(response.Object);
                alert("Upload file thành công");

            }
            else {
                alert(response.Object);
            }
        },
    });
};



// Event handler executed when a file is selected
const onSelectFile = () => upload(input.files[0]);

// Add a listener on your input
// It will be triggered when a file will be selected
input.addEventListener('change', onSelectFile, false);

var Uploadfile = {
    init: function () {
        this.getListFile();
    },
    registerEvent: function () {
        $(".btn_delete").off('click').on('click', async function (e) {
            e.preventDefault();
            if (confirm("You comfirm delete ?")) {
                var data = {
                    "FileAttachGUID": $(this).data('id')
                };
                const res = await fetch('/api/ApiUploadFile/Delete', {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json',
                    },
                    body: JSON.stringify(data)
                })
                const json = await res.json();
                if (json.Status == 0) {
                    $('#' + $(this).data('id')).remove();
                    alert("Xóa file thành công");
                    
                }
                else {
                    alert(json.Object);
                }

            }

        });
    },
    getListFile: async function () {
        if (true) {
            const res = await fetch('/api/ApiUploadFile/GetList', {
                method: 'GET',
                headers: {
                    'Content-Type': 'application/json',
                }
            })
            const json = await res.json();
            if (json.Status==0) {
                generateAllServerHTML(json.Object);
                this.registerEvent();

            }
            else {
                alert(json.Object);
            }
            
        }
        else {
            window.location.hash = "/UserRegister/Login";
        }
    }
}
Uploadfile.init();
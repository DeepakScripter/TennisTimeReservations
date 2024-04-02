TennisCourtBookingApp.Common = new function () {
    this.SignUpGet = function () {

        $(".preloader").show();
        $.ajax({
            type: "Get",
            url: UrlContent("Home/UserSignUp"),

            success: function (response) {

                $("#modalContent").html(response);
                $.validator.unobtrusive.parse($("#UserDetails"));
                $("#modalShow").modal('show');
                $(".preloader").hide();

            },
            error: function () {
                // Handle error if needed
                $(".preloader").hide(); // Ensure the preloader is hidden in case of an error
            }
        });
    }

    //this.SignUpPosttt=function() {
    //    debugger;
    //    if ($("#UserDetails").valid()) {
    //        $(".preloader").show();
    //        var fileUpload = $("#SaveImage").get(0);
    //        var files = fileUpload.files;
    //        var formdata = new FormData();
    //        for (var i = 0; i < files.length; i++) {
    //            formdata.append("Image", files[i]);
    //        }
    //        var formdata = $("#UserDetails").serialize();
    //        $.ajax({
    //            type: "Post",
    //            url: UrlContent("Home/UserSignUp"),
    //            data: formdata,
    //            contentType: false,
    //            processData: false,
    //            //dataType: "text",
    //            success: function (response) {
    //                debugger;
    //                Swal.fire({
    //                    title: "Registration  Successfully",
    //                    icon: "Success"
    //                }).then((result) => {
    //                    if (result.isConfirmed) {

    //                    }
    //                    //var url = 'https://localhost:7235/';
    //                    //window.location.href = url;
    //                    //LoginGet();
    //                    //location.replace(url);
    //                    //window.location.reload();
    //                });
    //                //$("#modalContent").html(response);
    //                //$("#modalShow").modal('show');

    //                $(".preloader").hide();

    //            },
    //            error: function () {
    //                // Handle error if needed
    //                $(".preloader").hide(); // Ensure the preloader is hidden in case of an error
    //            }
    //        });
    //    }
    //}




    //this.SignUpPosttttt=function () {

    //    if ($("#UserDetails").valid()) {
    //        $(".preloader").show();

    //        var formdata = new FormData();

    //        // Collect image files
    //        var fileUpload = $("#SaveImage").get(0);
    //        var files = fileUpload.files;
    //        for (var i = 0; i < files.length; i++) {
    //            formdata.append("Images", files[i]);
    //        }

    //        // Collect other form fields
    //        //formdata.append("Username", $("#Username").val());
    //        //formdata.append("Email", $("#Email").val());
    //        //formdata.append("Username", $("#UserAddress").val());
    //        //formdata.append("Username", $("#UserPhoneNo").val());
    //        //formdata.append("Username", $("#UserPassword").val());
    //        // Add more fields as needed

    //        $.ajax({
    //            type: "POST",
    //            url: UrlContent("Home/SaveProfileImage"),
    //            data: formdata,
    //            contentType: false,
    //            processData: false,
    //            //dataType: "json", // Adjust based on expected response type
    //            success: function (response) {

    //                Swal.fire({
    //                    title: "Registration Successfully",
    //                    icon: "success"
    //                }).then((result) => {
    //                    if (result.isConfirmed) {
    //                        // Handle successful registration, e.g., redirect to login page
    //                        window.location.href = 'https://localhost:7235/';
    //                    }
    //                });

    //                $(".preloader").hide();
    //            },
    //            error: function () {
    //                // Handle error if needed
    //                $(".preloader").hide();
    //            }
    //        });
    //    }
    //}



    this.LoginGet=function () {

        $(".preloader").show();
        $.ajax({
            type: "Get",
            url: UrlContent("Home/Login"),

            success: function (response) {

                $("#modalContent").html(response);
                //$.validator.unobtrusive.parse($("#UserDetails"));
                $("#modalShow").modal('show');
                $(".preloader").hide();

            },
            error: function () {
                // Handle error if needed
                $(".preloader").hide(); // Ensure the preloader is hidden in case of an error
            }
        });
    }

    this.LoginPost=function () {

        //if ($("#UserDetails").valid()) {
        $(".preloader").show();
        var formdata = $("#UserLogin").serialize();
        $.ajax({
            type: "Post",
            url: UrlContent("Home/Login"),
            data: formdata,
            dataType: "text",
            success: function (response) {

                //Swal.fire({
                //    title: "Registration  Successfully",
                //    icon: "Success"
                //}).then((result) => {
                //    if (result.isConfirmed) {

                //    }
                //    var url = 'https://localhost:7235/Home/Login';
                //    window.location.href = url;
                //    //location.replace(url);
                //    //window.location.reload();
                //});
                //$("#modalContent").html(response);
                //$("#modalShow").modal('show');
                if (response.userId == null) {
                    var url = 'https://localhost:7235/TennisCourt'
                    window.location.href = url;
                }
                else {


                    var url = 'https://localhost:7235/User/UserDetails?userId=response.userId'
                    window.location.href = url;
                }
                $(".preloader").hide();

            },
            error: function () {
                // Handle error if needed
                $(".preloader").hide(); // Ensure the preloader is hidden in case of an error
            }
        });
        //}
    }

    this.SignUpPost = function () {
        debugger;
        var fileUpload = $("#SaveImage").get(0);
        var files = fileUpload.files;
        var formdata = new FormData();
        for (var i = 0; i < files.length; i++) {
            formdata.append("Image", files[i]);
        }
        var form_data = $('#UserDetails').serializeArray();
        $.each(form_data, function (key, input) {
            formdata.append(input.name, input.value);
        });
        console.log(formdata)
        $.ajax({
            type: "Post",
            url: UrlContent("Home/SignUp/"),
            data: formdata,
            contentType: false,
            processData: false,
            dataType: 'json',
            success: function (response) {
                debugger;
                window.location.reload();
                $(".preloader").hide();

            },
            error: function () {
                // Handle error if needed
                $(".preloader").hide(); // Ensure the preloader is hidden in case of an error
            }
        });
    }

    this.previewImage=function (input) {
        var file = input.files[0];
        var reader = new FileReader();

        reader.onload = function (e) {
            $('#imagePreview').attr('src', e.target.result).show();
        };

        reader.readAsDataURL(file);
    }

    //this.triggerFileInput=function () {
    //    $('#SaveImage').click();
    //}


}



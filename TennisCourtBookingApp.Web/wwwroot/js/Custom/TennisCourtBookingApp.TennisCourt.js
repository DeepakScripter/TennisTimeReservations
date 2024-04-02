function ChangeStatus(selectStatus) {
    debugger;
    // Access the selected value using selectElement.value
    var selectedValue = selectStatus.value;
    TennisCourtBookingApp.TennisCourt.GetBookingListUser(selectedValue);
    //console.log("Selected value: " + selectedValue);
    // Perform any additional actions based on the selected value

}


TennisCourtBookingApp.TennisCourt = new function () {

   
    //this.ChangeStatus = function(selectStatus) {
    //    debugger;
    //    // Access the selected value using selectElement.value
    //    var selectedValue = selectStatus.value;
    //    //console.log("Selected value: " + selectedValue);

    //    // Perform any additional actions based on the selected value

    //    GetBookingListUser(selectedValue);
    //}
   
    this.GetBookingListUser = function (status) {
        debugger;
        var dataTable = $('#AllUserBookings').DataTable();

        // Destroy the DataTable
        dataTable.destroy();

        // Now, you can reinitialize the DataTable with your desired options
        $('#AllUserBookings').dataTable({


            searching: true,
            paging: true,
            serverSide: true,
            processing: true,
            bLengthChange: true,
            async: true,
            lengthMenu: [[5, 10, 25, 50, 100, -1], [5, 10, 25, 50, 100, "All"]],

            //dom: '<"top"flp>rt<"row btmpage"<"col-2 lgndrp"><"col-7"i><"col-3"p>>',
            pageLength: 5,
            ajax: {
               
                type: "Post",
                url: "/TennisCourt/UserBookingDetails",

                //dataType: 'Json'
                data: function (dtParms) {

                    debugger;
                    //dtParms.search.value = $("#" + Demo.Common.Option.SearchId).val();
                    dtParms.status=status;

                    return dtParms;
                },
            },

            columns: [
                {
                    "data": "userName", "name": "UserName", "autoWidth": true,
                    "render": function (data) {
                        // Apply inline CSS to the cell
                        return '<div style="color: Black; font-weight: bold;">' + data + '</div>';
                    }
                },

                {
                    "data": "tennisCourtName", "name": "TennisCourtName", "autoWidth": true,
                    "render": function (data) {
                        // Apply inline CSS to the cell
                        return '<div style="color: Black; font-weight: bold;">' + data + '</div>';
                    }

                },

                {
                    "data": "tennisCourtAddress", "name": "TennisCourtAddress", "autoWidth": true,
                    "render": function (data) {
                        // Apply inline CSS to the cell
                        return '<div style="color: Black; font-weight: bold;">' + data + '</div>';
                    }
                },

                {
                    "data": "bookingDate" ,
                    "name": "BookingDate",
                    "width": "200px",
                    "render": function (data) {
                        // Check if data is not null before formatting
                        if (data) {
                            var formattedDate = new Date(data).toLocaleDateString("en-US", { month: '2-digit', day: '2-digit', year: 'numeric' });

                            // Apply inline CSS to the cell
                            return '<div style="color: Black; font-weight: bold;">' + formattedDate + '</div>';
                        } else {
                            return '';  // Handle null or undefined data if needed
                        }
                    }
                },

                {
                    "data": "bookingTime", "name": "BookingTime", "autoWidth": true,
                    "render": function (data) {
                        var formattedTime = new Date('1970-01-01T' + data).toLocaleTimeString("en-US", { hour: '2-digit', minute: '2-digit', hour12: true });
                        // Apply inline CSS to the cell
                        return '<div style="color: Black; font-weight: bold;">' + formattedTime + '</div>';
                    }
                },
                {
                    "data": "status", "name": "Status", "autoWidth": true,
                    //"render": function (data) {
                    //    // Apply inline CSS to the cell
                    //    return '<div style="color: Black; font-weight: bold;">' + data + '</div>';
                    //}
                    "render": function (data) {
                        // Conditionally apply classes based on the value of isActive
                        if (data === 'Pending') {
                            return '<span class="badge badge-warning">Pending</span>';
                        } else if (data === 'Rejected') {
                            return '<span class="badge badge-danger">Reject</span>';
                        }
                        else if (data === 'Confirm') {
                            return '<span class="badge badge-success">Confirm</span>';
                        }
                    }
                },



                {
                    "data": 'bookingId', "className": "text-center", "width": "300px", orderable: false,
                    "render": function (data, type, row) {
                        debugger;
                        if (row.status === "Pending") {
                            //    //let btnConfirm = '<button title="Book Slot" class="btn btn-success btn-sm mt-1 mr-3" style=" margin-right: 2px;" onclick="TennisCourtBookingApp.Booking.EditBookingStatus(\'' + 'Confirm' + data + '\')">Confirm</button>';
                            let btnConfirm = '<button title="COnfirm" class="btn btn-success btn-sm mt-1 mr-3" style="margin-right: 2px;" onclick="TennisCourtBookingApp.Booking.EditBookingStatus(\'' + 'Confirm' + '\', \'' + data + '\')">Confirm</button>';
                            //    //let btnReject = '<button title="Book Slot" class="btn btn-success btn-sm mt-1 mr-3" style=" margin-right: 2px;" onclick="TennisCourtBookingApp.Booking.EditBookingStatus(\'' + 'Reject' + data + '\')">Reject</button>';
                            let btnReject = '<button title="Reject" class="btn btn-danger btn-sm mt-1 mr-3" style="margin-right: 2px;" onclick="TennisCourtBookingApp.Booking.EditBookingStatus(\'' + 'Rejected' + '\', \'' + data + '\')">Reject</button>';



                            //return btnConfirm + btnReject;
                            return btnConfirm + btnReject;
                        }
                        else {
                            
                            return null;
                        }
                    }
                }
            ],
            order: [0, "ASC"],
            "dom": '<"top"lf>rt<"bottom"ip><"clear">'
        });
        // Add custom dropdown next to the length dropdown
        var lengthDropdownContainer = $('#AllUserBookings_length');
        var customDropdownHtml = '<label style="margin-left: 20px;">Status</label>' +
            '<select id="customDropdown" onclick="ChangeStatus(this)">' +
            '<option value="0">Select Status</option>' +
            '<option value="1">All</option>' +
            '<option value="2">Pending</option>' +
            '<option value="3">Reject</option>' +
            '<option value="4">Confirm</option>' +
            '</select>';
        lengthDropdownContainer.append(customDropdownHtml);
        $('#customDropdown').on('change', function () {
            debugger;
            var selectedValue = $(this).val();
            TennisCourtBookingApp.User.GetUserBooking(selectedValue);
            // Handle the selected value as needed
            //console.log(selectedValue);
        });
    }

    this.TennisCourtOption = {
        Table: null,
        TableId: "",
        RoleId: 0
    }
    this.GetCourtList = function (options) {
        TennisCourtBookingApp.TennisCourt.TennisCourtOption = $.extend({}, TennisCourtBookingApp.TennisCourt.TennisCourtOption, options);
        TennisCourtBookingApp.TennisCourt.TennisCourtOption.Table = $('#CourtList').DataTable({
            searching: true,
            paging: true,
            serverSide: true,
            processing: true,
            bLengthChange: true,
            async: true,
            lengthMenu: [[5, 10, 25, 50, 100, -1], [5, 10, 25, 50, 100, "All"]],
            //dom: '<"top"flp>rt<"row btmpage"<"col-2 lgndrp"><"col-7"i><"col-3"p>>',
            pageLength: 5,
            ajax: {

                type: "Post",
                url: "/TennisCourt/GetList",

                //dataType: 'Json'
                data: function (dtParms) {
                    //dtParms.search.value = $("#" + Demo.Common.Option.SearchId).val();
                    return dtParms;
                },
            },

            columns: [

                {
                    "data": "tennisCourtName", "name": "TennisCourtName", "width": "300px",

                },

                {
                    "data": "tennisCourtAddress", "name": "TennisCourtAddress", "width": "200px",
                },

                {
                    "data": "tennisCourtCapacity", "name": "TennisCourtCapacity", "width": "200px",
                },
                {
                    "data": 'tennisCourtId', "className": "text-center", "width": "400px", orderable: false,
                    "render": function (data, type, row) {
                      
                        let btnDelete = '<button title="Delete" class="btn btn-danger btn-sm mt-1 mr-3" style=" margin-right: 2px;" onclick="TennisCourtBookingApp.TennisCourt.CourtDeleteGet(\'' + data + '\')">Delete</button>';

                        let btnEdit = '<button title="Edit" class="btn btn-primary btn-sm  mt-1 mr-3" style="  margin-right: 2px;" onclick="TennisCourtBookingApp.TennisCourt.CourtEditGet(\'' + data + '\')">Edit</button>';

                        //let btnShowCourse = '<button title="ShowCourse" class="btn btn-primary btn-sm  mt-1 mr-3" style="  margin-right: 2px;" onclick="ShowCourses(\'' + data + '\')">Show Course</button>';
                        return btnEdit + btnDelete;
                    }
                }
            ],
            order: [0, "ASC"]
        });
        $('#CourtList_filter ').css({
            'float': 'left',
            'margin-left': '10px',
            'padding-right': '20px'
        });

        $('#CourtList_length').css({
            'float': 'right'
        });
    }
    this.AddCourt = function () {

        $(".preloader").show();
        $.ajax({
            type: "Get",
            url: UrlContent("TennisCourt/AddCourt"),

            success: function (response) {

                $("#modalContent").html(response);

                $("#modalShow").modal('show');
                $.validator.unobtrusive.parse($("#TennisCourt"));
                $(".preloader").hide();

            },
            error: function () {
                // Handle error if needed
                $(".preloader").hide(); // Ensure the preloader is hidden in case of an error
            }
        });
    }



    this.AddCourtPost = function () {
        if ($("#TennisCourt").valid()) {
            $(".preloader").show();
            var formdata = $("#TennisCourt").serialize();
            $.ajax({
                type: "Post",
                url: UrlContent("TennisCourt/AddCourt"),
                data: formdata,
                dataType: "text",
                success: function (response) {


                    Swal.fire({
                        title: "Tennis Court Added Successfully",
                        icon: "Success"
                    }).then((result) => {
                        if (result.isConfirmed) {

                        }
                        var url = 'https://localhost:7235/TennisCourt';
                        window.location.href = url;
                    });

                    //$("#modalContent").html(response);
                    //$("#modalShow").modal('show');
                    //window.location.reload();
                    $(".preloader").hide();

                },
                error: function () {
                    // Handle error if needed
                    $(".preloader").hide(); // Ensure the preloader is hidden in case of an error
                }
            });
        }
    }




    this.CourtEditGet = function (courtId) {

        $(".preloader").show();
        $.ajax({
            type: "Get",
            url: UrlContent("TennisCourt/TennisCourtEdit"),
            data: {
                courtId: courtId
            },
            success: function (response) {

                $("#modalContent").html(response);
                $("#modalShow").modal('show');
                $.validator.unobtrusive.parse($("#TennisCourt"));
                $(".preloader").hide();

            },
            error: function () {
                // Handle error if needed
                $(".preloader").hide(); // Ensure the preloader is hidden in case of an error
            }
        });

    }



    this.CourtEditPost = function () {
        if ($("#TennisCourt").valid()) {
            $(".preloader").show();

            var formdata = $("#TennisCourt").serialize();
            $.ajax({
                type: "Post",
                url: UrlContent("TennisCourt/TennisCourtEdit"),
                data: formdata,
                dataType: "text",
                success: function (response) {


                    Swal.fire({
                        title: "Tennis Court Update Successfully",
                        icon: "Success"
                    }).then((result) => {
                        if (result.isConfirmed) {

                        }
                        var url = 'https://localhost:7235/TennisCourt';
                        window.location.href = url;
                    });

                    //$("#modalContent").html(response);
                    //$("#modalShow").modal('show');
                    //window.location.reload();
                    $(".preloader").hide();

                },
                error: function () {
                    // Handle error if needed
                    $(".preloader").hide(); // Ensure the preloader is hidden in case of an error
                }
            });
        }
    }




    this.CourtDeleteGet = function (courtId) {
        $(".preloader").show();
        $.ajax({
            type: "Get",
            url: UrlContent("TennisCourt/TennisCourtDelete"),
            data: {
                courtId: courtId
            },
            success: function (response) {

                $("#modalContent").html(response);
                $("#modalShow").modal('show');
                $(".preloader").hide();

            },
            error: function () {
                // Handle error if needed
                $(".preloader").hide(); // Ensure the preloader is hidden in case of an error
            }
        });

    }

    this.CourtDeletePost = function () {
        debugger;
        $(".preloader").show();
        var formdata = $("#TennisCourt").serialize();
        $.ajax({
            type: "Post",
            url: UrlContent("TennisCourt/TennisCourtDelete"),
            data: formdata,
            dataType: "text",
            success: function (response) {

                Swal.fire({
                    title: "Tennis Court Delete Successfully",
                    icon: "error"
                }).then((result) => {
                    if (result.isConfirmed) {

                    }
                    var url = 'https://localhost:7235/TennisCourt';
                    window.location.href = url;
                });

                //$("#modalContent").html(response);
                //$("#modalShow").modal('show');
                //window.location.reload();
                $(".preloader").hide();

            },
            error: function () {
                // Handle error if needed
                $(".preloader").hide(); // Ensure the preloader is hidden in case of an error
            }
        });
    }

}



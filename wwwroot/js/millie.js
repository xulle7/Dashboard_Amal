

var gid;


function AddNewitems()
{

    $('#newproducts').modal('show');
}

function AddNewDmg()
{
    $('#dmgproduct').modal('show');
}

function AddProDtil()
{
    $('#addproducts').modal('show');
}

function ShowDelMessage(id)
{
    gid = id;
    $('#confirm').modal('show');
}




function ConfirmDetails() {
    $.ajax
        ({
            url: '/Home/DeleteDetails',
            type: 'POST',
            data: { id: gid },

            //تنفذ بعد انتهاء العملية
            success: function (result) {
                alert('تم الحذف')

                window.location.href = '/Home/ProductDetails';
            }

        });
}


function updateDetail(id) {
    var gid = document.getElementById("Id");
    var ph = document.getElementById("photo");
    var price = document.getElementById("Price");
    var qty = document.getElementById("Qty");
    var color = document.getElementById("Color");
    var name = document.getElementById("ProductId");

    $.ajax({

        url: '/Home/GetDetails',
        type: 'POST',
        data: { ProductId: id },

        success: function (result)
        {
            gid.value = result.ProductId;
            ph.value = result.ph;
            price.value = result.price;
            qty.value = result.qty;
            color.value = result.color;
            name.value = result.name;

            console.table(result);
        }


    })


    $('#updateDetail').modal('show');
}




function Confirmdel1() {
    $.ajax
        ({
            url: '/Home/Delete',
            type: 'POST',
            data: { record: gid },

            //تنفذ بعد انتهاء العملية
            success: function (result) {
                alert('تم الحذف')

                window.location.href = '/Home/AddNewitems';
            }

        });
}


function update(id) {
    var gid = document.getElementById("Id");
    var name = document.getElementById("Name");
    var description = document.getElementById("Description");

    $.ajax({

        url: '/Home/GetData',
        type: 'POST',
        data: { id: id },

        success: function (result) {
            gid.value = result.id;
            name.value = result.name;
            description.value = result.description;

            console.table(result);
        }


    })


    $('#update').modal('show');
}



function ConfirmDmg() {
    $.ajax
        ({
            url: '/Home/DeleteDmg',
            type: 'POST',
            data: { record: gid },

            //تنفذ بعد انتهاء العملية
            success: function (result) {
                alert('تم الحذف')

                window.location.href = '/Home/DamageProducts';
            }

        });
}


function update(id) {
    var gid = document.getElementById("Id");
    var name = document.getElementById("Name");
    var description = document.getElementById("Description");

    $.ajax({

        url: '/Home/GetData',
        type: 'POST',
        data: { id: id },

        success: function (result) {
            gid.value = result.id;
            name.value = result.name;
            description.value = result.description;

            console.table(result);
        }
       
    })


    $('#update').modal('show');
}
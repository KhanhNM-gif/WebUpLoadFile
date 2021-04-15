var Register = {

    init: function () {
        this.registerEvent();

    },
    registerEvent: function () {
        $(".login100-form-btn").off('click').on('click', async function (e) {
            e.preventDefault();
            var data = {
                "Email": $("input[name='Email']").val(),
                "Username": $("input[name='UserName']").val(),
                "Password": $("input[name='Password']").val(),
                "ConfirmPassword": $("input[name='ConfirmPassword']").val(),
            };

            const res = await fetch('/api/ApiUserRegister/RegisterUser', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify(data),
            })
            const json = await res.json();

            if (json.Status == 0) {
                alert("Tạo Tài khoản thành công");
                window.location.replace("/UserRegister/Login");
            }
            else {
                alert(json.Object);
            }

        });
    }
}
Register.init();
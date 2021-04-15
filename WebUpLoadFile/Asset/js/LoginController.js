﻿var Login = {

    init: function () {
        this.registerEvent();

    },
    registerEvent: function () {
        $(".login100-form-btn").off('click').on('click', async function (e) {
            e.preventDefault();
            var data = {
                "UserName": $("input[name='email']").val(),
                "Password": $("input[name='pass']").val(),
                "IsRememberPassword": false
            };

            const res = await fetch('https://localhost:44387/api/UserRegister/Login', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify(data),
            })
            const json = await res.json();

            console.log(json);

            if (json.Status == 0) {
                localStorage.setItem("Authorization", json.Object.userToken.Token);
                alert("Đăng nhập thành công");
                window.location.replace("/Chating/Index");
            } 
            else {
                alert(json.Object);
            }

        });
    }
}
Login.init();
var ChatingIndex = {
    init: function () {
        this.registerEvent();
        this.getListFriends();
    },
    registerEvent: function () {
        $(".login100-form-btn").off('click').on('click', async function (e) {
            e.preventDefault();

        });
    },
    getListFriends: async function () {
        var Authorization = localStorage.getItem("Authorization");
        if (Authorization != null) {
            const res = await fetch('/api/ApiFriendship/GetListFriends', {
                method: 'GET',
                headers: {
                    'Content-Type': 'application/json',
                    'Authorization': Authorization,
                },
            })
            const json = await res.json();
            this.generateAllFriendsHTML(json.Object)
        }
        else {
            window.location.replace("/UserRegister/Login");
        }
    },
    generateAllFriendsHTML: function(data){
        var list = $('.listfen .list-friend');
        var theTemplateScript = $("#friend-template").html();
		var theTemplate = Handlebars.compile (theTemplateScript);
		list.append (theTemplate(data));
    },
    getListServer: async function () {
        var Authorization = localStorage.getItem("Authorization");
        if (Authorization != null) {
            const res = await fetch('/api/ApiChating/getListServer', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                    'Authorization': Authorization
                }
            })
            const json = await res.json();
            console.log(json.Object);
            this.generateAllServerHTML(json.Object)
        }
        else {
            window.location.hash = "/UserRegister/Login";
        }
    },
    generateAllServerHTML: function (data) {
        var list = $('.fisrtbar .list-server');
        var theTemplateScript = $("#server-template").html();
        var theTemplate = Handlebars.compile(theTemplateScript);
        list.append(theTemplate(data));
    }
}

ChatingIndex.init();
LunchApp.Profile = function () {
    this.init();
};

LunchApp.Profile.prototype = {
    init: function () {
        this.initFavoriteFoodItems();
    },

    initFavoriteFoodItems: function () {
        var url = '/umbraco/api/FoodRelation/GetFavoriteFoodItems';
        var menuId = $('#TodaysMenu').attr('data-menu');
        console.log(menuId);
        $.get(url, function (data) {
            $(".divided").html(data);
        });
    }
};

$(function () {
    new LunchApp.Profile();
});
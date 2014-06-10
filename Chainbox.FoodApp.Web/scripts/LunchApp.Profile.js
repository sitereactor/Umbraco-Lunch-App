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
        
        $.get(url, function (data) {
            $.each(data, function (index, value) {
                $(".divided").append('<li data-fooditem-id="' + value.Id + '"><strong>' + value.Name + '</strong></li>');
            });
        });
    }
};

$(function () {
    new LunchApp.Profile();
});
LunchApp.Profile = function () {
    this.init();
};

LunchApp.Profile.prototype = {
    init: function () {
        this.initShowFavoriteFoodItems();
        this.initShowCurrentFoodOrder();
        this.initResetOrder();
        this.initOrderFavorites();
    },

    initShowFavoriteFoodItems: function () {
        var url = '/umbraco/api/FoodRelation/GetFavoriteFoodItems';
        var menuId = $('#TodaysMenu').attr('data-menu');
        
        $.get(url, function (data) {
            $.each(data, function (index, value) {
                if (menuId == value.ParentId) {
                    $(".divided").append('<li data-fooditem-id="' + value.Id + '" data-supplier-id="' + value.ParentId + '"><strong>' + value.Name + '</strong></li>');
                } else {
                    $(".divided").append('<li data-fooditem-id="' + value.Id + '" data-supplier-id="' + value.ParentId + '">' + value.Name + '</li>');
                }
                
            });
        });
    },

    initShowCurrentFoodOrder: function() {
        var url = '/umbraco/api/FoodOrder/GetOrderFoodItems';
        $.get(url, function (data) {
            if (data.length === 0) {
                $('#ResetOrder').hide();
                $('#TodaysMenu').next().text('You have not ordered lunch yet!');
            }

            $.each(data, function(index, value) {
                $('#TodaysOrder').append('<li data-fooditem-id="' + value.FoodItemId + '"><strong>' + value.FoodItem + '</strong></li>');
            });
        });
    },

    initResetOrder: function () {
        $('#ResetOrder').on('click', function (e) {
            e.preventDefault();
            var url = '/umbraco/api/FoodOrder/ResetOrderFood';
            $.post(url).done(function(data) {
                $("ul#TodaysOrder").empty();
                $('#ResetOrder').hide();
                $('#TodaysMenu').next().text('You have not ordered lunch yet!');
            });
        });
    },

    initOrderFavorites: function() {
        $('#OrderFavorites').on('click', function (e) {
            e.preventDefault();
            var url = '/umbraco/api/FoodOrder/OrderFavorites';
            var menuId = $('#TodaysMenu').attr('data-menu');

            $.post(url, { CurrentFoodSupplier: menuId })
		        .done(function (data) {
		            $.each(data, function (index, value) {
		                $('#TodaysOrder').append('<li data-fooditem-id="' + value.FoodItemId + '"><strong>' + value.FoodItem + '</strong></li>');
		            });

		            $('#TodaysMenu').next().text('Below is your current order for today');
		            $('#ResetOrder').show();
            });
        });
    }
};

$(function () {
    new LunchApp.Profile();
});
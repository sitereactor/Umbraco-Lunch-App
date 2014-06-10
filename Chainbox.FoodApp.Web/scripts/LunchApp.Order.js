LunchApp.Order = function () {
	this.init();
};

LunchApp.Order.prototype = {
	init: function () {
	    this.initMenuOrder();
	    this.initFavoriteFoodInMenu();
	},

	initMenuOrder: function () {
		$('.box a.button').on('click', function (e) {
			e.preventDefault();
			var inner = $(this);
			inner.addClass('alt');

			//TOOD Call an endpoint instead outputting to the console
			console.log(inner.attr('data-food'));
		});
	},

    initFavoriteFoodInMenu: function() {
        $('.box a.button').on('click', 'favorite-pointer', function (e) {
            
        });
    }
};

$(function () {
	new LunchApp.Order();
});
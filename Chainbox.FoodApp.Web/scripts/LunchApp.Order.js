LunchApp.Order = function () {
	this.init();
};

LunchApp.Order.prototype = {
	init: function () {
		this.initMenuOrder();
	},

	initMenuOrder: function () {
		$('.box a.button').on('click', function (e) {
			e.preventDefault();
			var inner = $(this);
			inner.addClass('alt');

			//TOOD Call an endpoint instead outputting to the console
			console.log(inner.attr('data-food'));
		});
	}
};

$(function () {
	new LunchApp.Order();
});
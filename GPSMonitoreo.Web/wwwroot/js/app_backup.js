function Application()
{
	this.xx = 1;

}



Application.prototype = {

	_notification: null,
	_windowError: null,
	_windowInfo: null,
	init: function()
	{
		var $notif = $('#notification');
		var $contentContainer = $notif.find('> div');
		

		$notif.jqxNotification({
			//appendContainer: $notif.next(),
			appendContainer: $('#notifications_container'),
			position: 'bottom-right',
			width: 280, opacity: 1,
			autoOpen: false, animationOpenDelay: 800, autoClose: true, autoCloseDelay: 3000, template: "info"
		});

		this._notification = $notif.data('jqxWidget');
		this._notification.$contentContainer = $contentContainer;


		this._windowError = $('#windowError').jqxWindow({
			height:200, width: 400, isModal: true, autoOpen: false, draggable: false, resizable: false
		}).data('jqxWidget');



		//this.$notif = $notif;
		this.extended_init();
	},

	extended_init: function(){},

	showError: function(msg)
	{
		this._windowError.setContent(msg);
		this._windowError.open();
	},

	showNotification: function(msg)
	{
		this._notification.$contentContainer.html(msg);
		this._notification.open();
	},

	showInfo: function(title, content, size)
	{
		if(!size)
		{
			size = {width: 300, height: 300};
		};

		if(!this._windowInfo)
		{
			this._windowInfo = $('#windowInfo').jqxWindow({
				width: size.width, height: size.height,
				isModal: true, autoOpen: false, draggable: false, resizable: false
			}).data('jqxWidget');
		}
		else
		{
			this._windowInfo.width = size.width;
			this._windowInfo.height = size.height;
		}
		this._windowInfo.setTitle(title);
		this._windowInfo.setContent(content);
		this._windowInfo.open();
	},

	postJson: function(url, data, okCallback, options, handleOptions)
	{
		if(!options)
			options = {};

		if(!handleOptions)
			handleOptions = {};

		if(okCallback)
			handleOptions.ok = okCallback;

		
		Ajax.postJson(url, data, function(response){
			Response.handle(response, handleOptions);
		}, options);
	},

	postFormJsonFromButton: function(button, okCallback, options, handleOptions)
	{
		if(!options)
			options = {};

		options.loadingElement = $(button);
		
		this.postFormJson(button.$form ? button.$form : button.form.$form , okCallback, options, handleOptions);
	},

	postFormJson: function(form, okCallback, options, handleOptions)
	{
		
		if(!options)
			options = {};

		if(!handleOptions)
			handleOptions = {};


		//options.async = false;
		/*finalOptions.loadingElement = options.loadingElement ? options.loadingElement : null;*/

		handleOptions.form = form;


		if(handleOptions.responseToContainer)
			handleOptions.response_container = form.find('.response_container');

		var data = {};
		if(form._widgets)
			data = form.toJson();
		else
			data = form.serializeObject();


		this.postJson(form[0].action, data, okCallback, options, handleOptions);
	},

	redirect: function(url)
	{
		if(url == 'self')
			document.location.assign(document.location.href);
		else
			document.location.assign(url);

		//document.location = url;
		/*
		if(url == 'self')
			if(this.statePage)
				document.location.assign(document.location.href);
			else
				document.location.replace(document.location.href);
		else
			if(this.statePage)
				document.location.assign(url);
			else
				document.location.replace(url);
		*/
	}
};


Application.extend = function(options)
{
	for(var key in options)
	{
		if(key == 'init')
			this.prototype.extended_init = options[key];
		else
			this.prototype[key] = options[key];
	}
}



//var App2 = new Application();



//var App =
//{
//	notification: null,
//	$notification_content: null,
//	init: function()
//	{
//		var $notif = $('#notification');
//		this.$notification_content = $notif.find('> div');

//		$notif.jqxNotification({
//			//appendContainer: $notif.next(),
//			appendContainer: $('#notifications_container'),
//			position: 'bottom-right',
//			width: 280, opacity: 1,
//			autoOpen: false, animationOpenDelay: 800, autoClose: true, autoCloseDelay: 3000, template: "info"
//		});

//		this.notification = $notif.data('jqxWidget');
//		this.$notif = $notif;
//	},

//	extend: function(options)
//	{
//		for(var key in options)
//		{
//			console.log(key);
//		}
//	},

//	showError: function(msg)
//	{
//		var modal = $('#modal_error');
//		modal.find('.modal-body').html(msg);
//		modal.modal('show');	
//	},

//	showNotification: function(msg)
//	{
//		this.$notification_content.html(msg);
//		this.notification.open();
//	},

//	postJson: function(url, data, okCallback, options, handleOptions)
//	{
//		if(!options)
//			options = {};

//		if(!handleOptions)
//			handleOptions = {};

//		if(okCallback)
//			handleOptions.ok = okCallback;

		
//		Ajax.postJson(url, data, function(response){
//			Response.handle(response, handleOptions);
//		}, options);
//	},

//	postFormJson: function(form, okCallback, options, handleOptions)
//	{
		
//		if(!options)
//			options = {};

//		if(!handleOptions)
//			handleOptions = {};


//		//options.async = false;
//		/*finalOptions.loadingElement = options.loadingElement ? options.loadingElement : null;*/

//		handleOptions.form = form;


//		if(handleOptions.responseToContainer)
//			handleOptions.response_container = form.find('.response_container');

//		var data = {};
//		if(form._widgets)
//			data = form.toJson();
//		else
//			data = form.serializeObject();


//		this.postJson(form[0].action, data, okCallback, options, handleOptions);
//	},

//	redirect: function(url)
//	{
//		if(url == 'self')
//			document.location.assign(document.location.href);
//		else
//			document.location.assign(url);

//		//document.location = url;
//		/*
//		if(url == 'self')
//			if(this.statePage)
//				document.location.assign(document.location.href);
//			else
//				document.location.replace(document.location.href);
//		else
//			if(this.statePage)
//				document.location.assign(url);
//			else
//				document.location.replace(url);
//		*/
//	}
//}

var App = new Application();
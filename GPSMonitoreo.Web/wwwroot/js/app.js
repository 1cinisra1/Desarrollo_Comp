//prueba original

function Application()
{

	



	///aahh
}
Application.prototype = {

	_notification: null,
	_windowError: null,
	_windowInfo: null,
	_windowInput: null,
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

		this._windowWarning = $('#windowWarning').jqxWindow({
			height:200, width: 400, isModal: true, autoOpen: false, draggable: false, resizable: false
		}).data('jqxWidget');

		this._windowInfo = $('#windowInfo').jqxWindow({
			width: 300, height: 300,
			isModal: true, autoOpen: false, draggable: true, resizable: false
		}).data('jqxWidget');



		//this.$notif = $notif;
		this.extended_init();
	},

	extended_init: function(){},

	showInfo: function(title, content, size)
	{
		if(!size)
		{
			size = {width: 300, height: 300};
		}

		this._windowInfo.width = size.width;
		this._windowInfo.height = size.height;

		this._windowInfo.setTitle(title);
		this._windowInfo.setContent(content);
		this._windowInfo.open();
	},

	showError: function(msg)
	{
		this._windowError.setContent(msg);
		this._windowError.open();
	},

	showWarning: function(msg)
	{
		this._windowWarning.setContent(msg);
		this._windowWarning.open();
	},

	showNotification: function(msg)
	{
		this._notification.$contentContainer.html(msg);
		this._notification.open();
	},

	showInput: function(title, content, onContinue, size)
	{
		var win = this._windowInit('windowInput', title, content, size);
		win.onContinue = onContinue;

		if(!win.$input)
		{
			$input = win.host.find('input');
			$input.jqxInput();
			win.$input = $input;
		}

		if(!win.$continue)
		{
			$continue = win.host.find('.footer > button');
			$continue.jqxButton({template: 'info'});
			win.$continue = $continue;
			
			$continue.click(function(){
				win.onContinue(win.$input.val());
				win.close();
			});
		}

		win.$content.html(content);
		win.$input.val('');
		//this._windowInfo.setContent(content);
		//win.setContent(content);
		win.open();
	},

	handleResponseActions : function(response) {
		
		var actions = response.responseActions;
		var action;

		for(var x = 0; x < actions.length; x++)
		{
			action = actions[x];

			switch(action.action)
			{
				case 'notification':
					this.showNotification.apply(this, action.parameters);
					//console.log(2);
					break;

				case 'warning':
					this.showWarning.apply(this, action.parameters);
					//console.log(2);
					break;
			}

			//console.log(action.parameters);
		}



	
	
	},

	_windowInit: function(id, title, content, size)
	{
		if(!size)
		{
			size = {width: 300, height: 300};
		}

		var win = this['_' + id];

		if(!win)
		{
			var $div = $('#' + id); 
			win = $div.jqxWindow({
				width: size.width, height: size.height,
				isModal: true, autoOpen: false, draggable: false, resizable: false
			}).data('jqxWidget');

			win.$content = $div.find('.win_content');

			this['_' + id] = win;
		}
		else
		{
			win.width = size.width;
			win.height = size.height;
		}

		win.setTitle(title);

		return win;
	},





	get: function(url, okCallback, options, handleOptions)
	{
		options = options || {};
		handleOptions = handleOptions || {};

		if(okCallback) handleOptions.ok = okCallback;

		Ajax.get(url, function(response){
			Response.handle(response, handleOptions);
		}, options);
	},

	getHtml: function(url, okCallback, options)
	{
		options = options || {};
		options.dataType = 'html';
		Ajax.get(url, function(response){
			okCallback(response);
		}, options);
	},


	postJson: function(url, data, okCallback, options, handleOptions)
	{
		options = options || {};
		handleOptions = handleOptions || {};

		if(okCallback) handleOptions.ok = okCallback;

		
		Ajax.postJson(url, data, function(response){
			Response.handle(response, handleOptions);
		}, options);
	},

	postFormJsonFromButton: function($button, okCallback, options, handleOptions)
	{
		options = options || {};
		handleOptions = handleOptions || {};

		options.loadingElement = $button;

		//if(!handleOptions.errorMessageContainer)
		//{
		//	var $errorMessageContainer = $button.parent().find('.input_error');
		//	if($errorMessageContainer.length)
		//		handleOptions.errorMessageContainer = $errorMessageContainer;
		//}
		
		this.postFormJson($button.form ? $button.form : $button[0].form.$form , okCallback, options, handleOptions);
	},

	postFormJson: function(form, okCallback, options, handleOptions)
	{
		
		options = options || {};
		handleOptions = handleOptions || {};


		//options.async = false;
		/*finalOptions.loadingElement = options.loadingElement ? options.loadingElement : null;*/

		handleOptions.form = form;


		if(handleOptions.responseToContainer)
			handleOptions.response_container = form.find('.response_container');

		if(handleOptions.response_container)
			handleOptions.response_container.html('');

		var data = {};
		var action = '';
		if(form.widgets)
		{
			data = form.toJson();
			if(form.$form)
				action = form.$form[0].action;
			else
				action = form[0].action;
		}
		else
		{
			data = form.serializeObject();
			action = form[0].action;
		}


		this.postJson(action, data, okCallback, options, handleOptions);
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
};



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
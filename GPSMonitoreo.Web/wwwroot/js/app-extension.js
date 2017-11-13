Application.extend({
	$currentBody: null,
	
	$body: null,
	$splittedBody: null,

	windows	: {search: {}, quickview: {}, popupedit: {}},
	tabs	: null,

	_layouts : {},

	init: function(){
		//$('#app_layout').jqxSplitter({width: '100%', height: '100%', resizable: false, orientation: 'horizontal', panels: [{ size: '100px' }, { size: '100%' }] });
		//$('#app_layout_body').jqxSplitter({ height: '100%', width: '100%',  panels: [{ size: '30%' }, { size: '60%'}] });

		$('#app_menu_top').jqxMenu({ width: 670, height: 30});
		$('#app_menu_top').css('display', '');

		this.$body = $('#app_body');
		this.$splittedBody = $('#app_splittedBody');
		this.$splittedBody.$left = this.$splittedBody.find('#app_splittedBody_left');
		this.$splittedBody.$right = this.$splittedBody.find('#app_splittedBody_right');
		
		
		
		this.attachBody(this.$splittedBody);
		this.initSplittedBody();

		RemoteContent.load(this.$splittedBody.$left, '/home/menu');

		this.initTabs();

	},
	

	initTabs: function(){
		var $tabs = $('#app_tabs').jqxTabs({ width: '100%', height: '100%', showCloseButtons: true});
		var tabs = $tabs.data('jqxWidget');
		this.tabs = tabs;
		//tabs.removeLast();

		tabs._titles = tabs.host.find('> div > ul')[0].childNodes;
		tabs._loaded = {};

		//tabs.addLast('adsfs', 'asdfasdfasdf');

		//console.log(tabs._titles);


		//console.log(tabs._titles);

		//console.log(this.tabs);
		//console.log(this.tabs._titleList);

		var removeAt = tabs.removeAt;

		var self = this;

		tabs.removeAt = function(index){
			var li = this._titles[index];
			if(li._layout)
			{
				delete self._layouts[li._layout.url];
				delete li._layout;
			}
			else
			{
				delete this._loaded[li._loadedUrl];
			}

			//console.log(this._loaded);

			removeAt.call(this, index);
		}
	},

	//loadLayoutEdit: function(obj, id, title_suffix, options)
	//{
	//	options = options || {};

	//	var url = obj.basePath + '/editform' + (options.extendUrl ? options.extendUrl : '');

	//	var layout = this._layouts[url];

		

	//	if(layout)
	//	{
	//		if(!id)
	//			layout.editNew();
	//		else
	//			layout.edit(id);
	//	}
	//	else
	//	{
	//		var onLoaded = null;

	//		var self = this;
	//		if(id)
	//		{
	//			options.onLoaded = function(){
	//				this.edit(id);
	//			};
	//		}

	//		this.loadLayout(url, obj.tab_title + '/' + title_suffix, 'TabbedForm', options);
	//	}
	//},


	loadLayoutEdit: function(url, id, options, editOptions)
	{
		options = options || {};
		editOptions = editOptions || {};

		var layout = this._layouts[url];

		if(layout)
		{
			if(!id)
				layout.editNew(editOptions);
			else
				layout.edit(id, editOptions);
		}
		else
		{
			editOptions.skipReset = true;

			if(!id)
			{
				options.onLoaded = function() {
					this.editNew(editOptions);
				};
			}
			else
			{
				options.onLoaded = function() {
					this.edit(id, editOptions);
				};
			}

			this.loadLayout(url, options.tab_title, 'TabbedForm', options);
		}
	},

	loadLayoutSearchGrid: function(url, options) {
		var layout = this._layouts[url];

		if(layout)
		{
			layout.activate();
		}
		else
		{
			this.loadLayout(url, options.tab_title, 'SearchGrid', options);
		}
	},

	loadLayoutByType: function(url, type, options) {
		var layout = this._layouts[url];

		if(layout)
		{
			layout.activate();
		}
		else
		{
			this.loadLayout(url, options.tab_title, type, options);
		}
	},

	loadLayout: function(url, title, type, options) {
		var layout;
		options = options || {};

		switch(type)
		{
			case 'TabbedForm':
				layout = new AppLayoutTabbedForm();
				break;

			case 'Tabbed':
				layout = new AppLayoutTabbed();
				break;

			case 'SearchGrid':
				layout = new AppLayoutSearchGrid();
				break;

			default:
				layout = new AppLayout();
				break;
		}

		this._layouts[url] = layout;
		layout.url = url;
		layout.onLoaded = options.onLoaded;

		var tabs = this.tabs;

		tabs.addLast(title, '');

		//console.log(tabs._titles);

		var lastIndex = tabs._titles.length - 1;
			

		var li = tabs._titles[lastIndex];
		li._layout = layout;

		var $container = $(tabs.host[0].lastChild.lastChild);
		$container.css('position', 'relative');


		layout.ownerTabs = tabs;
		layout.ownerTabLI = li;
		layout.container = $container;

		layout.load(url);
	},

	hookLayout : function(url, hookInit) {
		var layout = this._layouts[url];
		hookInit(layout);
		layout.init();
	},

	loadTab: function(url, title, onActivate, onActivateIfLoaded)
	{
		var tabs = this.tabs;
		var loaded = tabs._loaded[url];

		

		if(loaded)
		{
			var index = Array.prototype.slice.call(tabs._titles).indexOf(loaded.li);
			tabs.select(index);
			

			if(onActivateIfLoaded)
				onActivateIfLoaded(loaded.$container);

			if(onActivate)
				onActivate(loaded.$container);
		}
		else
		{
			tabs.addLast(title, '');

			//console.log(tabs._titles);

			var lastIndex = tabs._titles.length - 1;
			

			var li = tabs._titles[lastIndex];

			var $container = $(tabs.host[0].lastChild.lastChild);
			$container.css('position', 'relative');


			
			li._loadedUrl = url;

			loaded = {url: url, li: li, $container: $container};

			tabs._loaded[url] = loaded;

			//console.log(tabs._loaded[url]);

			App.getHtml(url, function(response){
				tabs.setContentAt(lastIndex, response);
				if(onActivate)
					onActivate(loaded.$container);
			});
		}
	},



	loadTab2: function (options) {

		var tabs = this.tabs;

		var key = options.url;

		

		//if (options.forId)
		//{
		//	key += '/' + options.forId;
		//}



		var loaded = tabs._loaded[key];

		var li;

		if (loaded) {
			li = loaded.li;

			var index = Array.prototype.slice.call(tabs._titles).indexOf(li);
			tabs.select(index);


			if (options.onActivateIfLoaded)
				options.onActivateIfLoaded(loaded.$container);

			if (options.onActivate)
				options.onActivate(loaded.$container);

			tabs.setTitleAt(index, options.title);

		} else {
			tabs.addLast(options.title, '');

			//console.log(tabs._titles);

			var lastIndex = tabs._titles.length - 1;


			li = tabs._titles[lastIndex];

			var $container = $(tabs.host[0].lastChild.lastChild);
			$container.css('position', 'relative');



			li._loadedUrl = options.url;

			loaded = { url: options.url, li: li, $container: $container };

			tabs._loaded[key] = loaded;

			//console.log(tabs._loaded[url]);

			App.getHtml(options.url, function (response) {
				tabs.setContentAt(lastIndex, response);
				if (options.onActivate)
					options.onActivate(loaded.$container);
			});
		}

		if (options.hoverTitle)
			li.title = options.hoverTitle;
	},

	hookTab: function(url, onLoad){
		var loaded = this.tabs._loaded[url];
		onLoad(loaded.$container, this.tabs._titles.length - 1, this.tabs);
	},

	attachBody: function($body)
	{
		if(this.$currentBody == $body)
		{
			return;
		}

		if(this.$currentBody)
		{
			this.$currentBody.detach();
		}

		this.$body.append($body);
		this.$currentBody = $body;
	},

	//loadSplittedContentSide: function($side, url)
	//{
	//	console.log('loadSplittedContentSide');

	//	if($side.$current)
	//	{
	//		if($side.$current.url == url)
	//		{
	//			console.log('skipping side');
	//			return;
	//		}
	//		else
	//		{
	//			console.log('detaching side');
	//			$side.$current.detach();
	//		}
	//	}
		

	//	if($side.loaded[url])
	//	{
	//		$side.append($side.loaded[url]);
	//		$side.$current = $side.loaded[url];
	//		$side.$current.url = url;
	//		console.log('retaching side');
	//	}
	//	else
	//	{
	//		$side.load(url, function(){
	//			console.log('side loading');
	//			$side.find('> script').remove();
	//			$side.$current = $side.find('> div');
	//			$side.loaded[url] = $side.$current;
	//			$side.$current.url = url;
	//			console.log('side loaded');
	//		});
	//	}
	//},

	
	initSplittedBody: function(){
		if(!this.$splittedBody.initialized)
		{
			//this.$splittedBody.jqxSplitter({ height: '100%', width: '100%',  panels: [{ size: '20%' }, { size: '80%'}] });
			this.$splittedBody.jqxSplitter({ height: '100%', width: '100%',  panels: [{ size: '250px' }] });
			this.$splittedBody.css('display', '');
			this.$splittedBody.initialized = true;
		}
	},


	loadSplittedContent: function(leftUrl, rightUrl, onActivate)
	{
		//console.log('loadSplittedContent');
		this.attachBody(this.$splittedBody);
		this.initSplittedBody();


		RemoteContent.load(this.$splittedBody.$left, leftUrl);
		RemoteContent.load(this.$splittedBody.$right, rightUrl, null, onActivate);

		//this.loadSplittedContentSide(this.$splittedBody.$left, leftUrl);

	},

	loadSplittedContentCommon: function(obj, method, onActivate)
	{
		this.loadSplittedContent(obj.basePath + '/menu', obj.basePath + (method ? '/' + method : ''), onActivate);
	},

	loadSplittedCommonEdit: function(obj, id)
	{
		//var method = '/edit';
		var onActivate = null;
		var self = this;
		if(id !== undefined)
		{
			//method += '/'  + id;
			onActivate = function(remoteInstance){
				remoteInstance.onactivate = null;
				self.get(obj.basePath + '/edit/' + id, function(response){
					//console.log(response);
					remoteInstance.$container.form.fromJson(response.record);
				});
			};
		}
		this.loadSplittedContent(obj.basePath + '/menu', obj.basePath + '/edit', onActivate);
	},

	loadTabbedCommonEdit: function(obj, id, title_suffix, onLoaded)
	{
		//var method = '/edit';
		var onActivate = null;
		var onActivateIfLoaded = function($container){
			if($container.layout)
			{
				$container.layout.reset();
			
			}
			else
			{
				$container.form.reset();
			}
		};

		var self = this;
		if(id !== undefined)
		{
			onActivate = function($container){
				self.get(obj.basePath + '/edit/' + id, function(response){
					if($container.layout)
						$container.layout.loadData(response);
					else
						$container.form.fromJson(response.record);

					if (onLoaded)
						onLoaded($container, response);
				});
			};
		}

		//this.loadSplittedContent(obj.basePath + '/menu', obj.basePath + '/edit', onActivate);
		this.loadTab(obj.basePath + '/edit', obj.tab_title + '/' + title_suffix, onActivate, onActivateIfLoaded);
	},

	loadTabbedCommon: function(obj, title_suffix)
	{
		this.loadTab(obj.basePath, obj.tab_title + '/' + title_suffix);
	},

	popupEdit : function(url, id, options)
	{

		var $element = $('<div><div></div><div class="withfooter"></div></div>');
		$(document.body).append($element);

		var width;
		var height;

		if(options)
		{
			if(options.width !== undefined)
				width = options.width;

			if(options.height !== undefined)
				height = options.height;
		
		}

		var $win = $element.jqxWindow({width: width || 550, height: height || 550, isModal: true, draggable: true, resizable: false, autoOpen: false});
		

		var win = $win.data('jqxWidget');
		
		this.windows.popupedit[url] = win;
		win.$container = $element;

		if(options)
		{
			if(options.onFormLoaded)
				win.onFormLoaded = options.onFormLoaded;

			if(options.onSaved)
				win.onSaved = options.onSaved;
		}

		//$.param({id: 150}); //convert object to query string

		var self = this;

		$win.on('close', function(){
			delete self.windows.popupedit[url];
			win.destroy();
		});


		win.setTitle('&nbsp;');
		win.setContent('');

		win.open();

		var requestUrl = url;

		if(options && options.extendUrl !== undefined)
			requestUrl += '/' + options.extendUrl;

		App.getHtml(requestUrl, function(response){
			var $response = $(response);
			var $elements = $response.filter('div');
			win.setTitle($elements[0].innerHTML);
			win.setContent($elements[1].innerHTML);
			$element.append($response.filter('script'));

			if(id && id !== undefined)
			{

				var loadUrl;

				if(options && options.loadUrl)
					loadUrl = options.loadUrl + '/' + id;
				else
					loadUrl = url + '/' + id;


				App.get(loadUrl, function(response){
					win.$container.form.fromJson(response.record);
				});
			}
		});
	},


	popupEdit2 : function(url, options)
	{

		var $element = $('<div><div></div><div class="withfooter"></div></div>');
		$(document.body).append($element);

		var width;
		var height;

		if(options)
		{
			if(options.width !== undefined)
				width = options.width;

			if(options.height !== undefined)
				height = options.height;
		
		}

		var $win = $element.jqxWindow({width: width || 550, height: height || 550, isModal: true, draggable: true, resizable: false, autoOpen: false});
		

		var win = $win.data('jqxWidget');
		
		this.windows.popupedit[url] = win;
		win.$container = $element;

		if(options)
		{
			if(options.onFormLoaded)
				win.onFormLoaded = options.onFormLoaded;

			if(options.onSaved)
				win.onSaved = options.onSaved;
		}

		var self = this;

		$win.on('close', function(){
			delete self.windows.popupedit[url];
			win.destroy();
		});


		win.setTitle('&nbsp;');
		win.setContent('');

		win.open();

		App.getHtml(url, function(response){
			var $response = $(response);
			var $elements = $response.filter('div');
			win.setTitle($elements[0].innerHTML);
			win.setContent($elements[1].innerHTML);
			$element.append($response.filter('script'));
		});
	},

	hookPopupEdit: function(url, hookInit){

		var win = this.windows.popupedit[url];

		hookInit(win.$container, win);

		if(win.onFormLoaded)
			win.onFormLoaded(win.$container);
	
	},

	quickView	: function(url)
	{
		var win = this.windows.quickview[url];

		if(!win)
		{
			var self = this;

			win = this.initQuickViewWindow(url);

			this.windows.quickview[url] = win;
		}

		win.setTitle('&nbsp;');
		win.setContent('');

		win.open();

		App.getHtml(url, function (response) {
			var $response = $(response);
			var $elements = $response.filter('div');
			win.setTitle($elements[0].innerHTML);
			win.setContent($elements[1].innerHTML);
			win.$container.append($response.filter('script'));

		});

	},

	initQuickViewWindow: function(url)
	{

		var $element = $('<div><div></div><div></div></div>');
		$(document.body).append($element);

		var $win = $element.jqxWindow({width: 800, height: 600, isModal: true, draggable: true, resizable: false, autoOpen: false});
		

		var win = $win.data('jqxWidget');
		win.$container = $element;

		var self = this;

		$win.on('close', function(){
			delete self.windows.quickview[url];
			win.destroy();
		});
		return win;
	},

	hookQuickView: function (url, hookInit) {
		var win = this.windows.quickview[url];
		hookInit(win.$container, win);
	},

	//openWindowSearch	: function(id, onSelect, options)
	//{
	//	var win = this.windows.search[id];
	//	console.log(id);

	//	if(!win)
	//	{
	//		var self = this;
	//		this.windows.search[id] = {};
	//		App.getHtml(id + '/windowsearch', function(response){
	//			self.initWindowSearch(id, response, onSelect, options);
	//		});
	//	}
	//	else
	//	{
	//		win.onSelect = onSelect;
	//		win.win.open();
	//	}
	//},

	openWindowSearch	: function(url, onSelect, options, winOptions)
	{
		var win = this.windows.search[url];

		if(!win)
		{
			var self = this;
			this.windows.search[url] = {};
			App.getHtml(url, function(response){
				self.initWindowSearch(url, response, onSelect, options, winOptions);
			});
		}
		else
		{
			win.onSelect = onSelect;
			win.win.open();
		}
	},

	initWindowSearch: function(id, content, onSelect, options, winOptions)
	{
		var win = this.windows.search[id];
		$elements = $(content);
		$(document.body).append($elements);

		$win = $elements.filter('div');

		winOptions = winOptions || {};

		var width = winOptions.width || 800;
		var height = winOptions.height || 700;

		$win.jqxWindow({width: width, height, maxWidth: 1200, isModal: true, draggable: true, resizable: false, autoOpen: false});
		win.win = $win.data('jqxWidget');
		
		win.init($win, win);
		win.onSelect = onSelect;
		win.options = options || {};
		win.win.open();




			//win.$window.jqxWindow({width: 300, height: 400, isModal: true, draggable: false, resizable: false, autoOpen: false});
			//win.window = win.$window.data('jqxWidget');


	
	
	},

	initWindowSearchLoaded: function(options)
	{
	
	
	}
});


//var xxxx = {

//	attachBody2: function($body)
//	{
//		if(this.$currentBody == $body)
//		{
//			return;
//		}

//		if(this.$currentBody)
//		{
//			this.$currentBody.detach();
//		}

//		this.$body.append($body);
//		this.$currentBody = $body;
//	}
//};

//for(var key in xxxx)
//{
//	MyAPP.prototype[key] = xxxx[key];
//}








/*
$.extend(App, {

	attachBody: function($body)
	{
		if(this.$currentBody == $body)
		{
			return;
		}

		if(this.$currentBody)
		{
			this.$currentBody.detach();
		}

		this.$body.append($body);
		this.$currentBody = $body;
	}

});
*/

/*
$.extend(MyAPP.prototype, {
	testMethod : function(){


	}
});
*/

//MyAPP.prototype.testMethod = function(){


//};
//App.extend({

//	init: function(){
//		console.log('init from extend');
	
	
//	},

//	attachBody: function($body)
//	{
//		if(this.$currentBody == $body)
//		{
//			return;
//		}

//		if(this.$currentBody)
//		{
//			this.$currentBody.detach();
//		}

//		this.$body.append($body);
//		this.$currentBody = $body;
//	}




//});


//var Application = {

//	$currentBody: null,
	
//	$body: null,
//	$splittedBody: null,

//	init: function(){
//		//$('#app_layout').jqxSplitter({width: '100%', height: '100%', resizable: false, orientation: 'horizontal', panels: [{ size: '100px' }, { size: '100%' }] });
//		//$('#app_layout_body').jqxSplitter({ height: '100%', width: '100%',  panels: [{ size: '30%' }, { size: '60%'}] });

//		$("#app_menu").jqxMenu({ width: 670, height: 30});
//		$("#app_menu").css('display', '');

//		this.$body = $('#app_body');
//		this.$splittedBody = $('#app_splittedBody');
//		this.$splittedBody.$left = this.$splittedBody.find('#app_splittedBody_left');
//		this.$splittedBody.$right = this.$splittedBody.find('#app_splittedBody_right');
//	},

//	attachBody: function($body)
//	{
//		if(this.$currentBody == $body)
//		{
//			return;
//		}

//		if(this.$currentBody)
//		{
//			this.$currentBody.detach();
//		}

//		this.$body.append($body);
//		this.$currentBody = $body;
//	},

//	//loadSplittedContentSide: function($side, url)
//	//{
//	//	console.log('loadSplittedContentSide');

//	//	if($side.$current)
//	//	{
//	//		if($side.$current.url == url)
//	//		{
//	//			console.log('skipping side');
//	//			return;
//	//		}
//	//		else
//	//		{
//	//			console.log('detaching side');
//	//			$side.$current.detach();
//	//		}
//	//	}
		

//	//	if($side.loaded[url])
//	//	{
//	//		$side.append($side.loaded[url]);
//	//		$side.$current = $side.loaded[url];
//	//		$side.$current.url = url;
//	//		console.log('retaching side');
//	//	}
//	//	else
//	//	{
//	//		$side.load(url, function(){
//	//			console.log('side loading');
//	//			$side.find('> script').remove();
//	//			$side.$current = $side.find('> div');
//	//			$side.loaded[url] = $side.$current;
//	//			$side.$current.url = url;
//	//			console.log('side loaded');
//	//		});
//	//	}
//	//},

//	loadSplittedContent: function(leftUrl, rightUrl)
//	{
//		console.log('loadSplittedContent');
//		this.attachBody(this.$splittedBody);

//		if(!this.$splittedBody.initialized)
//		{
			
//			this.$splittedBody.jqxSplitter({ height: '100%', width: '100%',  panels: [{ size: '20%' }, { size: '80%'}] });
//			this.$splittedBody.css('display', '');
//			this.$splittedBody.initialized = true;
//		}

//		RemoteContent.load(this.$splittedBody.$left, leftUrl);
//		RemoteContent.load(this.$splittedBody.$right, rightUrl);

//		//this.loadSplittedContentSide(this.$splittedBody.$left, leftUrl);


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
//};
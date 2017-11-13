function AppLayout() {
	this.url = null;
	this.ownerTabs = null;
	this.ownerTabLI = null;
	this.container = null;
	this._initializers = [];
	this.options = null;

	this.title = null;
	this.subtitle = null;
	this.events = {};

	this.onActivate = null;
	this.onLoaded = null;
	this.onLayoutLoaded = null;

	this._initializers.push(function(self){
		var header = self.container.find('> div .layout_header');

		self.title = header.find('.layout_title');
		self.subtitle = header.find('.layout_subtitle');

	});
}






AppLayout.prototype = {
	setTitle: function(content) {
		this.title.html(content);
	},

	setSubTitle: function(content) {
		if(content)
			this.subtitle.html(' - ' + content);
		else
			this.subtitle.html('');
	},

	setTabHoverTitle: function(title) {
		if(this.ownerTabLI)
			this.ownerTabLI.title = this.ownerTabLI.firstChild.innerHTML + ': ' + title;
	},

	getOwnerTabIndex: function() {
		var tabIndex = Array.prototype.slice.call(this.ownerTabs._titles).indexOf(this.ownerTabLI);
		return tabIndex;
	},

	activate: function() {
		//console.log('activate from AppLayout');

		this.ownerTabs.select(this.getOwnerTabIndex());
	},

	init: function() {
		for(var x = 0; x < this._initializers.length; x++)
		{
			this._initializers[x](this);
		}
	},


	load: function(url) {
		var self = this;
		App.getHtml(url, function(response){
			var tabIndex = self.getOwnerTabIndex();
			self.ownerTabs.select(tabIndex);
			self.ownerTabs.setContentAt(tabIndex, response);

			if(self.onLayoutLoaded)
				self.onLayoutLoaded();


			if(self.onLoaded)
				self.onLoaded();


			//if(onLoaded)
				//onLoaded(self);
			//if(onActivate)
			//	onActivate(loaded.$container);
		});
	}
}




//AppLayout.prototype.init = function() {
//	console.log('initing AppLayout');
//};



function AppLayoutForm() {
	//AppLayout.call(this);
	this.AppLayout(this);
	this.editDataUrl = null;
	this.form = null;
	this.grids = Object.create(null);
	this.editingId = 0;
	this.onEditStarted = null;
	this.onEditNewStarted = null;
	this.onSaved = null;

	
	this._initializers.push(function(self){
		var $form = self.container.find('#' + self.options.formId).jqxForm();
		self.form = $form.instance;
		//self.form = self.container.find('#' + self.options.formId).jqxForm();


		var $button = self.container.find('>div >div button');
		self.form.$errorMessageContainer = $button.parent().find('.input_error');
		$button.form = self.form;

		$button.click(function(){
			App.postFormJsonFromButton($button, function(response){
				self.form.setWidgetValue('id', response.id);
				self.editingId = response.id;
				if(self.onSaved)
					self.onSaved();

				if(self.options.disableTabsOnEditNew)
					self.enableAllTabs();

				self.setSubTitle(response.description);
				self.setTabHoverTitle(response.description);

			}, null, {popupErrorMessage: true, resetFormErrors : true});
		});

		$button.jqxButton({template: 'info'});
	});
}




//AppLayoutForm.prototype = Object.create(AppLayout.prototype);
//AppLayoutForm.prototype.base = AppLayout.prototype;



//setBase(AppLayoutForm, AppLayout);




inherits(AppLayoutForm, AppLayout, true, {
	reset : function() {
		this.form.reset();
		var keys = Object.keys(this.grids);
		var key;
		for(var x = 0; x < keys; x++)
		{
			key = keys[x];
			if(!(key in this.form.widgets))
			{
				this.grids[key].clear();
			}
		}
	},

	edit : function(id, options) {
		options = options || {};
		if(!options.skipReset)
			this.reset();

		this.editingId = id;
		this.activate();
		var arr = this.url.split('/');
		arr.pop();

		//var url = arr.join('/') + '/EditData/' + id;
		var self = this;

		//onsole.log(this.events);

		App.get(this.editDataUrl + '/' + id, function(response){
			if(options.onDataLoaded)
				options.onDataLoaded();


			if(response.description)
			{
				self.setSubTitle(response.description);
				self.setTabHoverTitle(response.description);
			}
			self.form.fromJson(response.record);
			

			if(options.initialValues)
				self.form.updateWidgetsValue(options.initialValues);

			if(self.onEditStarted)
				self.onEditStarted();
		});
	},


	editNew : function(options) {
		this.editingId = 0;
		this.activate();
		if(!options.skipReset)
			this.reset();
		this.setSubTitle('NUEVO REGISTRO');
		this.setTabHoverTitle('NUEVO REGISTO');

		if(options.initialValues)
			this.form.updateWidgetsValue(options.initialValues);

		if(this.onEditNewStarted)
			this.onEditNewStarted();
	},

	addFormInitializer : function(initializer) {
		var form = this.form;
		this._initializers.push(function(self){
			initializer(self.form);
		});
	},


	addGridInitializer : function(initializer) {
		//var form = this.form;
		var options = initializer();
		this._initializers.push(function(self){
			//options.addToForm = self.form;
			var $element = self.container.find(options.elementId);
			options.$element = $element;

			var appGrid = new AppGrid(options);

			if($element[0].attributes.field_name)
				self.grids[$element[0].attributes.field_name.value] = appGrid;

			//initializer(self.form);
		});
	}
});


//var xx = new AppLayoutForm();










//AppLayoutForm.prototype.edit = function(id, onDataLoaded) {
//	this.editingId = id;
//	this.activate();
//	var arr = this.url.split('/');
//	arr.pop();

//	var url = arr.join('/') + '/EditData/' + id;
//	var self = this;

//	console.log(this.events);

//	if(this.events.onEditStarted)
//		this.events.onEditStarted();

//	App.get(url, function(response){
//		if(onDataLoaded)
//			onDataLoaded();


//		if(response.subtitle)
//		{
//			self.setSubTitle(response.subtitle);
//			self.setTabHoverTitle(response.subtitle);
//		}
//		self.form.fromJson(response.record);
//	});
//};


//AppLayoutForm.prototype.editNew = function() {
//	this.editingId = 0;
//	this.activate();
//	this.form.reset();
//};


//AppLayoutForm.prototype.addFormInitializer = function(initializer) {
//	var form = this.form;
//	this._initializers.push(function(self){
//		initializer(self.form);
//	});
//};


//AppLayoutForm.prototype.addGridInitializer = function(initializer) {
//	//var form = this.form;
//	var options = initializer();
//	this._initializers.push(function(self){
//		//options.addToForm = self.form;
//		var $element = self.container.find(options.elementId);
//		options.$element = $element;

//		var appGrid = new AppGrid(options);

//		if($element[0].attributes.field_name)
//			self.grids[$element[0].attributes.field_name.value] = appGrid;

//		//initializer(self.form);
//	});
//}

//AppLayoutForm.prototype.activate = function() {
//	console.log('activate from AppLayoutForm');
//};


//AppLayoutForm.prototype.constructor = AppLayoutForm;



function AppLayoutTabbedForm() {
	//AppLayoutForm.call(this);
	this.AppLayoutForm(this);
	this.tabs = null;
	this.tabInitializers = {};

	this._initializers.push(function(self){
		//self.form = self.container.find('#' + self.options.formId).jqxForm();

		self.tabs = self.container.find('> div > div.body > div').jqxTabs({
			width: '100%', height: '100%',
			initTabContent: function (tab) {
				//alert('initTabContent: ' + tab);
				if (self.tabInitializers[tab])
					self.tabInitializers[tab](this);

			}
		}).data('jqxWidget');

		if(self.options.disableTabsOnEditNew)
			self.disableAllTabs();
	});
}


//AppLayoutTabbedForm.prototype = Object.create(AppLayoutForm.prototype);

//AppLayoutTabbedForm.prototype.activate = function() {
//	console.log('activate from AppLayoutTabbedForm');
//};


inherits(AppLayoutTabbedForm, AppLayoutForm, true, {
	editNew: function(options) {
		//AppLayoutForm.prototype.editNew.call(this);
		this.tabs.select(0);
		this.AppLayoutForm.editNew(this, options);

		if(this.options.disableTabsOnEditNew)
			this.disableAllTabs();
	},

	edit: function(id, options) {
		var onDataLoaded = null

		if(this.options.disableTabsOnEditNew)
		{
			var self = this;
			options = options || {};
			options.onDataLoaded = function() {
				self.enableAllTabs();
			}
		}


		//AppLayoutForm.prototype.edit.call(this, id, onDataLoaded);
		this.AppLayoutForm.edit(this, id, options);
	},

	//init: function() {
	//	AppLayoutForm.prototype.init.call(this);
	//	console.log('initing AppLayoutTabbedForm');

	//},

	disableAllTabs: function(){
		this.tabs.select(0);
		var length = this.tabs.length();
		for(var x = 1; x < length; x++)
			this.tabs.disableAt(x);
	},

	enableAllTabs: function(){
		this.tabs.select(0);
		var length = this.tabs.length();
		for(var x = 1; x < length; x++)
			this.tabs.enableAt(x);
	}

});

/*


AppLayoutTabbedForm.prototype.editNew = function() {
	AppLayoutForm.prototype.editNew.call(this);

	if(this.options.disableTabsOnEditNew)
		this.disableAllTabs();
};


AppLayoutTabbedForm.prototype.edit = function(id) {
	var onDataLoaded = null

	if(this.options.disableTabsOnEditNew)
	{
		var self = this;
		onDataLoaded = function() {
			self.enableAllTabs();
		}
	}


	AppLayoutForm.prototype.edit.call(this, id, onDataLoaded);


};







AppLayoutTabbedForm.prototype.constructor = AppLayoutTabbedForm;

AppLayoutTabbedForm.prototype.init = function() {
	AppLayoutForm.prototype.init.call(this);
	console.log('initing AppLayoutTabbedForm');

};


AppLayoutTabbedForm.prototype.disableAllTabs = function(){
	this.tabs.select(0);
	var length = this.tabs.length();
	for(var x = 1; x < length; x++)
		this.tabs.disableAt(x);
};

AppLayoutTabbedForm.prototype.enableAllTabs = function(){
	this.tabs.select(0);
	var length = this.tabs.length();
	for(var x = 1; x < length; x++)
		this.tabs.enableAt(x);
};


*/
//setBase(AppLayoutTabbedForm, AppLayoutForm);






















function AppTabbedLayoutForm(options) {

	this.grids = {};
	this.tabs;
	this.tabInitializers = {};
	this.$container = options.$container;
	this.onSaved = null;
	this.onDataLoaded = null;
	this.$form = null;
	this.formId = options.formId;

	this.$button = this.$container.find('>div >div button');

	this._options = options;
};

/*
PENDING
AppTabbedLayoutForm.prototype._defaultOptions = {}
*/


//AppTabbedLayoutForm.prototype.initForm = function() {
//	var $form = this.$container.find('#' + this.formId).jqxForm();



//	$form.$errorMessageContainer = this.$button.parent().find('.input_error');


//	this.$button.$form = $form;
//	var self = this;
//	this.$button.click(function(){
//		App.postFormJsonFromButton(self.$button, function(response){
//			self.$form.setWidgetValue('id', response.id);
//			if(self.onSaved)
//				self.onSaved();
//		}, null, {popupErrorMessage: true, resetFormErrors : true});
//	});

//	this.$button.jqxButton({template: 'info'});

//	this.$form = $form;


//};

//AppTabbedLayoutForm.prototype.initTabs = function(){
//	var self = this;
//	this.tabs = this.$container.find('> div > div.body > div').jqxTabs({
//		width: '100%', height: '100%',
//		initTabContent: function (tab) {
//				if (self.tabInitializers[tab])
//					self.tabInitializers[tab](this);

//		}
//	}).data('jqxWidget');
//};


//AppTabbedLayoutForm.prototype.enableAllTabs = function(){
//	var length = this.tabs.length();

//	for(var x = 1; x < length; x++)
//		this.tabs.enableAt(x);
//};

//AppTabbedLayoutForm.prototype.disableAllTabs = function(){
//	var length = this.tabs.length();

//	for(var x = 1; x < length; x++)
//		this.tabs.disableAt(x);

//	this.tabs.select(0);
//};

//AppTabbedLayoutForm.prototype.reset = function() {
//	this.$form.reset();
//	if(this._options.onResetDisableTabs)
//		this.disableAllTabs();
//};


//AppTabbedLayoutForm.prototype.loadData = function(response) {
//	this.$form.fromJson(response.record);
//	if(this.onDataLoaded)
//		this.onDataLoaded();
//};
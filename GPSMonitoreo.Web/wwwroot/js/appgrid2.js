function AppGridFilter(settings) {
	this.label = settings.label;
	this.widgetName = settings.widgetName;
	this.widgetOptions = settings.widgetOptions;
	this.labelWidth = settings.labelWidth;
	this.widgetWidth = settings.widgetWidth;
	this.fieldName = settings.fieldName;
}

function AppGridFilterInitializer(settings) {
	this.label = settings.label;
	this.labelWidth = settings.labelWidth;
	this.widgetWidth = settings.widgetWidth;
	this.fieldName = settings.fieldName;
	this.initialize = settings.initialize
}

function AppGrid2(options) {

	//this._options = Object.create(this._defaultOptions);
	this._options = options;
	this.grid = null;

	this._addRow = null;
	this._removeSelectedRows = null;
	this._editRow = null;
	this._reload = null;

	this.init();

	if(this._options.onInit) {
		this._options.onInit.call(this);
	}

	window.mygrid = this;

	return this;
};





AppGrid2.prototype = {

	_defaultOptions : {
		$element		: null,
		elementId		: null,
		width			: '100%',
		height			: '100%',
		addToForm		: null,
		addToLayoutForm	: false,
		editable		: false,
		showToolBar		: false,
		selectionMode	: 'none',
		addMethod		: null,
		removeMethod	: null,
		rowEditMethod	: null,
		rowQuickViewMethod : null,
		showAddButton	: false,
		showRemoveButton: false,
		showReloadButton: false,
		showRowEditButton: false,
		showRowQuickViewButton : false,
		reloadMethod	: null,
		columns			: null,
		columnsResizable: true,
		columnGroups	: null,
		dataFields		: null,
		customVal		: null,
		postableFields	: null,
		swapable		: false,
		mode			: 'search',
		events			: {},
		searchFilters	: null,
		extraParameters	: null,
		searchUrl		: null,
		onInit			: null,
		onButtonsToolbarRendering : null
	},

	reload: function() {
		//this.grid.reload();
		if(this._reload)
			this._reload();
	},

	clear: function() {
		this.grid.clear();
	},

	clearSelection : function() {
		if(this.grid.getselectedrowindexes().length > 0)
			this.grid.clearselection();
	},

	setLocalData : function(data) {
		this.grid.refreshLocalData(data);
	},

	addRow : function(data) {
		data = data || {};
		this.grid.addrow(null, data);
	},

	removeSelectedRows: function() {
		this.grid.removeselectedrows();
	},

	partialUpdateRow: function(data) {
		this.grid.partialupdaterow(data);
	},

	search: function() {

		this.grid.clearselection();

		var postData = this._searchFiltersForm.toJson();
		this._adapter._source.data = postData;

		var paginginfo = this.grid.getpaginginformation();

		if(paginginfo.pagenum == 0)
		{
			this.grid.host.jqxGrid('source', this._adapter);
		}
		else
			this.grid.gotopage(0);

	},

	init: function(){
		var options = this._options;

		var columns = options.columns;


		var $grid = options.$element;

		var gridOptions = {};

		gridOptions.width = options.width;
		gridOptions.height = options.height;

		gridOptions.columns = options.columns;

		gridOptions.columnsresize = options.columnsResizable;

		if(options.columnGroups)
			gridOptions.columngroups = options.columnGroups;


		gridOptions.selectionmode = options.selectionMode;

		var self = this;

	
		switch(options.mode)
		{
			case 'search':
				options.showToolBar = true;
				options.showReloadButton = true;

				options.reloadMethod = function() {
					self.search();
				};

			
				var source =  {
					datafields: options.dataFields,
					datatype: 'json',
					url: options.searchUrl,
					type: 'POST',
					data: {},
					beforeprocessing: function(data)
					{
						self._adapter._source.data.recordcount = data.recordcount;
						//ComprobantesRecibidos._searchadapter._source.data.recordcount = data.recordcount;
						source.totalrecords = data.recordcount;
					},
					cache: false
				};

				var adapterSettings = {
					contentType : 'application/json',
					processData : function(data){
					},
					formatData : function(data)
					{
						var keys = Object.keys(self._searchFiltersForm.widgets);

						

						var newData = Object.create(null);
						var key;
						for(var x = 0; x < keys.length; x++)
						{
							key = keys[x];
							newData[key] = data[key];
						}

						newData.Page = data.pagenum;
						newData.PageSize = data.pagesize;
						newData.RecordCount = data.recordcount;

						if(self._options.extraParameters)
						{
							$.extend(newData, self._options.extraParameters);
						}

						return JSON.stringify(newData);
					},
					autoBind : false
				};


				self._adapter = new $.jqx.dataAdapter(source, adapterSettings);

				gridOptions.virtualmode = true;
				gridOptions.pageable = true;
				gridOptions.rendergridrows = function (params) {
					return params.data;
				};
				break;

			case 'edit':
			case 'read':
				var source_grid = {
					localdata: [],
					datatype: "array",
					datafields : options.dataFields,
					//datafields: options.datafields,
					deleterow: function (rowid, commit) {
						commit(true);
					},
					addrow: function (rowid, rowdata, position, commit) {
						commit(true);
					},
					updaterow: function (rowid, newdata, commit) {
						commit(true);
					}
				};

				var dataAdapter = new $.jqx.dataAdapter(source_grid);

				gridOptions.source = dataAdapter;
				gridOptions.editable = options.mode == 'edit';

				break;
		}

		if (options.showRowEditButton || options.showRowQuickViewButton)
		{
			this._editRow = options.rowEditMethod;

			var editWidgets = function (row, column, value, htmlElement) {
				var $element = $(htmlElement);
				$element.css('padding', '3px');

				if(options.showRowEditButton && options.rowEditMethod)
				{
					$editButton = $('<webicon class="file-4 clickable" style="margin-right:5px"></webicon>');
					$element.append($editButton);

					$editButton.on('click', function(){
						var boundRow = self.grid.getboundrowfrombutton(this);
						self._editRow(boundRow.Id, boundRow);
					});
				}

				if (options.showRowQuickViewButton && options.rowQuickViewMethod)
				{
					$viewButton = $('<webicon class="binoculars clickable" style="margin-right:5px"></webicon>');
					$element.append($viewButton);
					$viewButton.on('click', function () {
						var boundRow = self.grid.getboundrowfrombutton(this);
						//self._editRow(boundRow.id, boundRow);
						options.rowQuickViewMethod.call(self, boundRow.Id, boundRow);
						//quickview(searchgrid.grid.getboundrow(rowIndex).id);
						//alert(row.bounddata.id);
					});
				}




			};
			columns.push({ text: '', width: 100, createwidget: editWidgets, initwidget: function(){}});
		}


		if(options.swapable)
		{
			var swapWidget = function (row, column, value, htmlElement) {
				var grid = self.grid;
				$up = $('<webicon class="up-arrow-3 clickable"></webicon>');
				$down = $('<webicon class="down-arrow-3 clickable" style="margin-right:5px"></webicon>');

				var $element = $(htmlElement);
				$element.css('padding', '3px');
				$element.append($down);
				$element.append($up);

				$up.on('click', function(){
					var rowIndex = grid.getbuttonrowindex(this);

					if(rowIndex > 0)
					{
						var newRowData = grid.getcleanrowdata(rowIndex - 1);
						var newPrevRowData = grid.getcleanrowdata(rowIndex);
						grid.updaterow(rowIndex, newRowData);
						grid.updaterow(rowIndex - 1, newPrevRowData);
					}
				});

				$down.on('click', function(){
					var rowIndex = grid.getbuttonrowindex(this);
					if(rowIndex < grid.getdatainformation().rowscount-1)
					{
						var newRowData = grid.getcleanrowdata(rowIndex + 1);
						var newNextRowData = grid.getcleanrowdata(rowIndex);
						grid.updaterow(rowIndex, newRowData);
						grid.updaterow(rowIndex + 1, newNextRowData);
					}
				});
			};

			columns.push({ text: '', width: 100, createwidget: swapWidget, initwidget: function(){}});
		}


		var buttonsToolBarStyles = '';
	

		if(options.showToolBar)
		{

			gridOptions.showtoolbar = true;

			if(options.searchFilters)
			{
				gridOptions.toolbarheight = 38 + (30 * options.searchFilters.length) + ((options.searchFilters.length - 1) * 2);
				buttonsToolBarStyles = 'border-top: 1px solid #c2c2c2';
			}

			gridOptions.rendertoolbar = function ($toolbar) {
				if(options.searchFilters)
				{
					var $formContainer = $('<div class="form jqx-wide">');

					$toolbar.append($formContainer);

					var $form = $('<form>');

					$formContainer.append($form);

					var $table = $('<table>');
					var table = $table[0];

				

					$form.append($table);

					var searchFiltersRow;
					var filter;
					var tableRow;
					var tableCell;
					var widgetElement;

					$form.jqxForm();

					var searchFiltersForm = $form.instance;

					self._searchFiltersForm = searchFiltersForm;

					for(var x = 0; x < options.searchFilters.length; x++)
					{
						searchFiltersRow = options.searchFilters[x];


						tableRow = table.insertRow();

						for(var y = 0; y < searchFiltersRow.length; y++)
						{
							filter = searchFiltersRow[y];

							tableCell = tableRow.insertCell();

							if(filter.labelWidth)
							{
								tableCell.style.width = filter.labelWidth;
							}

							tableCell.innerHTML = filter.label;

							tableCell = tableRow.insertCell();

							if(filter.widgetWidth)
							{
								tableCell.style.width = filter.widgetWidth;
							}

							if(filter instanceof AppGridFilter)
							{
								if(filter.fieldName)
								{
									switch(filter.widgetName)
									{
										case 'jqxInput':
											widgetElement = document.createElement('input');
											widgetElement.setAttribute('name', filter.fieldName);
											break;

										default:
											widgetElement = document.createElement('div');
											widgetElement.setAttribute('field_name', filter.fieldName);
											break;
									}
								}

								tableCell.appendChild(widgetElement);

								searchFiltersForm.add($(widgetElement), filter.widgetName, filter.widgetOptions);
							}
							else
							{
								widgetElement = document.createElement('div');
								widgetElement.setAttribute('field_name', filter.fieldName);

								tableCell.appendChild(widgetElement);

								filter.initialize($(widgetElement), searchFiltersForm);

							}
						}
					}
				}

				var $buttonsToolBar = $('<div class="grid-toolbar-buttons" style="' + buttonsToolBarStyles + '"></div>');

				if(options.showAddButton)
				{
					var $add = $('<webicon class="placeholder-29 clickable"></webicon>');
					$buttonsToolBar.append($add);

					if(options.addMethod)
					{
						self._addRow = options.addMethod;

						$add.click(function(){
							self._addRow();
						});
					}
					else
					{
						$add.click(function(){
							self.addRow();
						});
					}
				}



				if(options.showRemoveButton)
				{
					var $remove = $('<webicon class="garbage-1 clickable"></webicon>');
					$buttonsToolBar.append($remove);


					if(options.removeMethod)
					{
						self._removeSelectedRows = options.removeMethod;
						$remove.click(function(){
							self._removeSelectedRows();
						});
					}
					else
					{
						$remove.click(function(){
							self.removeSelectedRows();
						});
					}
				}

				if(options.showReloadButton)
				{
					var $reload = $('<webicon class="arrows_repeat clickable"></webicon>');
					$buttonsToolBar.append($reload);
					$reload.click(function(){
						self.reload();
					});
				}

				$toolbar.append($buttonsToolBar);

				if(options.onButtonsToolbarRendering)
				{
					options.onButtonsToolbarRendering($buttonsToolBar);
				}

			}
		}




		//gridOptions.source = dataAdapter;

		$grid.jqxGrid(gridOptions);



	//	$grid.jqxGrid({
	//        width: '100%',
	////        source: dataAdapter,                
	////        pageable: true,
	////        autoheight: true,
	////        altrows: true,
	////        enabletooltips: true,
	////        editable: true,
	////        selectionmode: 'multiplecellsadvanced',
	//        //columns: gridOptions.columns
	//	});

		var events = this._options.events;

		var eventKeys = Object.keys(events);

		for(var x = 0; x < eventKeys.length; x++)
		{
			$grid.on(eventKeys[x], events[eventKeys[x]]);
		}

		var grid = $grid.data('jqxWidget');
		this.grid = grid;


		if(options.showToolBar && options.reloadMethod)
			this._reload = options.reloadMethod;

			//grid.setloader(options.reloadMethod);


	
		if(options.addToForm)
			options.addToForm.addWidget(grid);

		if(options.customVal)
			grid.customVal = options.customVal;
		else if(options.postableFields)
		{
			grid.customVal = function() {
				return this.getrowsforpost(options.postableFields);
			};
		}

		if(options.mode == 'search')
		{
			$grid.on('pagechanged', function(evt) {
				grid.clearselection();
			});
		}

		switch(options.mode)
		{
			case 'search':
				$grid.on('pagechanged', function(evt) {
					grid.clearselection();
				});
				break;

			case 'edit':
				$grid.on('cellbeginedit', function(evt) {
					var args = evt.args;
					grid._editingRowIndex = args.rowindex;
					grid._editingDataField = args.datafield;
				});
				break;
		}

	}
};

//AppGrid2.prototype.getGridOptions = function(){
//	var commonOptions = [
//		'width',
//		'height',
//		'editable',
//		'selectionmode',
//		'columns',
//		'columnsresize'
//	];

//	var mappedOptions = {
//		showToolBar : 'showtoolbar',


	
	
//	};

//	var gridOptions = {};
	

//	var key;

//	for(var x = 0; x < commonOptions.length; x++)
//	{
//		key = commonOptions[x];
//		if(this._options[key] !== undefined && this._options[key] !== null)
//		{
//			gridOptions[key] = this._options[key];
//		}
//	}

	
		
//	return gridOptions;
//};

AppGrid2.getDefaultOptions = function(){

	var ret = {};
	var keys = Object.keys(AppGrid2.prototype._defaultOptions);
	var key;
	for(var x = 0; x < keys.length; x++)
	{
		key = keys[x];
		ret[key] = AppGrid2.prototype._defaultOptions[key];
	}


	return ret;
	
	//return $.extend({}, AppGrid2.prototype._defaultOptions);
};
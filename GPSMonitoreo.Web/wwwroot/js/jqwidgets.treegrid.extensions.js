$(function(){
	if($.jqx._jqxTreeGrid)
	{

		$.jqx._jqxTreeGrid.prototype._getAllRows = function(rows, allRows) {
			
			var row;

			for(var x = 0; x < rows.length; x++)
			{
				row = rows[x];

				allRows.push(row);

				if(row.records.length)
				{
					this._getAllRows(row.records, allRows);
				}
			}
		}

		$.jqx._jqxTreeGrid.prototype._getAllRowsDictionary = function(rows, allRows) {
			
			var row;

			for(var x = 0; x < rows.length; x++)
			{
				row = rows[x];

				allRows[row.uid] = row;

				if(row.records.length)
				{
					this._getAllRowsDictionary(row.records, allRows);
				}
			}
		}

		$.jqx._jqxTreeGrid.prototype.getAllRows = function(){
			var rows = this.getRows();
			
			var allRows = [];

			this._getAllRows(rows, allRows);

			return allRows;

		};

		$.jqx._jqxTreeGrid.prototype.getAllRowsDictionary = function(){
			var rows = this.getRows();
			
			var allRows = {};

			this._getAllRowsDictionary(rows, allRows);

			return allRows;

		};

		$.jqx._jqxTreeGrid.prototype.getRowByKey = function(key){
			return this.base.rowsByKey[key];
		};

		$.jqx._jqxTreeGrid.prototype.getCleanRowData = function(rowId){
			var rowData = this.getRowByKey(rowId);

			var keys = Object.keys(rowData);
			var skipKeys = ['children','uid','records','level','parent','data','_visible','leaf'];
			var key;

			var newRowData = {};

			for(var x = 0; x < keys.length; x++)
			{
				key = keys[x];

				if(skipKeys.indexOf(key) == -1)
				{
					newRowData[key] = rowData[key];
				}
			}

			return newRowData;
		};


		$.jqx._jqxTreeGrid.prototype.partialUpdateRow = function(rowId, data){
			var newRowData = this.getCleanRowData(rowId);

			

			var keys = Object.keys(data);
			var key;

			for(var x = 0; x < keys.length; x++)
			{
				key = keys[x];
				newRowData[key] = data[key];
			}
			this.updateRow(rowId, newRowData);
		};

		$.jqx._jqxTreeGrid.prototype.displayError = function(error){
			console.log(error);
			var popOver = this._errorPopOver;
			if(!popOver)
			{
				console.log(this);
				var $parent = this.base.host.parent();
				var $before = $('<div>');
				var $div = $('<div><div></div></div>');

				//this.content.children().first().before($before);
				this.base.content.before($before);
				//this.host.before($before);

				//var $divContent = $('<div>');
				$parent.append($div);
				//$div.append($divContent);
				$div.jqxPopover({title: 'Errores', showCloseButton: true, autoClose: false, selector: $before, width: 500});
				popOver = $div.data('jqxWidget');
				//popOver._$divContent = $divContent;
				////$("#popover").jqxPopover({offset: {left: -50, top:0}, arrowOffsetValue: 50, title: "Employees", showCloseButton: true, selector: $("#button") });
				popOver._content.parent().addClass('err');
				popOver._content.parent().css('background-color', 'white');
				popOver.host.detach();
				popOver.host.addClass('jqx-popover-relative-centered');
				$before.append(popOver.host);

				this._errorPopOver = popOver;
			}

			var error_html = '';

			if(error.push) //is array
			{
				var err;
				
				error_html = '<table class="grid_table_error">';
				var columns = this.columns.records;
				var column;
				var y;
				var inner_table;
				var errors;
				var inner_error;

				for(var x = 0; x < error.length; x++)
				{
					err = error[x];
					errors = err.errors;

					error_html += '<tr><td style="width: 50px;vertical-align:top">Fila #' + (err.rowIndex + 1) + '</td><td>' ;

					inner_table = '<table>';

					for(y = 0; y < columns.length; y++)
					{
						column = columns[y];

						

						if(column.datafield)
						{
							inner_error = errors[column.datafield];

							if(inner_error)
							{
								inner_table += '<tr><td style="width:100px">' + column.text + '</td>';
								inner_table += '<td>' + inner_error.error + '</td></tr>';
							}
						}
					}

					inner_table += '</table>';
				
					error_html += inner_table + '</td></tr>';
				}
				error_html += '</table>';
			}
			else if(error.ItemsErrors)
			{
				var itemError;
				var itemErrors;
				error_html = error.Error + '<br/>';
				error_html += '<table class="grid_table_error">';
				var columns = this.columns.records;
				var column;
				var y;
				var inner_table;
				
				var inner_error;

				var itemsErrors = error.ItemsErrors;

				for(var x = 0; x < itemsErrors.length; x++)
				{
					itemError = itemsErrors[x];
					//errors = err.errors;
					itemErrors = itemError.Errors;

					error_html += '<tr><td style="width: 50px;vertical-align:top">Fila #' + (itemError.Index + 1) + '</td><td>' ;

					if(itemErrors)
					{
						inner_table = '<table style="width:100%">';

						for(y = 0; y < columns.length; y++)
						{
							column = columns[y];

							if(column.datafield)
							{
								inner_error = itemErrors[column.datafield];

								if(inner_error)
								{
									inner_table += '<tr><td style="width:100px">' + column.text + '</td>';
									inner_table += '<td>' + inner_error.Error + '</td></tr>';
								}
							}
						}

						inner_table += '</table>';
				
						error_html += inner_table
					}
					else
					{
						error_html += itemError.Error;
					}

					error_html += '</td></tr>';
				}
				error_html += '</table>';

				
			}
			else
			{
				error_html = error.error || error.Error;
			}

			popOver._content.html(error_html);
			popOver.open();

		};

		$.jqx._jqxTreeGrid.prototype.cleanError = function(error){
			if(this._errorPopOver)
			{
				this._errorPopOver._content.html('');
				this._errorPopOver.close();
			}
		};

		$.jqx._jqxTreeGrid.prototype.refreshLocalData = function (data) {
			this.base.clear();
			this.base.source._source.localdata = data;
			this.base.source._source.localData = data;
			this.base.host.jqxTreeGrid('source', this.base.source);
			this.base.updateBoundData();
		};
	}
});


function CommonStructure(parent, basePath, tab_title)
{
	this.parent = parent;
	this.basePath = basePath;
	this.tab_title = tab_title;
}



function CommonGeneral (baseUrl, title, children) {
	this.baseUrl = baseUrl;
	this.title = title;

	var keys = Object.keys(children);
	//derived.prototype.base = Object.create(null);
	var key;
	var child;
	for(var x = 0; x < keys.length; x++)
	{
		key = keys[x];
		child = children[key];


		if(child.edit)
		{
			this[key + 'edit'] = function(k) {
				return function(id, options) {
					App.popupEdit('/general/' + this.baseUrl + '/' + k + 'edit', id, options);
				};
			}(key);
		}

		if(child.search)
		{
			this[key] = function(k, c) {
				return function(id) {
					App.loadTab('/general/' + this.baseUrl + '/' + k, 'Adm.Gen./' + this.title + '/' + c.title);
				};
			}(key, child);

		}

		if(child.tree)
		{
			this[key + 'tree'] = function(k, c) {
				return function(id) {
					//App.loadTab('/general/' + this.baseUrl + '/' + k + 'tree', 'Adm.Gen./' + this.title + '/Jerarquia ' + child.title);
					App.loadLayout('/general/' + this.baseUrl + '/' + k + 'tree', 'Adm.Gen./' + this.title + '/Jerarquia ' + child.title);
				};
			}(key, child);
		}

		//console.log(key);
		
	}
}

CommonStructure.prototype = {
	edit	: function(id, onLoaded){
		App.loadTabbedCommonEdit(this, id, 'Edición', onLoaded);
	},
	index	: function(){
		App.loadTabbedCommon(this, 'Búsqueda');
	},
	windowSearch : function(onSelect, options){
		App.openWindowSearch(this.basePath + '/windowsearch', onSelect, options);
	}
};

function applyCommonControllerActions(target, controllerName, controllerSetting) {
	var controllerActions = Object.create(null);
	target[controllerName] = controllerActions;

	controllerActions._basePath = target.basePath + '/' + controllerName;


	if(!controllerSetting.actions || controllerSetting.actions.create !== false)
	{
		controllerActions.editNew = function(options) {
			options = options || {};
			var additionalQueryString = options.additionalQueryString ? '/' + options.additionalQueryString : '';

			App.loadLayoutEdit(target.basePath + '/' + controllerName + '/editform' + additionalQueryString, null, {
				tab_title: 'Edición ' + controllerSetting.tabTitle
			});
		};
	}

	if(!controllerSetting.actions || controllerSetting.actions.edit !== false)
	{
		controllerActions.edit = function(id, options) {
			options = options || {};
			var additionalQueryString = options.additionalQueryString ? '/' + options.additionalQueryString : '';
			App.loadLayoutEdit(target.basePath + '/' + controllerName + '/editform' + additionalQueryString, id, {
				tab_title: 'Edición ' + controllerSetting.tabTitle
			});
		};
	}

	if(!controllerSetting.actions || controllerSetting.actions.index !== false)
	{
		var indexMethod = controllerSetting.actions ? controllerSetting.actions.index : null;

		if(indexMethod && indexMethod.call) //is a custom function
		{
			controllerActions.index = indexMethod;
		}
		else if(indexMethod && indexMethod.substring) //specifies the layout type
		{
			controllerActions.index = function(options) {
				options = options || {};
				
				var additionalQueryString = options.additionalQueryString ? '/' + options.additionalQueryString : '';

				//App.loadLayout(target.basePath + '/' + controllerName + '/index', controllerSetting.tabTitle, indexMethod, options);
				
				App.loadLayoutByType(target.basePath + '/' + controllerName + additionalQueryString, indexMethod, {
					tab_title: controllerSetting.tabTitle
				});
			};
		}
		else
		{
			controllerActions.index = function(options) {
				options = options || {};

				var additionalQueryString = options.additionalQueryString ? '/' + options.additionalQueryString : '';

				//var layoutType = options.layoutType ? options.layoutType : 'SearchGrid';

				//App.loadLayoutByType(target.basePath + '/' + controllerName + additionalParameters, layoutType, {
				//	tab_title: controllerSetting.tabTitle
				//});

				App.loadLayoutSearchGrid(target.basePath + '/' + controllerName + additionalQueryString, {
					tab_title: controllerSetting.tabTitle
				});

				

				//if(options)
				//{
				//	if(options.additionalParameters)
				//	{
				//		additionalParameters = '/' + options.additionalParameters;
				//	}
				//}


			};
		}
	}

	controllerActions.windowSearch = function(onSelect, options) {
		App.openWindowSearch(target.basePath + '/' + controllerName + '/windowsearch', onSelect, options);
	};


	var additionalMethodsKeys;
	var additionalMethodKey;

	var additionalMethods = controllerSetting.additionalMethods;

	//console.log(additionalMethods);

	if(additionalMethods)
	{
		additionalMethodsKeys = Object.keys(controllerSetting.additionalMethods);

		for(var y = 0; y < additionalMethodsKeys.length; y++)
		{
			additionalMethodKey = additionalMethodsKeys[y];

			controllerActions[additionalMethodKey] = additionalMethods[additionalMethodKey];
		}
	}

}


function CommonStructure2(basePath, tab_title, controllers)
{
	this.basePath = basePath;
	this.tab_title = tab_title;


	var keys = Object.keys(controllers);
	//derived.prototype.base = Object.create(null);
	var key;
	var controller;
	var controllerActions;

	var self = this;

	for(var x = 0; x < keys.length; x++)
	{
		key = keys[x];
		controller = controllers[key];

		controllerActions = Object.create(null);
		
		this[key] = controllerActions;


		controllerActions.editNew = function(k) {
			return function(id, options) {
				App.loadLayoutEdit(self.basePath + '/' + k + '/editform', null, {
					tab_title: 'Edición ' + self.tab_title
				});
			};
		}(key);


		controllerActions.edit = function(k) {
			return function(id, options) {
				App.loadLayoutEdit(self.basePath + '/' + k + '/editform', id, {
					tab_title: 'Edición ' + self.tab_title
				});
			};
		}(key);



		//to be improved
		controllerActions.index = function(k) {
			return function(options) {
				App.loadTabbedCommon({basePath: self.basePath + '/' + k, tab_title: 'Búsqueda'}, self.tab_title);
			};
		}(key);


		controllerActions.windowSearch = function(k) {
			return function(onSelect, options) {
				App.openWindowSearch(self.basePath + '/' + k + '/windowsearch', onSelect, options);
			};
		}(key);

	}
}


function CommonStructure3(basePath, tab_title, controllersSettings)
{
	this.basePath = basePath;
	this.tab_title = tab_title;


	var keys = Object.keys(controllersSettings);
	//derived.prototype.base = Object.create(null);
	var key;
	var controllerSetting;
	//var controllerActions;

	var self = this;


	for(var x = 0; x < keys.length; x++)
	{
		key = keys[x];
		controllerSetting = controllersSettings[key];

		//applyCommonControllerActions(this, key, controllerSetting);

		//this.prepareController(key, controllerSetting)

		this[key] = new CommonStructureController(this, key, controllerSetting)

	}
}

function CommonStructureController(parent, controllerName, controllerSetting) {
	this.basePath = parent.basePath + '/' + controllerName;


	if(!controllerSetting.actions || controllerSetting.actions.index !== false)
	{
		var indexMethod = controllerSetting.actions ? controllerSetting.actions.index : null;

		if(indexMethod && indexMethod.call) //is a custom function
		{
			this.index = indexMethod;
		}
		else if(indexMethod && indexMethod.substring) //specifies the layout type
		{
			this.index = function(options) {
				App.loadLayoutByType(this.getUrl(null, options), indexMethod, {
					tab_title: controllerSetting.tabTitle
				});
			};
		}
		else
		{
			this.index = function(options) {
				App.loadLayoutSearchGrid(this.getUrl(null, options), {
					tab_title: controllerSetting.tabTitle
				});
			};
		}
	}


	if(controllerSetting.editPopup)
	{
		if(!controllerSetting.actions || controllerSetting.actions.create !== false)
		{
			this.editNew = function(options, popupOptions) {
				App.popupEdit2(this.getUrl('popupedit', options), popupOptions);
			};
		}

		if(!controllerSetting.actions || controllerSetting.actions.edit !== false)
		{
			this.edit = function(id, options, popupOptions) {
				App.popupEdit2(this.getUrl('popupedit', options) + '/' + id, popupOptions);
			};
		}
	}
	else
	{
		if(!controllerSetting.actions || controllerSetting.actions.create !== false)
		{
			this.editNew = function(options) {
				App.loadLayoutEdit(this.getUrl('editform', options), null, {
					tab_title: 'Edición ' + controllerSetting.tabTitle
				});
			};
		}

		if(!controllerSetting.actions || controllerSetting.actions.edit !== false)
		{
			this.edit = function(id, options) {
				App.loadLayoutEdit(this.getUrl('editform', options), id, {
					tab_title: 'Edición ' + controllerSetting.tabTitle
				});
			};
		}
	}





	this.windowSearch = function(onSelect, options) {
		options = options || {};

		App.openWindowSearch(this.getUrl('windowsearch', options), onSelect, options, controllerSetting.windowSearchOptions);
	};


	var additionalMethodsKeys;
	var additionalMethodKey;

	var additionalMethods = controllerSetting.additionalMethods;

	if(additionalMethods)
	{
		additionalMethodsKeys = Object.keys(controllerSetting.additionalMethods);

		for(var y = 0; y < additionalMethodsKeys.length; y++)
		{
			additionalMethodKey = additionalMethodsKeys[y];

			this[additionalMethodKey] = additionalMethods[additionalMethodKey];
		}
	}
};

CommonStructureController.prototype.getUrl = function(methodName, options) {
	options = options || {};

	var additionalQueryString = options.additionalQueryString ? '/' + options.additionalQueryString : '';

	return this.basePath + (methodName ? '/' + methodName : '') + additionalQueryString;
}


//CommonStructure2.prototype = {
//	edit	: function(id, onLoaded){
//		App.loadLayoutEditCommon(this, id, 'Edición', onLoaded);
//	},
//	index	: function(){
//		App.loadTabbedCommon(this, 'Búsqueda');
//	},
//	windowSearch : function(onSelect, options){
//		App.openWindowSearch(this.basePath + '/windowsearch', onSelect, options);
//	}
//};



/*

App.geografico = {
	basePath	: '/geografico',
	cercas		: {
		basePath	: '/geografico/cercas',
		edit: function(id){
			//App.loadSplittedContent(this.basePath + '/menu', this.basePath + '/editar');
			App.loadSplittedCommonEdit(this, id);
		},
		index: function(){
			//App.loadSplittedContent(this.basePath + '/menu', this.basePath);
			App.loadSplittedContentCommon(this);
		}
	}
};

*/



App.geografico = {
	basePath	: '/geografico',
	cercas		: new CommonStructure(App.geografico, '/geografico/cercas', 'Geo/Cercas'),
	segmentos	: new CommonStructure(App.geografico, '/geografico/segmentos', 'Geo/Segmentos'),
	tramos		: new CommonStructure(App.geografico, '/geografico/tramos', 'Geo/Tramos'),
	rutas		: new CommonStructure(App.geografico, '/geografico/rutas', 'Geo/Rutas'),
	rutasexternas: new CommonStructure(App.geografico, '/geografico/rutasexternas', 'Geo/Rutas Externas')
};

App.geographics = new CommonStructure3('/geographics', 'Geográfico', {
	routes: { tabTitle: 'Rutas', additionalMethods : {
		routeTemplate : function() {
			App.loadLayoutEdit(this.basePath + '/routetemplate', null, {
				tab_title: 'Plantilla Rutas'
			});
		}
	}},
	sections: { tabTitle: 'Tramos' },
	segments: { tabTitle: 'Segmentos' },
	courses: { tabTitle: 'Trayectos' },
	geofences: { tabTitle: 'Cercas', windowSearchOptions: { width: 1000} }
});


App.generalparameters = new CommonStructure3('/generalparameters', 'Parámetros Generales', {
	addresslocationtype: { tabTitle: 'Tipo Ubicación', editPopup: true },
	addresstype: { tabTitle: 'Tipo Dirección', editPopup: true },
	alarmgrade: { tabTitle: 'Grado Alarmas', editPopup: true },
	alarmcategory: { tabTitle: 'Categoria Alarmas', editPopup: true },
	alarmspeedlimit: { tabTitle: 'Alarmas - Limite Velocidad'},
	brand: { tabTitle: 'Marcas', editPopup: true },
	brandcategory: { tabTitle: 'Categoria Marcas', editPopup: true },
	emailpurpose: { tabTitle: 'Email - Propósito', editPopup: true },
	emailtype: { tabTitle: 'Email - Tipo', editPopup: true },
	entitycategory: { tabTitle: 'Categoria Entidades', editPopup: true },
	entitycontacttype: { tabTitle: 'Relación Empresa', editPopup: true },
	entitypersonprofession: { tabTitle: 'Entidad - Profesón Persona', editPopup: true },
	entitypersonsalutation: { tabTitle: 'Entidad - Saludo Persona', editPopup: true },
	entityposition: { tabTitle: 'Entidad - Cargos', editPopup: true },
	entitytype: { tabTitle: 'Tipo Entidad', editPopup: true },
	equipmentcategory: { tabTitle: 'Categoria Equipos', editPopup: true },
	equipmentcapability: { tabTitle: 'Capacidad Equipos', editPopup: true },
	equipmentgroup: { tabTitle: 'Grupos Equipos', editPopup: true },
	equipmentoperationalstatus: { tabTitle: 'Estados Operativos Equipos', editPopup: true },
	geofencecategory: { tabTitle: 'Categoria Cercas', editPopup: true },
	measureunit: { tabTitle: 'Unidad de Medidas', editPopup: true },
	model: { tabTitle: 'Modelos Equipos', editPopup: true },
	phonetype: { tabTitle: 'Tipo Teléfono', editPopup: true },
	productcategory: { tabTitle: 'Categoria Marcas', editPopup: true },
	roadtraffic: { tabTitle: 'Tráfico Cercas', editPopup: true },
	routecategory: { tabTitle: 'Categoria Rutas', editPopup: true },
	sectioncategory: { tabTitle: 'Categoria Tramos', editPopup: true },
	segmentcategory: { tabTitle: 'Categoria Segmentos', editPopup: true }
});

/*
App.users = {
	basePath	: '/users',
	user		: new CommonStructure(App.geografico, '/users/user', 'Usuarios/Usuario')
};
*/

App.authorization = new CommonStructure3('/authorization', 'Accesos y Seguridades', {
	users: { tabTitle: 'Usuarios' },
	roles: { tabTitle: 'Roles' }
});

App.entidades = new CommonStructure2('/entidades', 'Entidad', {
	entidad: {},
	direccion : {} //pendiente
});

App.entities = new CommonStructure3('/entities', 'Entidad', {

	entity : { tabTitle: 'Entidad', additionalMethods : {
		personWindowSearch : function(onSelect, options) {
			App.openWindowSearch(this.basePath + '/personwindowsearch', onSelect, options);
		}
	}},

	entityalarmnotifications: {
		tabTitle: 'Notificaciones Alarmas',
		actions : { index: 'Tabbed' },
	}
});


App.equipos = {
	basePath	: '/equipos',
	equipo		: new CommonStructure(App.geografico, '/equipos/equipo', 'Equipos/Equipo'),
	gps			: new CommonStructure(App.geografico, '/equipos/gps', 'Equipos/Gps')
};

App.equipments = new CommonStructure3('/equipments', 'Equipos', {

	equipment : { tabTitle: 'Equipos' }

});

App.productos = {
	productos: new CommonStructure(App.geografico, '/productos/productos', 'Productos/Productos')
	, producto: {
		cats:function(){App.loadTab('/productos/producto/cats', 'Productos/Categorias');}
		, catsedit: function(id){App.popupEdit('/productos/producto/catsedit', id);}
	}
};

App.admin = new CommonStructure3('/admin', 'Admin', {
	alarmfilters: { tabTitle: 'Filtros Alarmas', actions : {
		index: 'TabbedForm'
	}}
});

App.general = {
	alarmas: {
		tipos: function () { App.loadTab('/general/alarmas/tipos', 'Adm.Gen./Alarmas/Tipos'); }
		, tiposedit: function (id) { App.popupEdit('/general/alarmas/tiposedit', id); }
		, aunidades: function () { App.loadTab('/general/alarmas/unidades', 'Adm.Gen./Alarmas/Unidades'); }
		, aunidadesedit: function (id) { App.popupEdit('/general/alarmas/unidadesedit', id); }
		, permans: function () { App.loadTab('/general/alarmas/permans', 'Adm.Gen./Alarmas/Permans'); }
		, permansedit: function (id) { App.popupEdit('/general/alarmas/permansedit', id); }
		, permansresets: function () { App.loadTab('/general/alarmas/permansresets', 'Adm.Gen./Alarmas/PermansResets'); }
		, permansresetsedit: function (id) { App.popupEdit('/general/alarmas/permansresetsedit', id); }
		, niveles: function () { App.loadTab('/general/alarmas/niveles', 'Adm.Gen./Alarmas/Niveles'); }
		, nivelesedit: function (id) { App.popupEdit('/general/alarmas/nivelesedit', id); }
		//, controlvelocidad: function () { App.loadTab('/general/alarmas/controlvelocidad', 'Adm.Gen./Alarmas/Matrices Control Velocidad'); }
		//, controlvelocidadedit: function (id) { App.popupEdit('/general/alarmas/controlvelocidadedit', id); }
	},
	controlvelocidad: {
		basePath: '/ControlVelocidad',
		velocidad: new CommonStructure(App.general, '/general/controlvelocidad', 'General/Matrices Control Velocidad')
	},
	logtipos: {
		cats:function(){App.loadTab('/general/logtipos/cats', 'Adm.Gen./Logtipos/Tipos Logs');}
		, catsedit: function(id){App.popupEdit('/general/logtipos/catsedit', id);}
	},
	viajes: {
		estado: function () { App.loadTab('/general/viajes/estado', 'Adm.Gen./Viajes/Estado'); }
		, estadoedit: function (id) { App.popupEdit('/general/viajes/estadoedit', id); }
		, geografico: function () { App.loadTab('/general/viajes/geografico', 'Adm.Gen./Fases/Geografico'); }
		, geograficoedit: function (id) { App.popupEdit('/general/viajes/geograficoedit', id); }
		, operacionruta: function () { App.loadTab('/general/viajes/operacionruta', 'Adm.Gen./Fases/Operacion Ruta'); }
		, operacionrutaedit: function (id) { App.popupEdit('/general/viajes/operacionrutaedit', id); }
	},

	equipos: new CommonGeneral('equipos', 'Equipos', {
		cats: {edit: false, search: false, title: 'Categorias', tree: true} /*solo por el tree, despues mover a general parameters*/
	}),

	//Temporal, solo lo usamos por la Visualizacion Jerarquica.  Hay que mover a generalparameters
	geografico: new CommonGeneral('geografico', 'Geografico', {
		cercas: { edit: false, search: false, title: 'Cercas Categorias', tree: true },
		segmentos: { edit: false, search: false, title: 'Segmentos Categorias', tree: true },
		tramos: { edit: false, search: false, title: 'Tramos Categorias', tree: true },
		rutas: { edit: false, search: false, title: 'Rutas Categorias', tree: true }
	}),

	marcasmodelos: new CommonGeneral('marcasmodelos', 'MarcasModelos', {
		categorias: { edit: false, search: false, title: 'Categorias', tree: true } /*solo por el tree, despues mover a general parameters*/
	}),

	otros: new CommonGeneral('otros', 'Otros', {
		unidades: { edit: false, search: false, title: 'Unidades', tree: true } /*temporal, solo es necesario por tree, luego mover a generalparameters*/
	})
};



//ORIGINAL:
App.general.localidades = {
	basePath: '/localidades',
	paises: new CommonStructure(App.general.localidades, '/localidades/paises', 'Localidades/Paises'),	
	provincias: new CommonStructure(App.general.localidades, '/localidades/provincias', 'Localidades/Provincias'),
	ciudades: new CommonStructure(App.general.localidades, '/localidades/ciudades', 'Localidades/Ciudades')
}

//ok
/*App.general.paises = {
	pais: function () { App.loadTab('/general/paises/pais', 'Adm.Gen./Localidades/Paises'); },
	paisedit: function (id) { App.popupEdit('/general/paises/paisedit', id); }
}*/

//error: se movió el controlador paisescontroller dentro de la carpeta localidades y se cambió el namespace del controlador
/*
App.localidades.paises = {
	pais: function () { App.loadTab('/general/paises/pais', 'Adm.Gen/Localidades/Pais'); },
	paisedit: function (id) { App.popupEdit('/general/paises/paisedit', id);}
}*/

/*App.general.localidades = {
	basePath: '/localidades',	
	provincias: new CommonStructure(App.localidades, '/localidades/provincias', 'Localidades/Provincias'),
	ciudades: new CommonStructure(App.localidades, '/localidades/ciudades', 'Localidades/Ciudades'),
	paises: new CommonStructure(App.general.localidades, '/localidades/paises', 'Localidades/Paises'),
}*/



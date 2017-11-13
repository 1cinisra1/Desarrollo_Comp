
/*
Function.prototype.clone = function() {
    var cloneObj = this;
    if(this.__isClone) {
      cloneObj = this.__clonedFrom;
    }

    var temp = function() { return cloneObj.apply(this, arguments); };
    for(var key in this) {
		console.log('key: ' + key);
        temp[key] = this[key];
    }

    temp.__isClone = true;
    temp.__clonedFrom = cloneObj;

    return temp;
};
*/

/*
Function.prototype.clone2 = function() {
	var self = this;
    var temp = function() {
		self.apply(self, arguments);
	};

    return temp;
};
*/

function cloneFunction(func) {
	var cloned = function(){
		func.call(this);
	};
	return cloned;
};

function extend(target, source)
{

	var keys = Object.keys(source);
	//derived.prototype.base = Object.create(null);
	var key;
	for(var x = 0; x < keys.length; x++)
	{
		key = keys[x];
		//console.log(key);
		target[key] = source[key];
	}
}

function setBase(derived, base)
{
	var keys = Object.keys(base.prototype);
	derived.prototype.base = Object.create(null);
	var key;
	for(var x = 0; x < keys.length; x++)
	{
		key = keys[x];
		//console.log(key);
		derived.prototype.base[key] = base.prototype[key];
	}
}





function inherits(derived, base, addBase, extendPrototype)
{
	derived.prototype = Object.create(base.prototype);
	derived.prototype.constructor = derived;
	

	if(extendPrototype)
	{
		extend(derived.prototype, extendPrototype);
	}
	
	if(addBase)
	{
		var baseName;
		
		if(base._name)
			baseName = base._name;
		else
		{
			baseName = (base.name && base.name) || base.toString().match(/function ([^\(]+)/);
			base._name = baseName;
		}
		

		var baseProto = function(self) {
			base.apply(self, Array.prototype.slice.call(arguments, 1));
		};
		
		
		baseProto._isBase = true;
		
		derived.prototype[baseName] = baseProto;
		
		var keys = Object.keys(base.prototype);
		var key;
		var f;
		for(var x = 0; x < keys.length; x++)
		{
			key = keys[x];
			
			
			if(key != 'constructor' && !base.prototype[key]._isBase)
			{
				//console.log(key);
				
				baseProto[key] = function(f) {
					return function(self){
						f.apply(self, Array.prototype.slice.call(arguments, 1));
						
					};
				}(base.prototype[key]);					
			}
		}		
	}

}

function inherits_old(derived, base, addBase, extendPrototype)
{
	derived.prototype = Object.create(base.prototype);
	derived.prototype.constructor = derived;
	

	if(extendPrototype)
	{
		extend(derived.prototype, extendPrototype);
	}
	
	if(addBase)
	{
		var self = null;
		var baseName;
		
		if(base._name)
			baseName = base._name;
		else
		{
			baseName = (base.name && base.name) || base.toString().match(/function ([^\(]+)/);
			base._name = baseName;
		}
		

		var clonedBase;
		
		clonedBase = function(){
			//console.log('-----------------');
			//console.log(clonedBase);
			console.log('clonedBase: ' + baseName);
			//console.log('-----------------');
			//console.log(this);
			self = this;
			var self2 = this;
			base.apply(this, arguments);
			//clonedBase.getThis = function(){return self2;};
			return this;
		};
		
		//console.log(clonedBase);
		
		clonedBase._isBase = true;

		//derived.prototype.base = clonedBase;
		
		derived.prototype[baseName] = clonedBase;
		
		/*
		if(base.prototype.base)
			derived.prototype.base.base = base.prototype.base;*/

/*
		derived.prototype.base = Object.create(null);
		derived.prototype.base.constructor = base;
		*/
		//derived.prototype.base = base.prototype;
		
		//console.log(clonedBase.getThis);
		
		
		var keys = Object.keys(base.prototype);
		var key;
		var f;
		for(var x = 0; x < keys.length; x++)
		{
			key = keys[x];
			
			if(key != 'constructor' && !base.prototype[key]._isBase)
			{
				//console.log('key: ' + key);
				
				clonedBase[key] = function(f, cl) {
					//base.prototype[key].apply(self, arguments);
					//f.apply(self, arguments);
					return function(){
						//console.log('returning');
						//console.log(clonedBase.getThis());
						//f.apply(self, arguments);
						var self = cl();
						f.apply(self, arguments);
					}
				}(base.prototype[key], clonedBase);				
				
				
				//clonedBase[key] = base.prototype[key];				
			}
		}

	}
}



function MyBase(par1, par2) {
	//console.log('MyBase constructing');
	this.var1 = par1;
	this.var2 = par2;
	//this.passed1 = par1;
	//this.passed2 = par2;
	//console.log('MyBase constructed');
}

MyBase.prototype.test = function(par1) {
	//console.log('var 1 is: ' + this.var1 + ', passed: ' + passedArg1);
	this.var1 = par1;
}

MyBase.prototype.test2 = function() {
	//console.log('var 2 is: ' + this.var2);
}


function MyDerived(par1, par2) {
	//console.log('MyDerived constructing');
	this.MyBase(this, par1, par2);
	//this.base.constructor.call(this, par1, par2);
	//this.MyBase.constructor.call(this, par1, par2);
	
	
	
	
	//this.MyBase(par1, par2);
	//this.base(par1, par2);
	//this.base.call(this, par1, par2);
	//this.base.constructor.call(this);
	
	//this.base.test('fabian');
	//this.base.test();
	
	//this.var3 = 3;
	//this.var4 = 4;
	
	//console.log('MyDerived constructed');
	
}

inherits(MyDerived, MyBase, true);



function MyDerived2(par1, par2) {
	//console.log('MyDerived2 constructing');
	this.MyDerived(this, par1, par2);
	//this.base.constructor.call(this, par1, par2);
	//this.MyDerived.constructor.call(this, par1, par2);
	
	
	//this.MyDerived(par1, par2);
	
	//this.base.call(this, par1, par2);
	//this.base.constructor.call(this);
	
	//console.log('MyDerived2 constructed');
	
}

inherits(MyDerived2, MyDerived, true);

MyDerived2.prototype.test = function(par1) {
	//console.log('testing');
	this.MyBase.test(this, par1);
	
	//this.MyBase.test.call(this, par1);
	
};

function timer(fun) {
	var start = Date.now();
	fun();
	console.log((Date.now() - start) + 'ms');
}


//var xx = new MyDerived2('100', '200');

function benchmarkInheritance(repeat) {
	var inst;
	for(var x = 1; x <= repeat; x++)
	{
		//inst = new MyDerived2(x, x*2);
		inst = new AppLayoutTabbedForm();
	}
}


/*


var yy = new MyDerived2('300', '300');

xx.MyBase.test(true);
xx.MyBase.test(999);


*/







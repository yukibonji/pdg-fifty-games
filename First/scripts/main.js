var context = {};

function cellWidth(){
	return 10;
}
function cellHeight(){
	return 10;
}
function cellColumns(){
	return 32;
}
function cellRows(){
	return 32;
}
function screenWidth(){
	return cellColumns() * cellWidth();
}
function screenHeight(){
	return cellRows() * cellHeight();
}
function lineSize(){
	return 12;
}

function createMapRow(columns,value){
	var row = [];
	while(row.length<columns){
		row.push(value);
	}
	return row;
}

function createMap(columns,rows,value){
	var map = [];
	while(map.length<rows){
		map.push(createMapRow(columns,value));
	}
	return map;
}

function gotFile(file){
}

function windowResized(){
	resizeCanvas(windowWidth,windowHeight);
}

function setup(){
	var canvas = createCanvas(windowWidth, windowHeight);
	canvas.drop(gotFile);
	
	context.map = createMap(cellColumns(),cellRows(),0);

	context.palette=[
			color(0,0,0),
			color(85,85,85),
			color(170,170,170),
			color(255,255,255),
			color(0,0,255),
			color(0,128,255),
			color(0,255,255),
			color(0,255,128),
			color(0,128,0),
			color(128,255,0),
			color(255,255,0),
			color(255,128,0),
			color(255,0,0),
			color(255,0,128),
			color(255,0,255),
			color(128,0,255)
		];
		
	context.cursorX = 0;
	context.cursorY = 0;
	context.cursorColor = 3;
	context.pen = false;
}

function drawMap(){
	for(var y=0;y<cellRows();++y){
		for(var x=0;x<cellColumns();++x){
			var color = context.palette[context.map[y][x]];
			noStroke();
			fill(color);
			rect(x*cellWidth(),y*cellHeight(),cellWidth(),cellHeight());
		}
	}
	noFill();
	stroke(context.palette[context.cursorColor]);
	rect(context.cursorX * cellWidth(), context.cursorY * cellHeight(), cellWidth(), cellHeight());
}

function draw(){
	clear();
	if(context.pen){
		context.map[context.cursorY][context.cursorX]=context.cursorColor;
	}
	drawMap();
	fill(0);
	text("Position: ("+context.cursorX+","+context.cursorY+")\nColor:"+context.cursorColor+"\nPen:"+context.pen,0,screenHeight()+lineSize());
}

function keyTyped(){
	if(key==='0'){
		context.cursorColor = 0;
	}else if(key==='1'){
		context.cursorColor = 1;
	}else if(key==='2'){
		context.cursorColor = 2;
	}else if(key==='3'){
		context.cursorColor = 3;
	}else if(key==='4'){
		context.cursorColor = 4;
	}else if(key==='5'){
		context.cursorColor = 5;
	}else if(key==='6'){
		context.cursorColor = 6;
	}else if(key==='7'){
		context.cursorColor = 7;
	}else if(key==='8'){
		context.cursorColor = 8;
	}else if(key==='9'){
		context.cursorColor = 9;
	}else if(key==='a'){
		context.cursorColor = 10;
	}else if(key==='b'){
		context.cursorColor = 11;
	}else if(key==='c'){
		context.cursorColor = 12;
	}else if(key==='d'){
		context.cursorColor = 13;
	}else if(key==='e'){
		context.cursorColor = 14;
	}else if(key==='f'){
		context.cursorColor = 15;
	}else if(key==='p'){
		context.pen = !context.pen;
	}else if(key===' '){
		context.map[context.cursorY][context.cursorX]=context.cursorColor;
	}else if(key==='s'){
		saveJSON(context.map,'map.json',true);
	}
}

function keyPressed(){
	if(keyCode===LEFT_ARROW){
		context.cursorX = (context.cursorX + cellColumns() - 1) % cellColumns();
		return false;
	}else if(keyCode===RIGHT_ARROW){
		context.cursorX = (context.cursorX + 1) % cellColumns();
		return false;
	}else if(keyCode===UP_ARROW){
		context.cursorY = (context.cursorY + cellRows() - 1) % cellRows();
		return false;
	}else if(keyCode===DOWN_ARROW){
		context.cursorY = (context.cursorY + 1) % cellRows();
		return false;
	}
}
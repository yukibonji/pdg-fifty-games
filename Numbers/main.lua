local image
local quads={}
local board={}
local boardColumns=40
local boardRows=40
local spriteColumns = 16
local spriteRows = 16
local boardCellMin = 0
local boardCellMax = 9
local playerX
local playerY
local gameover
function newGame()
  board={}
  while table.getn(board)<boardColumns do
    local boardColumn={}
    while table.getn(boardColumn)<boardRows do
      local cell = {value=love.math.random(boardCellMin,boardCellMax), visited=false}
      table.insert(boardColumn,cell)
    end
    table.insert(board,boardColumn)
  end
  repeat
    playerX = love.math.random(1,boardColumns)
    playerY = love.math.random(1,boardRows)
  until board[playerX][playerY].value~=0
  gameover = false
end
function love.load()
  image = love.graphics.newImage("romfont8x8.png")
  local imageWidth, imageHeight = image:getDimensions()
  local cellWidth = imageWidth / spriteColumns
  local cellHeight = imageHeight /spriteRows
  for row=1,spriteRows do
    for column=1,spriteColumns do
      local quad = love.graphics.newQuad(cellWidth * column - cellWidth, cellWidth * row - cellHeight, cellWidth, cellHeight, imageWidth, imageHeight)
      table.insert(quads,quad)
    end
  end
  newGame()
end
function love.update(dt)
  board[playerX][playerY].visited=true
  if not gameover then
    gameover = board[playerX][playerY].value == 0
    if gameover then
      love.window.showMessageBox("Sorry!","You lose! \n\nClose message box and press F2 to restart.","info",true)
    end
  end
  local allvisited = true
  for column=1,boardColumns do
    for row=1,boardRows do
      if not board[column][row].visited then
        allvisited = false
      end
    end
  end
  if allvisited and not gameover then
      love.window.showMessageBox("Congratulations!","You Win! \n\nClose message box and press F2 to restart.","info",true)
      gameover=true
  end
end
function love.draw()
  local currentValue = board[playerX][playerY].value
  for column=1,boardColumns do
    for row=1,boardRows do
      if board[column][row].visited then
        love.graphics.setColor(255,255,0)
        love.graphics.rectangle("fill",column*16-16,row*16-16,16,16)
      end
      if (column==playerX) and (row==playerY) then
        love.graphics.setColor(0,255,0)
      elseif (column==playerX) and (row==playerY-1) and (board[column][row].value==currentValue) then
        love.graphics.setColor(255,0,0)
      elseif (column==playerX) and (row==playerY+1) and (board[column][row].value==currentValue) then
        love.graphics.setColor(255,0,0)
      elseif (column==playerX-1) and (row==playerY) and (board[column][row].value==currentValue) then
        love.graphics.setColor(255,0,0)
      elseif (column==playerX+1) and (row==playerY) and (board[column][row].value==currentValue) then
        love.graphics.setColor(255,0,0)
      else
        love.graphics.setColor(192,192,192)
      end
      love.graphics.draw(image,quads[49+board[column][row].value],column*16-16,row*16-16)
    end
  end
end
function love.keypressed(key,scancode, isrepeat)
  if gameover then 
    if key=="f2" then
      newGame()
    end
  else
    local oldValue = board[playerX][playerY].value
    if key=="left" then
      if playerX>1 then
        playerX = playerX - 1
        board[playerX][playerY].value = math.abs(board[playerX][playerY].value-oldValue)
      end
    elseif key=="right" then
      if playerX<boardColumns then
        playerX = playerX + 1
        board[playerX][playerY].value = math.abs(board[playerX][playerY].value-oldValue)
      end
    elseif key=="up" then
      if playerY>1 then
        playerY = playerY - 1
        board[playerX][playerY].value = math.abs(board[playerX][playerY].value-oldValue)
      end
    elseif key=="down" then
      if playerY<boardRows then
        playerY = playerY + 1
        board[playerX][playerY].value = math.abs(board[playerX][playerY].value-oldValue)
      end
    end
  end
end

local mazeColumns = 80
local mazeRows = 80
local screenWidth = 640
local screenHeight = 640
local cellWidth = screenWidth / mazeColumns
local cellHeight = screenHeight / mazeRows
local maze

local directions = {
  north={
      nextColumn = function(c,r)
          return c
        end,
        opposite = "south",
      nextRow = function(c,r)
          return r-1
        end
    },
  east={
      nextColumn = function(c,r)
          return c+1
        end,
        opposite = "west",
      nextRow = function(c,r)
          return r
        end
    },
  south={
      nextColumn = function(c,r)
          return c
        end,
        opposite = "north",
      nextRow = function(c,r)
          return r+1
        end
    },
  west={
      nextColumn = function(c,r)
          return c-1
        end,
        opposite = "east",
      nextRow = function(c,r)
          return r
        end
    }
  }

function newMaze(columns,rows,directions)
    local maze = {}
    while #maze < columns do
      local mazeColumn = {}
      table.insert(maze,mazeColumn)
      while #mazeColumn < rows do
        local mazeCell = {}
        for k,v in pairs(directions) do
          mazeCell[k]=false
        end
        table.insert(mazeColumn,mazeCell)
      end
    end
    
    local frontier={}
    local column = math.random(1,columns)
    local row = math.random(1,rows)
    maze[column][row].inside=true
    for k,v in pairs(directions) do
      local nextColumn = v.nextColumn(column,row)
      local nextRow = v.nextRow(column,row)
      if nextColumn>=1 and nextColumn<=columns and nextRow>=1 and nextRow<=rows and not maze[nextColumn][nextRow].frontier and not maze[nextColumn][nextRow].inside then
        maze[nextColumn][nextRow].frontier = true
        table.insert(frontier,{column=nextColumn,row=nextRow})
      end
    end
    
    while #frontier > 0 do
      local index=math.random(1,#frontier)
      column = frontier[index].column
      row = frontier[index].row
      table.remove(frontier,index)
      local mazeCell = maze[column][row]
      
      local possible = {}
      for k,v in pairs(directions) do
        local nextColumn = v.nextColumn(column,row)
        local nextRow = v.nextRow(column,row)
        if nextColumn>=1 and nextColumn<=columns and nextRow>=1 and nextRow<=rows and maze[nextColumn][nextRow].inside then
          table.insert(possible,k)
        end
      end
      
      local direction = possible[math.random(1,#possible)]
      mazeCell[direction]=true
      mazeCell.inside=true
      
      local nextColumn = directions[direction].nextColumn(column,row)
      local nextRow = directions[direction].nextRow(column,row)
      
      local nextCell = maze[nextColumn][nextRow]

      nextCell[directions[direction].opposite]=true
      
      for k,v in pairs(directions) do
        nextColumn = v.nextColumn(column,row)
        nextRow = v.nextRow(column,row)
        if nextColumn>=1 and nextColumn<=columns and nextRow>=1 and nextRow<=rows and not maze[nextColumn][nextRow].frontier and not maze[nextColumn][nextRow].inside then
          maze[nextColumn][nextRow].frontier = true
          table.insert(frontier,{column=nextColumn,row=nextRow})
        end
      end
    end
    
    return maze
end

function love.load()
  maze = newMaze(mazeColumns,mazeRows,directions)
end
function love.update(dt)
end
function love.draw()
  for column=1,mazeColumns do
    for row=1,mazeRows do
      local mazeCell = maze[column][row]
      if not mazeCell.north then
        love.graphics.line(column * cellWidth-1,row*cellHeight - cellHeight, column* cellWidth-cellWidth,row*cellHeight - cellHeight)
      end
      if not mazeCell.east then
        love.graphics.line(column * cellWidth-1,row*cellHeight - 1, column* cellWidth-1,row*cellHeight - cellHeight)
      end
      if not mazeCell.south then
        love.graphics.line(column * cellWidth-1,row*cellHeight - 1, column* cellWidth-cellWidth,row*cellHeight - 1)
      end
      if not mazeCell.west then
        love.graphics.line(column * cellWidth-cellWidth,row*cellHeight - 1, column* cellWidth-cellWidth,row*cellHeight - cellHeight)
      end
    end
  end
end
function love.keypressed(key,scancode, isrepeat)
end

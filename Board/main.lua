local screenWidth=640
local screenHeight=640
local centerX = screenWidth/2
local centerY=screenHeight/2
local ringThickness=32
local farOuterRadius=screenWidth/2
local farInnerRadius=screenWidth/2 - ringThickness
local nearOuterRadius = farOuterRadius/2
local nearInnerRadius = nearOuterRadius-ringThickness
local middleOuterRadius = (farOuterRadius+nearOuterRadius)/2
local middleInnerRadius = middleOuterRadius - ringThickness

function love.load()
end
function love.update(dt)
end

function drawRaySegment(x,y,angle,near,far)
  local x1 = x + math.cos(angle) * near
  local y1 = y + math.sin(angle) * near
  local x2 = x + math.cos(angle) * far
  local y2 = y + math.sin(angle) * far
  love.graphics.line(x1,y1,x2,y2)
end

function love.draw()
  love.graphics.circle("line",centerX,centerY,farOuterRadius)
  love.graphics.circle("line",centerX,centerY,farInnerRadius)
  
  love.graphics.circle("line",centerX,centerY,middleOuterRadius)
  love.graphics.circle("line",centerX,centerY,middleInnerRadius)
  
  love.graphics.circle("line",centerX,centerY,nearOuterRadius)
  love.graphics.circle("line",centerX,centerY,nearInnerRadius)
  
  for i = 1,50 do
    drawRaySegment(centerX,centerY, i * math.pi / 25,farInnerRadius,farOuterRadius)
  end
  
  for i = 1,42 do
    drawRaySegment(centerX,centerY, i * math.pi / 21,middleInnerRadius,middleOuterRadius)
  end
  
  for i = 1,32 do
    drawRaySegment(centerX,centerY, i * math.pi / 16,nearInnerRadius,nearOuterRadius)
  end
  
end
function love.keypressed(key,scancode, isrepeat)
end
